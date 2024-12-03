using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VipCargoPlace : MonoBehaviour
{
    public TextMeshPro remaningText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject remaningObj;
    [HideInInspector] public List<GameObject> cargoItems;
    [SerializeField] private Transform vipCustomerTarget;
    [SerializeField] private Transform vipBaseTarget;
    [SerializeField] private GameObject vipDrone;

    private MoneyManager _moneyManager;
    private bool _isVipActive = false;
    private int _maxConvertedMaterial;
    public int MaxConvertedMaterial => _maxConvertedMaterial;
    public bool IsCurrierGoing => _isCurrierGoing;

    private bool _isCurrierGoing = false;
    private AddMaterialToVip _addMaterialToCargo;


    [Header("UI Elements")] [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField] private Slider vipComplatedRewardWaitSlider;
    [SerializeField] private GameObject vipComplatedCanvas;
    [SerializeField] private Button claimButton;


    [Header("Settings")] [Range(4, 10)] [SerializeField]
    private float totalDuration;

    [SerializeField] private Vector2 minMaxCargoPerVehicle;
    [SerializeField] private int arriveTimeSeconds;
    [SerializeField] private float timeForCompleteOrder;
    [SerializeField] private float timeForWaitAfterCompleteOrder;
    [SerializeField] private int moneyEarnMuliplier, rewarMoneyMuliplier;


    private Coroutine timerCoroutine;

    private void Awake()
    {
        _moneyManager = GetComponentInChildren<MoneyManager>();
        _maxConvertedMaterial = Random.Range((int)minMaxCargoPerVehicle.x, (int)minMaxCargoPerVehicle.y);
        _addMaterialToCargo = GetComponentInChildren<AddMaterialToVip>();
        DeactiveteVipCustomer();
        vipComplatedRewardWaitSlider.maxValue = timeForWaitAfterCompleteOrder;
        claimButton.onClick.AddListener(ClaimReward);
        Invoke(nameof(ActivateVipCustomer), 1);
        remaningText.text = "0/" + _maxConvertedMaterial;

        // timerSlider.maxValue = totalDuration;
        // timerSlider.value = 0;
        // timerSlider.gameObject.SetActive(false);
    }

    private void ClaimReward()
    {
        vipComplatedCanvas.SetActive(false);
        StartCoroutine(CurrierGo());
    }

    private void DeactiveteVipCustomer()
    {
        _addMaterialToCargo.gameObject.SetActive(false);
        remaningObj.SetActive(false);
        _isVipActive = false;
        vipDrone.transform.DOMove(vipCustomerTarget.position, .1f);
    }

    private void ActivateVipCustomer()
    {
        if (_isVipActive) return;
        _isVipActive = true;
        vipDrone.transform.DOMove(vipBaseTarget.position, 3f).OnComplete(() =>
        {
            _addMaterialToCargo.gameObject.SetActive(true);
            remaningObj.SetActive(true);
            timerCoroutine = StartCoroutine(TimerForWaitingCompleteOrder());
        }).SetEase(Ease.OutQuad);
    }

    private IEnumerator TimerForWaitingCompleteOrder()
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
        DeactiveteVipCustomer();
        timeText.transform.DOScale(timeText.transform.localScale, arriveTimeSeconds)
            .OnComplete(ActivateVipCustomer); // This is just for wait arriveTimeSeconds and do Activate vip customer
    }

    private IEnumerator WaitAfterCompleteOrderForUIInteraction()
    {
        float timer = 0;

        while (timer < timeForWaitAfterCompleteOrder)
        {
            timer += Time.deltaTime;
            vipComplatedRewardWaitSlider.value = timer;
            yield return null;
        }

        vipComplatedCanvas.SetActive(false);
        StartCoroutine(CurrierGo());
        _moneyManager.MoneyCreate(moneyEarnMuliplier);
    }

    public void VipCargoFinishedOpenUI()
    {
        //StartCoroutine(CurrierGo());
        StopCoroutine(timerCoroutine);
        timeText.transform.parent.gameObject.SetActive(false);
        timeText.transform.DOScale(timeText.transform.localScale, arriveTimeSeconds)
            .OnComplete(ActivateVipCustomer); // This is just for wait arriveTimeSeconds and do Activate vip customer
        vipComplatedCanvas.SetActive(true);
        StartCoroutine(WaitAfterCompleteOrderForUIInteraction());
    }


    private IEnumerator CurrierGo()
    {
        if (_isCurrierGoing) yield break;
        _isCurrierGoing = true;
        _addMaterialToCargo.gameObject.SetActive(false);
        remaningObj.SetActive(false);
        _isVipActive = false;
        vipDrone.transform.DOMove(vipCustomerTarget.position, totalDuration);
        yield return new WaitForSeconds(totalDuration);
        _addMaterialToCargo.TapedItemStatus = SetRandomShippingObject();
        cargoItems.ForEach(Destroy);
        cargoItems.Clear();
        _isCurrierGoing = false;
        remaningText.text = "0/" + _maxConvertedMaterial;
    }

    private TapedItemStatus SetRandomShippingObject()
    {
        var randomTapedBox = Helper.GetRandomTaped(UIManager.instance.InGameCollectablesCount.Keys.Count);
        _maxConvertedMaterial = Random.Range((int)minMaxCargoPerVehicle.x, (int)minMaxCargoPerVehicle.y);
        spriteRenderer.sprite = Helper.GetRandomSpriteAccordiongToTapedBox(randomTapedBox);
        return randomTapedBox;
    }


    // private IEnumerator SliderTimer()
    // {
    //     timerSlider.gameObject.SetActive(true);
    //     float timer = 0;
    //     while (timer < totalDuration)
    //     {
    //         timer += Time.deltaTime;
    //         timerSlider.value = timer;
    //         yield return null;
    //     }
    // }
}