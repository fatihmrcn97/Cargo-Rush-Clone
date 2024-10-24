 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;

    [SerializeField] private GameObject moneyPrefab;

    private List<GameObject> moneyList = new();

    [SerializeField] StackSO stackData;

    private Vector3 origPoisitonOfSpawnPoint;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        origPoisitonOfSpawnPoint = spawnPosition.position;
    }
 
    public void MoneyCreate(int moneyCount)
    {
        StartCoroutine(MoneyEarned(spawnPosition, moneyCount)); 
    } 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (moneyList.Count <= 0) return;
            StartCoroutine(MoneyCollecting(other.gameObject)); 
        }
    }

    private IEnumerator MoneyEarned(Transform spawnPoint, int moneyAmount)
    {
        for (var i = 0; i < moneyAmount; i++)
        {
            var item = Instantiate(moneyPrefab, spawnPoint.position, Quaternion.Euler(0, 90, 0));
            item.transform.localScale = Vector3.zero;
            item.transform.DOScale(Vector3.one, .1f);
            moneyList.Add(item);
            DropPointHandle();
            item.transform.parent = transform;
            yield return new WaitForSeconds(.05f);
        }
         
    }
     
    private int line = 0, column = 0;

    private void DropPointHandle()
    {
        if (line < 3)
        {
            // ilk yerle�meden sonra bu kod �al���yor 
            line++;
            var localPosition = spawnPosition.transform.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z + stackData.Z);
            spawnPosition.transform.localPosition = localPosition;
        }
        else
        {
            if (column < 2)
            {
                column++;
                var localPosition = spawnPosition.transform.localPosition;
                localPosition = new Vector3(localPosition.x + stackData.X, localPosition.y, localPosition.z - (line * stackData.Z));
                line = 0;
                spawnPosition.transform.localPosition = localPosition;
            }
            else
            {
                var localPosition = spawnPosition.transform.localPosition;
                localPosition = new Vector3(localPosition.x - ((column) * stackData.X), localPosition.y + stackData.Y, localPosition.z - (line * stackData.Z));
                line = 0;
                column = 0;
                spawnPosition.transform.localPosition = localPosition;
            }
        }
    }

    private IEnumerator MoneyCollecting(GameObject player)
    {
        List<GameObject> tempMoneyList = new(moneyList);
        line = 0;
        column = 0;
        spawnPosition.position = origPoisitonOfSpawnPoint;
        foreach (var item in Enumerable.Reverse(tempMoneyList))
        {
            item.GetComponent<MoneyMovement>().MoneyCollectedMovement(player);
            Vibration.Vibrate(10);
            moneyList.Remove(item); 
            yield return null;
        }
         
    }
 

}