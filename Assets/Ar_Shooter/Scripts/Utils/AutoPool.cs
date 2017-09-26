using UnityEngine;
using System.Collections;

public class AutoPool : MonoBehaviour
{


    // Use this for initialization
    public void SetupPool(float time)
    {
        Invoke("Pool", time);
    }

    private void Pool()
    {
        PoolManager.ReleaseObject(gameObject);
    }

    public static void AttackPool(GameObject go, float time)
    {
        AutoPool p = go.AddComponent<AutoPool>();
        p.SetupPool(time);
    }
}
