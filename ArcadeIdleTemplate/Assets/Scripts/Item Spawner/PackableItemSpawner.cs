using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackableItemSpawner : MonoBehaviour
{

    [SerializeField] private Transform spawnTransform;

    [SerializeField] private int spawnMaxCount=10;

    private int spawnCount = 0;
    private WaitForSeconds waitTime;

    
    private void Start()
    {
        waitTime = new WaitForSeconds(.15f);
        StartCoroutine(SpawnObjects());
    }


    private IEnumerator SpawnObjects()
    {
        if(spawnCount >= spawnMaxCount) yield break;

        for (int i = 0; i < spawnMaxCount-spawnCount; i++)
        {
            spawnCount++;
            var createdOjb = PoolSystem.instance.SpawnFromPool("duck", null);
            createdOjb.transform.position = spawnTransform.position + new Vector3(Random.Range(-.25f,.75f) ,0 , Random.Range(-.25f, .75f));
            yield return waitTime;
        }
      
    }


   
}
