using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CargoPlace : MonoBehaviour
{

    [SerializeField] private GameObject currier;

    [SerializeField] private SplineFollower splineFollower;

    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] private GameObject remaningObj;

    public List<GameObject> cargoItems;
    public Transform CurrierTransform => currier.transform;
    public bool IsCurrierGoing => _isCurrierGoing;
    public TextMeshPro remaningText; 
    public int maxConvertedMaterial = 8;
    
    private bool _isCurrierGoing = false;
    private MoneyManager _moneyManager;
    private AddMaterialToCargo _addMaterialToCargo;

    [SerializeField] private UnityEvent unityEvent;
    
    
    private void Awake()
    {
        _moneyManager = GetComponentInChildren<MoneyManager>();
        _addMaterialToCargo = GetComponentInChildren<AddMaterialToCargo>();
    }
    

    public void CargoFullCurrierGo()
    {
        StartCoroutine(CurrierGo());
    }
 

    private IEnumerator CurrierGo()
    {
        if (_isCurrierGoing) yield break;
        unityEvent?.Invoke();
        splineFollower.follow = true;
        splineFollower.followDuration = 4;
        splineFollower.Restart();
        _isCurrierGoing = true;
        _moneyManager.MoneyCreate(10);
        remaningObj.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        _addMaterialToCargo.TapedItemStatus = SetRandomShippingObject();
        cargoItems.ForEach(Destroy);
        cargoItems.Clear();
        remaningText.text = "0/" + maxConvertedMaterial;
    }

    public void OnPathFinished()
    {
        remaningObj.SetActive(true);
        _isCurrierGoing = false;
    }

    private TapedItemStatus SetRandomShippingObject()
    {
        Debug.Log(UIManager.instance.InGameCollectablesCount.Keys.Count);
        var randomTapedBox =Helper.GetRandomTaped(UIManager.instance.InGameCollectablesCount.Keys.Count);
        maxConvertedMaterial = Random.Range(5, 9);
        spriteRenderer.sprite = Helper.GetRandomSpriteAccordiongToTapedBox(randomTapedBox);
        unityEvent?.Invoke();
        return randomTapedBox;
    }

  
}