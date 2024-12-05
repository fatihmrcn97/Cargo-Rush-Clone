using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoosterSystem : SingletonMonoBehaviour<BoosterSystem>
{
    [SerializeField] private GameObject speedBooster,
        capasityBooster,
        doubleIncomeBooster,
        productionBooster,
        workerBooster,
        freeMoneyBooster;


    [SerializeField] private GameObject money2d;
    
    
    private Button speedBoosterButton,
        capasityBoosterButton,
        doubleIncomeBoosterButton,
        productionBoosterButton,
        workerBoosterButton,
        freeMoneyBoosterButton;


    private float speedBoosterAmount;
    public float SpeedBoosterAmount => speedBoosterAmount;

    private int capasityBoosterAmount;
    public int CapasityBoosterAmount => capasityBoosterAmount;

    private int doubleIncomeBoosterAmount;
    public int DoubleIncomeBoosterAmount => doubleIncomeBoosterAmount;
    
    private float productionBoosterAmount;
    public float ProductionBoosterAmount => productionBoosterAmount;

    protected override void Awake()
    {
        base.Awake();
        speedBoosterAmount = 0;
        capasityBoosterAmount = 0;
        doubleIncomeBoosterAmount = 1;
        productionBoosterAmount = 0;
        InitializeItems();
    }

    private void InitializeItems()
    {
        speedBoosterButton = speedBooster.GetComponentInChildren<Button>();
        capasityBoosterButton = capasityBooster.GetComponentInChildren<Button>();
        doubleIncomeBoosterButton = doubleIncomeBooster.GetComponentInChildren<Button>();
        productionBoosterButton = productionBooster.GetComponentInChildren<Button>();
        workerBoosterButton = workerBooster.GetComponentInChildren<Button>();
        freeMoneyBoosterButton = freeMoneyBooster.GetComponentInChildren<Button>();

        speedBoosterButton.onClick.AddListener(SpeedBoost);
        capasityBoosterButton.onClick.AddListener(CapasityBoost);
        doubleIncomeBoosterButton.onClick.AddListener(DoubleIncomeBoost);
        productionBoosterButton.onClick.AddListener(ProductionBoost);
        workerBoosterButton.onClick.AddListener(WorkerBoost);
        freeMoneyBoosterButton.onClick.AddListener(FreeMoneyBoost);
    }


    private void SpeedBoost()
    {
        // Karakteri hızlandır
        StartCoroutine(BoostSpeedFor3Min());
        speedBooster.SetActive(false);
    }

    private void CapasityBoost()
    {
        StartCoroutine(BoostCapasityFor3Min());
        capasityBooster.SetActive(false);
    }

    private void DoubleIncomeBoost()
    {
        StartCoroutine(BoostDoubleIncomeFor3Min());
        doubleIncomeBooster.SetActive(false);
    }

    private void ProductionBoost()
    {
        StartCoroutine(BoostProductionSpeedFor3Min());
        productionBooster.SetActive(false);
    }

    private void WorkerBoost()
    {
        Events.OnWorkerBoosterClaimed?.Invoke();
        workerBooster.SetActive(false);
    }

    private void FreeMoneyBoost()
    {
        StartCoroutine(BoostFreeMoney());
        freeMoneyBooster.SetActive(false);
    }

    private IEnumerator BoostFreeMoney()
    {
        int moneyEanerdAmount = 100; 
        
        for (int i = 0; i < 55; i++)
        {
            var money = Instantiate(money2d, UIManager.instance.transform);
            money.transform.DOLocalMove(
                money.transform.localPosition + new Vector3(Random.Range(-600, 600), Random.Range(-850, 850), 0),
                0.05f);
            money.transform.DOLocalMove(UIManager.instance.moneyTargetPos.localPosition, .45f)
                .OnComplete(() => Destroy(money));
            yield return null;
        }

        UIManager.instance.ScoreAdd(moneyEanerdAmount);
    }


    private IEnumerator BoostSpeedFor3Min()
    {
        speedBoosterAmount = 1.5f;
        yield return new WaitForSeconds(150f);
        speedBoosterAmount = 0;
    }

    private IEnumerator BoostCapasityFor3Min()
    {
        capasityBoosterAmount = 15;
        yield return new WaitForSeconds(150f);
        capasityBoosterAmount = 0;
    }

    private IEnumerator BoostDoubleIncomeFor3Min()
    {
        doubleIncomeBoosterAmount = 2;
        yield return new WaitForSeconds(150f);
        doubleIncomeBoosterAmount = 1;
    }
    private IEnumerator BoostProductionSpeedFor3Min()
    {
        productionBoosterAmount = .5f;
        yield return new WaitForSeconds(150f);
        productionBoosterAmount = 0;
    }

    public void OpenBoosterUI(BoosterTypes boosterType)
    {
        switch (boosterType)
        {
            case BoosterTypes.SpeedBooster:
                speedBooster.SetActive(true);
                break;
            case BoosterTypes.CapasityBooster:
                capasityBooster.SetActive(true);
                break;
            case BoosterTypes.DoubleIncomeBooster:
                doubleIncomeBooster.SetActive(true);
                break;
            case BoosterTypes.ProductionBooster:
                productionBooster.SetActive(true);
                break;
            case BoosterTypes.WorkerBooster:
                workerBooster.SetActive(true);
                break;
            case BoosterTypes.FreeMoneyBooster:
                freeMoneyBooster.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(boosterType), boosterType, null);
        }
    }
}