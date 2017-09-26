using UnityEngine;
using System.Collections;

public class ShakeRandomPosition : MonoBehaviour
{

	public bool isShakePosition;
	public bool isShakeScale;
	// Use this for initialization
	void Start ()
	{
		if (isShakePosition)
		{
			iTween.ShakePosition(gameObject,iTween.Hash("amount", new Vector3(0.7f,0.7f,0f), "time", 3f,"loop",iTween.LoopType.loop));
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

