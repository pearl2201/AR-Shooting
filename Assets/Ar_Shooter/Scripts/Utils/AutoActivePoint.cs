using UnityEngine;
using System.Collections;

public class AutoActivePoint : MonoBehaviour
{

	public Transform border;
	public bool isLower;
	public GameObject content;

	public void Setup (Transform borderObject, bool isLower)
	{
		content.gameObject.SetActive (false);
		this.border = borderObject;
		this.isLower = isLower;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isLower) {
			if (transform.position.x <= border.transform.position.x) {
				content.gameObject.SetActive (true);
			}
		} else {
			if (transform.position.x >= border.transform.position.x) {
				content.gameObject.SetActive (true);
			}
		}
	}
}

