using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleInfo : MonoBehaviour {

	[Range(0,5)]
	[SerializeField]
	private float _radius;

	public float Radius
	{
		get{
			return _radius;
		}
		set{
			_radius = value;
		}
	}
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere (transform.position, _radius);

	}
}
