using UnityEngine;
using System.Collections;

public class AutoDeactive : MonoBehaviour {
	public float deactiveTime = 1.0f;
//	public tk2dSprite sprite;
	// Use this for initialization
	void Start () {

	}

	void OnEnable() {
		//sprite.color = Color.white;
		StopCoroutine ("SetDeactive");
		StartCoroutine ("SetDeactive");
	}

	IEnumerator SetDeactive() {
		yield return new WaitForSeconds (deactiveTime);
		gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		/*
		Color c = sprite.color;
		c.a -= Time.deltaTime / deactiveTime;
		sprite.color = c;
		*/
	}
}
