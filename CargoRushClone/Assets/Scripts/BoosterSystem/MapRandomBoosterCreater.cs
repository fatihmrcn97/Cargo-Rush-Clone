using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapRandomBoosterCreater : MonoBehaviour
{
    [SerializeField]
    private GameObject[] boosters; // 0-> Speed , 1-> Capasity ,2-> doubleincome,3-> production ,4-> worker,5->freemoney

    [SerializeField]
    private float[] boosterTimes; // 0-> Speed , 1-> Capasity ,2-> doubleincome,3-> production ,4-> worker,5->freemoney

    //[SerializeField] private List<BoosterSpwanPoint> boosterSpwanPoints;
    [SerializeField] private List<Transform> boosterSpwanPoints;

    [SerializeField] private List<Transform> boosterYedekSpawnPoints;

    private void Awake()
    {
        InvokeRepeating(nameof(CreateSpeedBooster), 1f, boosterTimes[0]);
        InvokeRepeating(nameof(CreateCapsityBooster), 1f, boosterTimes[1]);
        InvokeRepeating(nameof(CreateDoubleBooster), 1f, boosterTimes[2]);
        InvokeRepeating(nameof(CreateProductionBooster), 1f, boosterTimes[3]);
        InvokeRepeating(nameof(CreateWorkerBooster), 1f, boosterTimes[4]);
        InvokeRepeating(nameof(CreateFreeMoneyBooster), 1f, boosterTimes[5]);
    }


    public void ChangeSpawnPoint(int changeIndex)
    {
        boosterSpwanPoints[changeIndex] = boosterYedekSpawnPoints[changeIndex];
    }
     
    
    #region WRAPPER OF CRATEBOOSTERS
    private void CreateSpeedBooster()
    {
        CreateBooster(BoosterTypes.SpeedBooster);
    }
    private void CreateCapsityBooster()
    {
        CreateBooster(BoosterTypes.CapasityBooster);
    }
    private void CreateDoubleBooster()
    {
        CreateBooster(BoosterTypes.DoubleIncomeBooster);
    }
    private void CreateProductionBooster()
    {
        CreateBooster(BoosterTypes.ProductionBooster);
    }
    private void CreateWorkerBooster()
    {
        CreateBooster(BoosterTypes.WorkerBooster);
    }
    private void CreateFreeMoneyBooster()
    {
        CreateBooster(BoosterTypes.FreeMoneyBooster);
    }
    #endregion
    private void CreateBooster(BoosterTypes boosterType)
    {
        var booster = Instantiate(boosters[(int)boosterType]);
        booster.transform.position = boosterSpwanPoints[(int)boosterType].position;
    }
    
    // private void CreateBoosters()
    // {
    //     if(GetRandomNonUsedBoosterSpawnPoint()==null) return;
    //     
    //     var booster = Instantiate(boosters[Random.Range(0, boosters.Length)]);
    //     booster.transform.position = GetRandomNonUsedBoosterSpawnPoint().position;
    // }
    //
    // private Transform GetRandomNonUsedBoosterSpawnPoint()
    // {
    //     var a = boosterSpwanPoints.All(val => val.isAvaliable == false);
    //     if (a) return null;
    //
    //     var random = boosterSpwanPoints[Random.Range(0, boosterSpwanPoints.Count)];
    //     if (random.isAvaliable)
    //     {
    //         random.isAvaliable = false;
    //         return random.spawnPoint;
    //     }
    //
    //     return GetRandomNonUsedBoosterSpawnPoint();
    // }
}

[Serializable]
public class BoosterSpwanPoint
{
    public Transform spawnPoint;
    [HideInInspector] public bool isAvaliable = true;
}