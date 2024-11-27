using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CargoPlace : MonoBehaviour
{
    public TextMeshPro remaningText;
    [SerializeField] private GameObject currier;
    [SerializeField] private SplineFollower splineFollower;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject remaningObj;

    [HideInInspector] public List<GameObject> cargoItems;
    
    private int _maxConvertedMaterial;
    public int MaxConvertedMaterial=> _maxConvertedMaterial;
    public bool IsCurrierGoing => _isCurrierGoing;

    private bool _isCurrierGoing = false;
    private MoneyManager _moneyManager;
    private AddMaterialToCargo _addMaterialToCargo;

    [SerializeField] private UnityEvent unityEvent;
    
    [SerializeField] private Slider timerSlider;
    [Range(4, 10)] [SerializeField] private float totalDuration;

    [SerializeField] private Vector2 minMaxCargoPerVehicle;


    private void Awake()
    {
        _maxConvertedMaterial = Random.Range((int)minMaxCargoPerVehicle.x, (int)minMaxCargoPerVehicle.y);
        _moneyManager = GetComponentInChildren<MoneyManager>();
        _addMaterialToCargo = GetComponentInChildren<AddMaterialToCargo>();
        timerSlider.maxValue = totalDuration;
        timerSlider.value = 0;
        timerSlider.gameObject.SetActive(false);
    }


    public void CargoFullCurrierGo()
    {
        StartCoroutine(CurrierGo());
    }


    private IEnumerator CurrierGo()
    {
        if (_isCurrierGoing) yield break;
        StartCoroutine(SliderTimer());
        unityEvent?.Invoke();
        splineFollower.follow = true;
        splineFollower.followDuration = totalDuration;
        splineFollower.Restart();
        _isCurrierGoing = true;
        _moneyManager.MoneyCreate(cargoItems.Count);
        remaningObj.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        _addMaterialToCargo.TapedItemStatus = SetRandomShippingObject();
        cargoItems.ForEach(Destroy);
        cargoItems.Clear();
        remaningText.text = "0/" + _maxConvertedMaterial;
    }

    public void OnUpgrade()
    {
        _moneyManager.MoneyCreate(cargoItems.Count);
        cargoItems.ForEach(Destroy);
        cargoItems.Clear();
    }

    public void OnPathFinished()
    {
        remaningObj.SetActive(true);
        _isCurrierGoing = false;
        timerSlider.gameObject.SetActive(false);
    }

    private TapedItemStatus SetRandomShippingObject()
    {
        var randomTapedBox = Helper.GetRandomTaped(UIManager.instance.InGameCollectablesCount.Keys.Count);
        _maxConvertedMaterial = Random.Range((int)minMaxCargoPerVehicle.x, (int)minMaxCargoPerVehicle.y);
        spriteRenderer.sprite = Helper.GetRandomSpriteAccordiongToTapedBox(randomTapedBox);
        unityEvent?.Invoke();
        return randomTapedBox;
    }

    public void CargoSystemUpgraded(SplineFollower newSplineFollower)
    {
        splineFollower = newSplineFollower;
    }

    private IEnumerator SliderTimer()
    {
        timerSlider.gameObject.SetActive(true);
        float timer = 0;
        while (timer < totalDuration)
        {
            timer += Time.deltaTime;
            timerSlider.value = timer;
            yield return null;
        }
    }
}