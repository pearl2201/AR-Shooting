using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ClonePrefabResourceTool: MonoBehaviour
{
    public GameObject CreateFromResource(string resourceName)
    {
        return Instantiate(Resources.Load<GameObject>(resourceName));
    }

    public GameObject CreateGameObject(GameObject go)
    {
        return Instantiate(go);
    }

    public void SelfDestroy()
    {
        DestroyImmediate(this);
    }
}

