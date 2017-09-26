using System;
using System.Collections.Generic;
using MonsterLove.Collections;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public bool logStatus;
    public Transform root;

    private Dictionary<GameObject, ObjectPool<GameObject>> prefabLookup;
    private Dictionary<GameObject, ObjectPool<GameObject>> instanceLookup;
    private static PoolManager _instance;

    public static PoolManager Instance
    {
        get
        {
            return _instance;
        }
    }


    private bool dirty = false;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        prefabLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
        instanceLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
    }

    void Update()
    {
        if (logStatus && dirty)
        {
            PrintStatus();
            dirty = false;
        }
    }

    public void warmPool(GameObject prefab, int size)
    {
        if (prefabLookup.ContainsKey(prefab))
        {
            throw new Exception("Pool for prefab " + prefab.name + " has already been created");
        }
        var pool = new ObjectPool<GameObject>(() => { return InstantiatePrefab(prefab); }, size);
        prefabLookup[prefab] = pool;

        dirty = true;
    }

    public GameObject spawnObject(GameObject prefab)
    {
        return spawnObject(prefab, Vector3.zero, Quaternion.identity);
    }

    public GameObject spawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!prefabLookup.ContainsKey(prefab))
        {
           // Debug.Log("Warn Pool: " + prefab.name);
            WarmPool(prefab, 1);
        }

        var pool = prefabLookup[prefab];

        var clone = pool.GetItem();
        if (clone == null)
        {
            Debug.Log("cline.name: " + clone.name);
        }
        clone.transform.position = position;
        clone.transform.rotation = rotation;
        clone.SetActive(true);

        instanceLookup.Add(clone, pool);
        dirty = true;
        return clone;
    }

    public void releaseObject(GameObject clone)
    {
        if (clone == null)
        {
            Debug.Log("may add thang null vao day lam gi, release di");
        }

        if (clone.GetComponent<AutoPool>() != null)
        {
            Destroy(clone.GetComponent<AutoPool>());
        }
        clone.transform.SetParent(transform);
        clone.SetActive(false);

        if (instanceLookup.ContainsKey(clone))
        {
            instanceLookup[clone].ReleaseItem(clone);
            instanceLookup.Remove(clone);
            dirty = true;
        }
        else
        {
            /*
            Debug.Log("---------------------------------------------------");
            // PrintStatus();
            foreach (KeyValuePair<GameObject, ObjectPool<GameObject>> keyVal in prefabLookup)
            {
                if (keyVal.Key!=null)
                {
                    Debug.Log(keyVal.Key.name);
                }
            }
            */
            Debug.LogWarning("No pool contains the object: " + clone.name);
            //  Destroy(clone.gameObject);
        }
    }


    private GameObject InstantiatePrefab(GameObject prefab)
    {
        var go = Instantiate(prefab) as GameObject;
        if (root != null) go.transform.parent = root;
        return go;
    }

    public void PrintStatus()
    {
        foreach (KeyValuePair<GameObject, ObjectPool<GameObject>> keyVal in prefabLookup)
        {
            Debug.Log(string.Format("Object Pool for Prefab: {0} In Use: {1} Total {2}", keyVal.Key.name, keyVal.Value.CountUsedItems, keyVal.Value.Count));
        }
    }

    #region Static API

    public static void WarmPool(GameObject prefab, int size)
    {
        Instance.warmPool(prefab, size);
    }

    public static GameObject SpawnObject(GameObject prefab)
    {
        return Instance.spawnObject(prefab);
    }

    public static GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return Instance.spawnObject(prefab, position, rotation);
    }

    public static void ReleaseObject(GameObject clone)
    {
        Instance.releaseObject(clone);
    }

    #endregion
}


