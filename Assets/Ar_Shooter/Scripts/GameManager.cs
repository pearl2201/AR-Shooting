using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public DragonControl dragonPrefab;

	public List<DragonControl> dragons;

	public float deltatimeUpdateDragon;

	private static float timeSpawnDragon = 1.5f;

	public float cooldownGame;

	public int score;
	public Text txtScore;
	public Text txtCooldownGame;

	public Camera cam;

	public Vector3 destMoveCam;
	public float speedShakeCam = 1f;
	public float lerpCam;
	public bool isShakeCam;

	public int countHitMiss;

	private bool isEndGame;

	public EndGameScreen endgamePopup;
	// Use this for initialization
	void Start ()
	{
		cooldownGame = 60f;
		score = 0;
		txtScore.text = "score " + score.ToString ();
		Time.timeScale = 1;
		isEndGame = false;
		GameExtensions.COUNT_GAME++;
		if (GameExtensions.COUNT_GAME % 2 == 0) {
			GoogleMobileAdsControl.Instance.RequestInterstitial ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isEndGame) {
			deltatimeUpdateDragon += Time.deltaTime;
			if (deltatimeUpdateDragon >= timeSpawnDragon && dragons.Count <= 12) {
				deltatimeUpdateDragon = 0;
				DragonControl dragonScript = (Instantiate (dragonPrefab.gameObject)).GetComponent<DragonControl> ();
				dragons.Add (dragonScript);
				dragonScript.SetupInfo (this);
			}
			/*
		if (isShakeCam)
		{

			lerpCam += speedShakeCam * Time.deltaTime;
			cam.transform.position = Vector3.Lerp (cam.transform.position, destMoveCam, lerpCam);
			if (lerpCam>=1)
			{

				isShakeCam = false;
			}

		}
*/
			cooldownGame -= Time.deltaTime;
			txtCooldownGame.text = cooldownGame.ToString ("#00.00");
			if (cooldownGame <= 0) {
				Debug.Log ("End game");
				isEndGame = true;
				endgamePopup.gameObject.SetActive (true);
				if (Application.isMobilePlatform)
					GameCenterController.Instance.ReportScore (Mathf.Max (0, score * 3 - countHitMiss));
				
				endgamePopup.Show (Mathf.Max (0, score * 3 - countHitMiss), score, score + countHitMiss);
				UnityEngine.Time.timeScale = 0f;

			}
		} else {
			
		}


	}

	public void RemoveDragon (DragonControl dragon)
	{
		dragons.Remove (dragon);

	}


	public void AddScore ()
	{
		score += 1;
		txtScore.text = "score " + score.ToString ();
	
	}

	public void AddCountHitMiss ()
	{
		countHitMiss++;
	}
	// sau moi lan shot thi cam se bi
	public void ShakeCam ()
	{
		Debug.Log ("Shake cam");
		Vector3 posCam = transform.position;
		posCam.x += Random.Range (-0.2f, 0.2f);
		posCam.y += Random.Range (-0.2f, 0.2f);
		destMoveCam = posCam;
		Debug.Log ("dest Shake cam: " + destMoveCam.ToString ());
		lerpCam = 0;
		isShakeCam = true;
	}

	public void BackToMenu ()
	{
		Application.LoadLevel (0);
	}
}
