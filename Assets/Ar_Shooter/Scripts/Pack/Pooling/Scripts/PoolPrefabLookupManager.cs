using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PoolPrefabLookupManager : MonoBehaviour
{

    private static PoolPrefabLookupManager _instance;

    public static PoolPrefabLookupManager Instance
    {
        get
        {
            return _instance;
        }
    }
    public PrefabLookup[] arrPrefabLookup;

    private List<string> listKey;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance!=this)
            {
                Destroy(gameObject);
            }
        }
        listKey = new List<string>();
        for (int i = 0; i < arrPrefabLookup.Length; i++)
        {
            listKey.Add(arrPrefabLookup[i].key);
        }
    }
    void Start()
    {

    }

    public GameObject lookPrefab(string key)
    {
        if (listKey.Contains(key))
        {
            return arrPrefabLookup[listKey.IndexOf(key)].prefab;
        }
        else
        {
            Debug.LogError("prefab: " + key + " not in pool lookup manager");
            return null;
        }
    }

    #region Static API

    public static GameObject LookPrefab(string key)
    {

        return PoolPrefabLookupManager.Instance.lookPrefab(key);
    }
    #endregion
}
[Serializable]
public class PrefabLookup
{
    public string key;
    public GameObject prefab;
}
