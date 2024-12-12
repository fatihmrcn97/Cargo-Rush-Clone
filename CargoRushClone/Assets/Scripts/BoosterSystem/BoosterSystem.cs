using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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


    [SerializeField] private List<TextMeshProUGUI> boosterTimeTxt;
    

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
        StartCoroutine(BoostSpeedFor3Min(180));
        StartCoroutine(TimerForWaitingCompleteOrder(boosterTimeTxt[0], 180));
        speedBooster.SetActive(false);
    }

    private void CapasityBoost()
    {
        StartCoroutine(BoostCapasityFor3Min(150));
        StartCoroutine(TimerForWaitingCompleteOrder(boosterTimeTxt[1], 150));
        capasityBooster.SetActive(false);
    }

    private void DoubleIncomeBoost()
    {
        StartCoroutine(BoostDoubleIncomeFor3Min(150));
        StartCoroutine(TimerForWaitingCompleteOrder(boosterTimeTxt[2], 150));
        doubleIncomeBooster.SetActive(false);
    }

    private void ProductionBoost()
    {
        StartCoroutine(BoostProductionSpeedFor3Min(150));
        StartCoroutine(TimerForWaitingCompleteOrder(boosterTimeTxt[3], 150));
        productionBooster.SetActive(false);
    }

    private void WorkerBoost()
    {
        StartCoroutine(TimerForWaitingCompleteOrder(boosterTimeTxt[4], 150));
        Events.OnWorkerBoosterClaimed?.Invoke(150);
        workerBooster.SetActive(false);
    }

    private void FreeMoneyBoost()
    {
        StartCoroutine(BoostFreeMoney());
        freeMoneyBooster.SetActive(false);
    }

    private IEnumerator BoostFreeMoney()
    {
        int moneyEanerdAmount = (300*UIManager.instance.activeBantMachines)+(200*UIManager.instance.activeCargo);
        var listOfMoney = new List<GameObject>();
        for (int i = 0; i < 55; i++)
        {
            var money = Instantiate(money2d, UIManager.instance.transform);
            money.transform.DOLocalMove(
                money.transform.localPosition + new Vector3(Random.Range(-500, 500), Random.Range(-650, 650), 0),
                0.05f);
           listOfMoney.Add(money);
        }

        foreach (var money in listOfMoney)
        {
            money.transform.DOLocalMove(UIManager.instance.moneyTargetPos.localPosition, .45f)
                .OnComplete(() => Destroy(money));
            yield return null;
        }
        listOfMoney.Clear();
        UIManager.instance.ScoreAdd(moneyEanerdAmount);
    }


    private IEnumerator BoostSpeedFor3Min(float time)
    {
        speedBoosterAmount = 1.5f;
        yield return new WaitForSeconds(time);
        speedBoosterAmount = 0;
        Events.OnBoosterFinished?.Invoke(BoosterTypes.SpeedBooster);
    }

    private IEnumerator BoostCapasityFor3Min(float time)
    {
        capasityBoosterAmount = 15;
        yield return new WaitForSeconds(time);
        capasityBoosterAmount = 0;
        Events.OnBoosterFinished?.Invoke(BoosterTypes.CapasityBooster);
    }

    private IEnumerator BoostDoubleIncomeFor3Min(float time)
    {
        doubleIncomeBoosterAmount = 2;
        yield return new WaitForSeconds(time);
        doubleIncomeBoosterAmount = 1;
        Events.OnBoosterFinished?.Invoke(BoosterTypes.DoubleIncomeBooster);
    }
    private IEnumerator BoostProductionSpeedFor3Min(float time)
    {
        productionBoosterAmount = .5f;
        yield return new WaitForSeconds(time);
        productionBoosterAmount = 0;
        Events.OnBoosterFinished?.Invoke(BoosterTypes.ProductionBooster);
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
    
    
    private IEnumerator TimerForWaitingCompleteOrder(TextMeshProUGUI timeText,float timeForCompleteOrder)
    {
        timeText.transform.parent.gameObject.SetActive(true);
        float timer = timeForCompleteOrder;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            string niceTime = $"{minutes:0}:{seconds:00}";
            timeText.text = niceTime;
            yield return null;
        } 
        timeText.text = "0:00";
        timeText.transform.parent.gameObject.SetActive(false);  
    }
}