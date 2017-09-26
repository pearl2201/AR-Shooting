using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIButtonClick : MonoBehaviour
{

	void Start ()
	{
		(GetComponent <Button> ()).onClick.AddListener (() => {
			Click ();
		});
	}

	public void Click ()
	{
		SoundManager.Instance.Play ("click");
	}
}

