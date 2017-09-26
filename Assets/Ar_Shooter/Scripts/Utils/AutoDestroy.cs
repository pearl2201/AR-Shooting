using UnityEngine;
using System.Collections;
namespace Stater{

	public class AutoDestroy : MonoBehaviour
	{
		public float destroyTime;
		private bool isDestroy;
		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			//		Color c = transform.GetChild(0).GetComponent<Renderer>().material.GetColor("_Color");
			//		c.a -= Time.deltaTime / destroyTime;
			//		transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", c);
		}

		public void setDestroy(float time)
		{
			destroyTime = time;

			Invoke("t", destroyTime);
		}

		void t()
		{
			if (!isDestroy)
				PoolManager.ReleaseObject(gameObject);
			else
				Destroy(gameObject);
		}

		public static void Attach(GameObject go, float time)
		{
			AutoDestroy p = go.AddComponent<AutoDestroy>();
			p.setDestroy(time);
		}
	}

}
