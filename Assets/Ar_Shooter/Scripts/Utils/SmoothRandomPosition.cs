using UnityEngine;
using System.Collections;

public class SmoothRandomPosition : MonoBehaviour
{

	public float speed = 1.0f;
	public Vector3 range;
	private Perlin noise;
	private Vector3 localPosition;
	private Vector3 t;
	public bool setup;
	public float oldSpeed;
	public bool isNhayLoiChoi;
	public Vector3 originalRange;
	// Use this for initialization
	void Start ()
	{
		if (!setup) {
			noise = new Perlin ();
			localPosition = transform.localPosition;
			speed = Random.Range (0.3f, 0.8f);
		}
		originalRange = range;
		range = Vector3.zero;
		StartCoroutine (UpdatePos ());
	}

	// Update is called once per frame
	IEnumerator UpdatePos ()
	{
		while (true) {
			yield return new WaitForSeconds (1 / 20.0f);
			{
				while(range.x <=originalRange.x)
				{
					range.x += Time.deltaTime/4;
					range.y +=Time.deltaTime/4;

				}

			}
			if (!isNhayLoiChoi)
				transform.localPosition = localPosition + Vector3.Scale (SmoothRandom.GetVector3 (speed), range);
		}
	}

	public void SetNhayLoiChoi ()
	{

		if (!isNhayLoiChoi) {
			isNhayLoiChoi = true;
			transform.localPosition = new Vector3(0,0,0);
			Vector3 v = new Vector3(0.2f,0.2f,0f);
			iTween.ShakePosition (this.gameObject, iTween.Hash ("amount",v, "islocal", false, "looptype", iTween.LoopType.loop));
		}


	}

	public void SetDiChuyenTinh ()
	{
		if (isNhayLoiChoi)
		{
			isNhayLoiChoi = false;
			iTween it = gameObject.GetComponent<iTween> ();
			if (it != null) {
				Destroy (it);
			}
			transform.localPosition = new Vector3(0,0,0);
		}

	}
}

