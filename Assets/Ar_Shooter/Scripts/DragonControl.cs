using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class DragonControl : MonoBehaviour
{

	public enum DRAGON_ANIM
	{
		FLY,
		DIE,
		HURT,
		ATTACK1,
		ATTACK2,
		IDLE,
	}

	public AnimationClip[] clips;


	public Animation dAnimation;

	public int hp;

	public enum DRAGON_STATE
	{
		FLYIN,
		FLYOUT,
		HURT,
		DYING,
		IDLE,
		ATTACK,
		REMOVE
	}

	private StateMachine<DRAGON_STATE> fsm;

	public enum MOVE_STATE
	{
		MOVE_IN,
		MOVE_OUT,
		IDLE}

	;

	public enum DIRECTION_FLY
	{
		X_MIN,
		X_MAX,
		Y_MIN,
		Y_MAX,
		Z_MIN,
		Z_MAX

	}

	private DIRECTION_FLY dirDragon;
	private MOVE_STATE moveState;

	private Vector3 destMovingIn;
	private Vector3 destMovingOut;


	private  static float speedMoving = 3f;

	private float speedRotating = 1f;
	private static float dropSpeed = 5f;
	public static float BORDER_X_MIN = -20f;
	public static float BORDER_X_MAX = 20f;
	public static float BORDER_Y_MIN = -7.75f;
	public static float BORDER_Y_MAX = 5.73f;
	public static float BORDER_Z_MIN = 6.5f;
	public static float BORDER_Z_MAX = 12.5f;

	public static float DROP_Y = -7f;
	public static float SPLIT = 6;

	public static float NOISE = 0.5f;
	private Vector3 pos;
	private Vector3 rot;

	// if shouldAttack, fly to middle and attack
	// if not shouldAttack, only fly to outside
	private bool isShouldAttack;
	private bool isMinToMax = false;

	private float timeupdateidle = 0;
	private static float MAX_TIME_IDLE = 1f;
	private Quaternion _facing;

	private Vector3 originalRotation;

	public GameManager gameManager;

	private Vector3 originScale;
	private bool isRaiseScale;
	[SerializeField]
	private AudioClip clipDie, clipAttack, clipHurt;
	[SerializeField]
	private AudioSource audioSource;

	private float destScl;
	[SerializeField]
	private SkinnedMeshRenderer skinnedMeshRenderer;
	public void SetupInfo (GameManager gameManager)
	{

		this.gameManager = gameManager;
	}

	void Start ()
	{
		speedMoving = Random.Range (2f, 5f);
		hp = Random.Range (1, 3);
		originalRotation = transform.localEulerAngles;
		_facing = transform.rotation;
		originScale = transform.localScale;
		transform.localScale = Vector3.zero;
		dirDragon = DIRECTION_FLY.Z_MAX;
		//while (dirDragon == DIRECTION_FLY.Z_MAX) 
		{
			dirDragon = (DIRECTION_FLY)Random.Range (0, 7);
		}
		pos = new Vector3 (0, 0, 0);
		// Setup start position
		if (dirDragon == DIRECTION_FLY.X_MAX || dirDragon == DIRECTION_FLY.X_MIN) {
			

			if (dirDragon == DIRECTION_FLY.X_MAX) {
				pos.x = GetRandomPosMiddle (BORDER_X_MIN - NOISE, BORDER_X_MIN + NOISE);
				isMinToMax = true;

			} else if (dirDragon == DIRECTION_FLY.X_MIN) {
				pos.x = GetRandomPosMiddle (BORDER_X_MAX - NOISE, BORDER_X_MAX + NOISE);
				isMinToMax = false;
			}

			pos.y = GetRandomPosMiddle (BORDER_Y_MIN, BORDER_Y_MAX);
			pos.z = GetRandomPosMiddle (BORDER_Z_MIN, BORDER_Z_MAX);

		} else if (dirDragon == DIRECTION_FLY.Y_MAX || dirDragon == DIRECTION_FLY.Y_MIN) {
			if (dirDragon == DIRECTION_FLY.Y_MAX) {
				pos.y = GetRandomPosMiddle (BORDER_Y_MIN - NOISE, BORDER_Y_MIN + NOISE);
				isMinToMax = true;

			} else if (dirDragon == DIRECTION_FLY.Y_MIN) {
				pos.y = GetRandomPosMiddle (BORDER_Y_MAX - NOISE, BORDER_Y_MAX + NOISE);
				isMinToMax = false;
			}

			pos.x = GetRandomPosMiddle (BORDER_X_MIN, BORDER_X_MAX);
			pos.z = GetRandomPosMiddle (BORDER_Z_MIN, BORDER_Z_MAX);
		} else if (dirDragon == DIRECTION_FLY.Z_MAX || dirDragon == DIRECTION_FLY.Z_MIN) {
			if (dirDragon == DIRECTION_FLY.Z_MAX) {
				pos.z = GetRandomPosMiddle (BORDER_Z_MIN - NOISE, BORDER_Z_MIN + NOISE);
				isMinToMax = true;

			} else if (dirDragon == DIRECTION_FLY.Z_MIN) {
				pos.z = GetRandomPosMiddle (BORDER_Z_MAX - NOISE, BORDER_Z_MAX + NOISE);
				isMinToMax = false;
			}

			if (Random.Range(0,2) == 0)
			{
				pos.y = GetRandomPosMiddle (BORDER_Y_MIN - NOISE, BORDER_Y_MIN + NOISE);
			}else
			{
				pos.y = GetRandomPosMiddle (BORDER_Y_MAX - NOISE, BORDER_Y_MAX + NOISE);
			}
			if (Random.Range(0,2) == 0)
			{
				pos.x = GetRandomPosMiddle (BORDER_X_MIN - NOISE, BORDER_X_MIN + NOISE);
			}else
			{
				pos.x = GetRandomPosMiddle (BORDER_X_MAX - NOISE, BORDER_X_MAX + NOISE);
			}
		}
		transform.position = pos;
		// setup end pos

		// setup middle pos
		isShouldAttack = Random.Range (0, 3) != 0;
		if (isShouldAttack) {
			pos.x = GetRandomPosMiddle (BORDER_X_MIN, BORDER_X_MAX);
			pos.y = GetRandomPosMiddle (BORDER_Y_MIN, BORDER_Y_MAX);
			pos.z = GetRandomPosMiddle (BORDER_Z_MIN, BORDER_Z_MAX);
		//	pos = new Vector3 (0, -1, 9);
			destMovingIn = pos;
			// setup middle pos	
		} else {
			if (dirDragon == DIRECTION_FLY.X_MAX || dirDragon == DIRECTION_FLY.X_MIN) {


				if (dirDragon == DIRECTION_FLY.X_MAX) {
					pos.x = GetRandomPosMiddle (BORDER_X_MAX - NOISE, BORDER_X_MAX + NOISE);
					isMinToMax = true;

				} else if (dirDragon == DIRECTION_FLY.X_MIN) {
					pos.x = GetRandomPosMiddle (BORDER_X_MIN - NOISE, BORDER_X_MIN + NOISE);
					isMinToMax = false;
				}

				pos.y = GetRandomPosMiddle (BORDER_Y_MIN, BORDER_Y_MAX);
				pos.z = GetRandomPosMiddle (BORDER_Z_MIN, BORDER_Z_MAX);

			} else if (dirDragon == DIRECTION_FLY.Y_MAX || dirDragon == DIRECTION_FLY.Y_MIN) {
				if (dirDragon == DIRECTION_FLY.Y_MIN) {
					pos.y = GetRandomPosMiddle (BORDER_Y_MIN - NOISE, BORDER_Y_MIN + NOISE);
					isMinToMax = true;

				} else if (dirDragon == DIRECTION_FLY.Y_MAX) {
					pos.y = GetRandomPosMiddle (BORDER_Y_MAX - NOISE, BORDER_Y_MAX + NOISE);
					isMinToMax = false;
				}

				pos.x = GetRandomPosMiddle (BORDER_X_MIN, BORDER_X_MAX);
				pos.z = GetRandomPosMiddle (BORDER_Z_MIN, BORDER_Z_MAX);
			} else if (dirDragon == DIRECTION_FLY.Z_MAX || dirDragon == DIRECTION_FLY.Z_MIN) {
				if (dirDragon == DIRECTION_FLY.Z_MIN) {
					pos.y = GetRandomPosMiddle (BORDER_Z_MIN - NOISE, BORDER_Z_MIN + NOISE);
					isMinToMax = true;

				} else if (dirDragon == DIRECTION_FLY.Z_MAX) {
					pos.y = GetRandomPosMiddle (BORDER_Z_MAX - NOISE, BORDER_Z_MAX + NOISE);
					isMinToMax = false;
				}

				if (Random.Range(0,2) == 0)
				{
					pos.y = GetRandomPosMiddle (BORDER_Y_MIN - NOISE, BORDER_Y_MIN + NOISE);
				}else
				{
					pos.y = GetRandomPosMiddle (BORDER_Y_MAX - NOISE, BORDER_Y_MAX + NOISE);
				}
				if (Random.Range(0,2) == 0)
				{
					pos.x = GetRandomPosMiddle (BORDER_X_MIN - NOISE, BORDER_X_MIN + NOISE);
				}else
				{
					pos.x = GetRandomPosMiddle (BORDER_X_MAX - NOISE, BORDER_X_MAX + NOISE);
				}
			}

			destMovingIn = pos;
		}
		Debug.Log ("is Attack: " + isShouldAttack);
		Debug.Log ("startPos: " + transform.position.ToString () + " - endpos: " + destMovingIn.ToString ());
		moveState = MOVE_STATE.MOVE_IN;
		dAnimation.Play (clips [(int)DRAGON_ANIM.FLY].name);
		//Color c = Color.HSVToRGB (Random.Range(0f,1f), Random.Range (0.0f,0.7f), Random.Range(0.0f,0.3f));
		skinnedMeshRenderer.material.color = new Color(Random.Range (0f,1f),Random.Range (0f,1f),Random.Range (0f,1f),1f);
		destScl = Random.Range (0.4f, 0.8f);
		fsm = StateMachine<DRAGON_STATE>.Initialize (this);
		fsm.ChangeState (DRAGON_STATE.FLYIN);
	}

	void Update ()
	{
		if (!isRaiseScale) {
			Vector3 tmpScale = transform.localScale;
			tmpScale += Vector3.one * Time.deltaTime * 2;
			transform.localScale = tmpScale;
			if (tmpScale.x >= destScl) {

				isRaiseScale = true;
			}


		}

	}

	public void OnHit (int damage)
	{
		Debug.Log ("hp " + hp);
		if (hp > 0 && fsm.State != DRAGON_STATE.REMOVE) {
			hp -= damage;
			if (hp > 0) {
				Debug.Log ("Hurt");
				audioSource.PlayOneShot (clipHurt);
				fsm.ChangeState (DRAGON_STATE.HURT);
			} else {
				Debug.Log ("Remove");
				audioSource.PlayOneShot (clipDie);
				gameManager.AddScore ();

				fsm.ChangeState (DRAGON_STATE.DYING,StateTransition.Overwrite);
			
			}

		}

	}

	void RemoveCharacter ()
	{
		gameManager.RemoveDragon (this);
		fsm.ChangeState (DRAGON_STATE.REMOVE);
		StartCoroutine (IERemove ());
	}

	IEnumerator IERemove()
	{
		Vector3 destScl = Vector3.zero;
		Vector3 prevScl = transform.localScale;
		float p = 0;
		while (p<=0.3f)
		{
			p += Time.deltaTime;
			transform.localScale = Vector3.Lerp (prevScl, destScl, p);
			yield return null;
		}
		Destroy (gameObject);
	}

	IEnumerator FLYIN_Enter ()
	{
		dAnimation.Blend (clips [(int)DRAGON_ANIM.FLY].name);
		Quaternion startAngle = transform.rotation;
		Vector3 dir = (destMovingIn - pos).normalized;


		var rotation = Quaternion.LookRotation (dir);
		float p = 0;
		while (p<=1)
		{
			p += Time.deltaTime / 0.3f;
			transform.rotation = Quaternion.Slerp (startAngle, rotation, p);
			yield return null;
		}

	}

	void FLYIN_Update ()
	{
		pos = transform.position;
		Vector3 dir = (destMovingIn - pos).normalized;


		var rotation = Quaternion.LookRotation (dir);
		//rotation *= _facing;
		transform.rotation = rotation;


		//Vector3 addRotation = Vector3.Cross(Vector3.up, dir);
		//transform.localEulerAngles = originalRotation + dir;

		pos = pos + dir * speedMoving * Time.deltaTime;
		transform.position = pos;
		bool finishMoving = false;
		if (Vector3.Distance (pos, destMovingIn) < 0.2f) {
			finishMoving = true;
			Debug.Log ("Cond1");
		} else {
			if (dirDragon == DIRECTION_FLY.X_MAX && (pos.x >= destMovingIn.x)) {
				finishMoving = true;
				Debug.Log ("Cond2");
			} else if (dirDragon == DIRECTION_FLY.X_MIN && (pos.x <= destMovingIn.x)) {
				Debug.Log ("Cond3");
				finishMoving = true;
			} else if (dirDragon == DIRECTION_FLY.Y_MAX && (pos.y >= destMovingIn.y)) {
				finishMoving = true;
				Debug.Log ("Cond2");
			} else if (dirDragon == DIRECTION_FLY.Y_MIN && (pos.y <= destMovingIn.y)) {
				Debug.Log ("Cond3");
				finishMoving = true;
			} else if (dirDragon == DIRECTION_FLY.Z_MAX && (pos.z >= destMovingIn.z)) {
				finishMoving = true;
				Debug.Log ("Cond2");
			} else if (dirDragon == DIRECTION_FLY.Z_MIN && (pos.z <= destMovingIn.z)) {
				Debug.Log ("Cond3");
				finishMoving = true;
			}
		}

		if (finishMoving) {
			if (isShouldAttack) {
				moveState = MOVE_STATE.IDLE;
				fsm.ChangeState (DRAGON_STATE.IDLE);
			} else {
				RemoveCharacter ();
			}
		}
	}

	IEnumerator FLYOUT_Enter ()
	{
		dAnimation.Blend (clips [(int)DRAGON_ANIM.FLY].name);
		Quaternion startAngle = transform.rotation;
		Vector3 dir = (destMovingOut - pos).normalized;


		var rotation = Quaternion.LookRotation (dir);
		float p = 0;
		while (p<=1)
		{
			p += Time.deltaTime / 0.3f;
			transform.rotation = Quaternion.Slerp (startAngle, rotation, p);
			yield return null;
		}
	}

	void FLYOUT_Update ()
	{
		pos = transform.position;
		Vector3 dir = (destMovingOut - pos).normalized;

		var rotation = Quaternion.LookRotation (dir);
		//rotation *= _facing;
		transform.rotation = rotation;

		pos = pos + dir * speedMoving * Time.deltaTime;
		transform.position = pos;
		bool finishMoving = false;
		if (Vector3.Distance (pos, destMovingOut) < 0.1f) {
			finishMoving = true;
		} else {
			if (dirDragon == DIRECTION_FLY.X_MAX && (pos.x >= destMovingOut.x)) {
				finishMoving = true;
				Debug.Log ("Cond2");
			} else if (dirDragon == DIRECTION_FLY.X_MIN && (pos.x <= destMovingOut.x)) {
				Debug.Log ("Cond3");
				finishMoving = true;
			} else if (dirDragon == DIRECTION_FLY.Y_MAX && (pos.y >= destMovingOut.y)) {
				finishMoving = true;
				Debug.Log ("Cond2");
			} else if (dirDragon == DIRECTION_FLY.Y_MIN && (pos.y <= destMovingOut.y)) {
				Debug.Log ("Cond3");
				finishMoving = true;
			} else if (dirDragon == DIRECTION_FLY.Z_MAX && (pos.z >= destMovingOut.z)) {
				finishMoving = true;
				Debug.Log ("Cond2");
			} else if (dirDragon == DIRECTION_FLY.Z_MIN && (pos.z <= destMovingOut.z)) {
				Debug.Log ("Cond3");
				finishMoving = true;
			}
		}
		if (finishMoving) {
			
			RemoveCharacter ();

		}
	}

	void DYING_Enter ()
	{
		dAnimation.Blend (clips [(int)DRAGON_ANIM.DIE].name);
	}

	void DYING_Update ()
	{
		pos = transform.position;
		pos.y -= dropSpeed * Time.deltaTime;
		transform.position = pos;
		if (pos.y < DROP_Y) {
			if (transform.localScale.x > 0) {

				transform.localScale = transform.localScale - Vector3.one * Time.deltaTime * 2;
			} else {
				RemoveCharacter ();
			}

		}
	}

	IEnumerator HURT_Enter ()
	{
		dAnimation.Blend (clips [(int)DRAGON_ANIM.HURT].name);
		yield return new WaitForSeconds (clips [(int)DRAGON_ANIM.HURT].length);
		if (moveState == MOVE_STATE.IDLE) {
			fsm.ChangeState (DRAGON_STATE.IDLE);
		} else if (moveState == MOVE_STATE.MOVE_IN) {
			fsm.ChangeState (DRAGON_STATE.FLYIN);
		} else if (moveState == MOVE_STATE.MOVE_OUT) {
			fsm.ChangeState (DRAGON_STATE.FLYOUT);
		}
	}

	IEnumerator IDLE_Enter ()
	{

		Vector3 destAngle = new Vector3 (16.1f, 180, 0);
	

		Quaternion startAngle = transform.rotation;



		var rotation = Quaternion.Euler (destAngle);
		float p = 0;
		while (p<=1)
		{
			p += Time.deltaTime / 0.3f;
			transform.rotation = Quaternion.Slerp (startAngle, rotation, p);
			yield return null;
		}

		dAnimation.Blend (clips [(int)DRAGON_ANIM.IDLE].name);
		timeupdateidle = 0;
	}

	void IDLE_Update ()
	{
		timeupdateidle += Time.deltaTime;
		if (timeupdateidle > MAX_TIME_IDLE) {
			if (moveState == MOVE_STATE.MOVE_IN) {
				fsm.ChangeState (DRAGON_STATE.FLYIN);
			} else if (moveState == MOVE_STATE.IDLE) {
				int option = Random.Range (0, 3);
				if (option == 0) {
					timeupdateidle = 0;
				} else if (option == 1) {
					fsm.ChangeState (DRAGON_STATE.ATTACK);
				} else if (option == 2) {
					{
						{
							pos = new Vector3 (0, 0, 0);
							dirDragon = DIRECTION_FLY.Y_MIN;
							while (dirDragon == DIRECTION_FLY.Y_MIN)
							{
								dirDragon = (DIRECTION_FLY)Random.Range (0, 7);
							}
							if (dirDragon == DIRECTION_FLY.X_MAX || dirDragon == DIRECTION_FLY.X_MIN) {


								if (dirDragon == DIRECTION_FLY.X_MAX) {
									pos.x = GetRandomPosMiddle (BORDER_X_MAX - NOISE, BORDER_X_MAX + NOISE);
									isMinToMax = true;

								} else if (dirDragon == DIRECTION_FLY.X_MIN) {
									pos.x = GetRandomPosMiddle (BORDER_X_MIN - NOISE, BORDER_X_MIN + NOISE);
									isMinToMax = false;
								}

								pos.y = GetRandomPosMiddle (BORDER_Y_MIN, BORDER_Y_MAX);
								pos.z = GetRandomPosMiddle (BORDER_Z_MIN, BORDER_Z_MAX);

							} else if (dirDragon == DIRECTION_FLY.Y_MAX || dirDragon == DIRECTION_FLY.Y_MIN) {
								if (dirDragon == DIRECTION_FLY.Y_MIN) {
									pos.y = GetRandomPosMiddle (BORDER_Y_MIN - NOISE, BORDER_Y_MIN + NOISE);
									isMinToMax = true;

								} else if (dirDragon == DIRECTION_FLY.Y_MAX) {
									pos.y = GetRandomPosMiddle (BORDER_Y_MAX - NOISE, BORDER_Y_MAX + NOISE);
									isMinToMax = false;
								}

								pos.x = GetRandomPosMiddle (BORDER_X_MIN, BORDER_X_MAX);
								pos.z = GetRandomPosMiddle (BORDER_Z_MIN, BORDER_Z_MAX);
							}  else if (dirDragon == DIRECTION_FLY.Z_MAX || dirDragon == DIRECTION_FLY.Z_MIN) {
								if (dirDragon == DIRECTION_FLY.Z_MIN) {
									pos.z = GetRandomPosMiddle (BORDER_Z_MIN - NOISE, BORDER_Z_MIN + NOISE);
									isMinToMax = true;

								} else if (dirDragon == DIRECTION_FLY.Y_MAX) {
									pos.z = GetRandomPosMiddle (BORDER_Z_MAX - NOISE, BORDER_Z_MAX + NOISE);
									isMinToMax = false;
								}
								if (Random.Range(0,2) == 0)
								{
									pos.y = GetRandomPosMiddle (BORDER_Y_MIN - NOISE, BORDER_Y_MIN + NOISE);
								}else
								{
									pos.y = GetRandomPosMiddle (BORDER_Y_MAX - NOISE, BORDER_Y_MAX + NOISE);
								}
								if (Random.Range(0,2) == 0)
								{
									pos.x = GetRandomPosMiddle (BORDER_X_MIN - NOISE, BORDER_X_MIN + NOISE);
								}else
								{
									pos.x = GetRandomPosMiddle (BORDER_X_MAX - NOISE, BORDER_X_MAX + NOISE);
								}
							}

						
							destMovingOut = pos;
							Debug.Log ("pos out: " + pos.ToString ());
						}

					}
					moveState = MOVE_STATE.MOVE_OUT;
					fsm.ChangeState (DRAGON_STATE.FLYOUT);
				}
			} else if (moveState == MOVE_STATE.MOVE_OUT) {
				
			}
		}
	}

	IEnumerator ATTACK_Enter ()
	{
		audioSource.PlayOneShot (clipAttack);
		if (Random.Range (0, 2) == 0) {
			dAnimation.Blend (clips [(int)DRAGON_ANIM.ATTACK1].name);
			yield return new WaitForSeconds (clips [(int)DRAGON_ANIM.ATTACK1].length);
		} else {
			dAnimation.Blend (clips [(int)DRAGON_ANIM.ATTACK2].name);
			yield return new WaitForSeconds (clips [(int)DRAGON_ANIM.ATTACK2].length);
		}

		fsm.ChangeState (DRAGON_STATE.IDLE);
	}



	public float GetDestMiddleMin (float min, float max)
	{
		return (min * 3.5f + max*6.5f) / 10;
	}

	public float GetDestMiddleMax (float min, float max)
	{
		return (min*6.5f + max * 3.5f) / 10;
	}

	public float GetRandomPosMiddle (float min, float max)
	{
		return Random.Range (GetDestMiddleMin (min, max), GetDestMiddleMax (min, max));
	}
}
