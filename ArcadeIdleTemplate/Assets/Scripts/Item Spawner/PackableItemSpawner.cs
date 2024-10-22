using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackableItemSpawner : MonoBehaviour
{

    [SerializeField] private Transform spawnTransform;

    [SerializeField] private int spawnMaxCount=10;
 
    private WaitForSeconds _waitTime;

    private Dictionary<string, int> _inGameCollectablesCount;

    private bool _isCurrentlySpwaning = false;
    
    private void Start()
    {
        _inGameCollectablesCount = UIManager.instance.InGameCollectablesCount;
        _waitTime = new WaitForSeconds(.15f);
        StartCoroutine(SpawnObjects("duck"));
        InvokeRepeating(nameof(CheckShouldSpawnCollectable),15,5);
    }

    public void SpawnCollectableObject(string poolName)
    { 
        StartCoroutine(SpawnObjects(poolName));
    }


    private IEnumerator SpawnObjects(string poolName)
    {
        _isCurrentlySpwaning = true;
        
        if(!_inGameCollectablesCount.ContainsKey(poolName))
            _inGameCollectablesCount.Add(poolName, 0);
        var createdItemCount = _inGameCollectablesCount[poolName];
        
        for (int i = 0; i < spawnMaxCount; i++)
        {
            createdItemCount++;
            var createdOjb = PoolSystem.instance.SpawnFromPool(poolName, null);
            createdOjb.transform.position = spawnTransform.position + new Vector3(Random.Range(-.25f,.75f) ,0 , Random.Range(-.25f, .75f));
            _inGameCollectablesCount[poolName] = createdItemCount;
            yield return _waitTime;
        } 
        _isCurrentlySpwaning = false;


    }


    private void CheckShouldSpawnCollectable()
    {
        if(_isCurrentlySpwaning) return;
        
        foreach (var item in _inGameCollectablesCount)
        {
            if (item.Key == "duck")
            {
                if(item.Value <= 15)
                    SpawnCollectableObject("duck");
                
                Debug.Log("var geliyor duck -> " + item.Value);
            }

            if (item.Key=="pinkDuck")
            {
                if(item.Value <= 15)
                    SpawnCollectableObject("pinkDuck");
                
                Debug.Log("var geliyor pink duck -> " + item.Value);
            }
        }
    }


   
}
