using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour
{
	public Vector3 vt;
	public float speed;
	public bool setup;
    // Use this for initialization
    void Start()
    {
        if (!setup)
        {
            vt = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            speed = Random.Range(500, 1100);
        }
    }
	
    void Update()
    {
        transform.RotateAroundLocal(vt, speed * Time.deltaTime);
    }
	
}
