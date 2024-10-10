using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolSystem : MonoBehaviour
{
    #region Singleton
    public static PoolSystem instance;

    #endregion

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }


    public GameObject SpawnFromPool(string tag, Transform parent)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            if (!objectToSpawn.activeInHierarchy)
            {
                objectToSpawn.transform.SetParent(parent);
                objectToSpawn.SetActive(true);
                poolDictionary[tag].Enqueue(objectToSpawn);
                return objectToSpawn;
            }
            else
            {
                return SpawnFromPool(tag, parent);
            }
        }
        else
        {
            Debug.Log("Pool with tag " + tag + " does not exist");
            return null;
        }
    }





}