using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLoopPosition : MonoBehaviour
{

	public Vector3 deltaPos = new Vector3 (0.2f, 0.2f, 0);
	public iTween.LoopType loopType = iTween.LoopType.pingPong;
	public iTween.EaseType easeType = iTween.EaseType.easeInOutQuad;
	public float time = 0.4f;
	// Use this for initialization
	void Start ()
	{
		Vector3 localScale = transform.localScale;

		iTween.MoveBy (gameObject, iTween.Hash ("x", deltaPos.x, "y", deltaPos.y, "time", time, "looptype", loopType, "easetype", easeType));
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
