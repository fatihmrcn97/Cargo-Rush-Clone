using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CargoPlace : MonoBehaviour
{

    public List<GameObject> cargoItems;

    [SerializeField] private GameObject currier;

    public Transform CurrierTransform => currier.transform;
    
    private bool _isCurrierGoing =false;
    public bool IsCurrierGoing => _isCurrierGoing;

    private MoneyManager _moneyManager;

    [SerializeField] private GameObject remaningObj;

    public TextMeshPro remaningText;

    public int maxConvertedMaterial=8;
    private void Awake()
    {
        _moneyManager = GetComponentInChildren<MoneyManager>();
    }

    public void CargoFullCurrierGo()
    {
        StartCoroutine(CurrierGo());
    }

    private IEnumerator CurrierGo()
    {
        if(_isCurrierGoing) yield break;
        _isCurrierGoing = true;
        _moneyManager.MoneyCreate(10);
        remaningObj.SetActive(false);
        yield return new WaitForSeconds(.5f);
        currier.transform.DOMove(currier.transform.position + new Vector3(-10, 0, 0), 5.5f).OnComplete(() =>
        {
            cargoItems.ForEach(Destroy);
            cargoItems.Clear();
            remaningText.text = "0/" + maxConvertedMaterial;
            currier.transform.DOMove(currier.transform.position + new Vector3(10, 0, 0), 2.5f).OnComplete(() =>
            {
                remaningObj.SetActive(true);
                _isCurrierGoing = false;
            });
        });
    }
    
}
