using UnityEngine;
using System.Collections;

public class RandomScale : MonoBehaviour
{

    public float t;
    public bool up;
    public Vector3 vScale;
    public float speedFade = 0.1f;
    public float maxFade = 0.7f;

    public float minFade = 0f;

    private float cdr = 0;

    [SerializeField]
    private bool isScaleX;
    [SerializeField]
    private bool isScaleY;
    // Use this for initialization
    void Start()
    {

        t = Random.Range(minFade, maxFade * 0.8f);
        up = Random.Range(0, 2) == 0;
        vScale = transform.localScale;
        if (isScaleX)
            vScale.x = t;
        if (isScaleY)
            vScale.y = t;
           

        speedFade = Random.Range(0, 0.2f);

    }

    void Update()
    {
        cdr += Time.deltaTime;
        if (cdr > 0.05f)
        {
            cdr = 0;
        }
        else
        {
            return;
        }

        if (up)
        {
            t += Time.deltaTime * speedFade;
            if (t >= maxFade)
            {
                t = maxFade;
                up = false;
            }

            if (isScaleX)
                vScale.x = t;
            if (isScaleY)
                vScale.y = t;

        }
        else
        {
            t -= Time.deltaTime * speedFade;
            if (t <= minFade)
            {
                t = minFade;
                up = true;
            }
            if (isScaleX)
                vScale.x = t;
            if (isScaleY)
                vScale.y = t;
        }
        transform.localScale = vScale;
    }


}

