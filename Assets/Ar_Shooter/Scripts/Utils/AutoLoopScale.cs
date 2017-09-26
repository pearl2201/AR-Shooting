using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLoopScale : MonoBehaviour {

	public float ratioScl = 1.2f;
	public iTween.LoopType loopType = iTween.LoopType.pingPong;
	public iTween.EaseType easeType = iTween.EaseType.easeInOutQuad;
	public float time = 0.4f;
	// Use this for initialization
	void Start () {
		Vector3 localScale = transform.localScale;
		Vector3 destScale = localScale * ratioScl;
		iTween.ScaleTo (gameObject, iTween.Hash ("scale", destScale,"time",time,"looptype", loopType,"easetype",easeType));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
