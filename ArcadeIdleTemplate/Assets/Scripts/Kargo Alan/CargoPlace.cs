using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CargoPlace : MonoBehaviour
{

    public List<GameObject> cargoItems;

    [SerializeField] private GameObject currier;

    public Transform CurrierTransform => currier.transform;
    
    private bool _isCurrierGoing =false;
    public bool IsCurrierGoing => _isCurrierGoing;

    private MoneyManager _moneyManager;

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
        yield return new WaitForSeconds(.5f);
        currier.transform.DOMove(currier.transform.position + new Vector3(-10, 0, 0), 5.5f).OnComplete(() =>
        {
            cargoItems.ForEach(Destroy);
            cargoItems.Clear();
            currier.transform.DOMove(currier.transform.position + new Vector3(10, 0, 0), 2.5f).OnComplete(() =>
            {
                
                _isCurrierGoing = false;
            });
        });
    }
    
}
