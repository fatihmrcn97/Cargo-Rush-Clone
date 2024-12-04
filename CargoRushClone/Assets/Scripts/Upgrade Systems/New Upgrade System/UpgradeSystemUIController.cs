using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystemUIController : MonoBehaviour
{

    [SerializeField] private List<GameObject> sekmeler;
    [SerializeField] private List<GameObject> zeminler;


    private List<Vector3> sekmelerBasePosition;

    private void Awake()
    {
        GetAllBasePositions();
        for (int i = 0; i < sekmeler.Count; i++)
        {
            var skinButton = sekmeler[i].GetComponentInChildren<Button>();
            int index = i; // Coppy the index otherwise index always be sekmeler.Count
            skinButton.onClick.AddListener(() => OpenSekme(index));
        }
    }

    private void OnEnable()
    {
        OpenSekme(0);
    }

    private void GetAllBasePositions()
    {
        sekmelerBasePosition = new List<Vector3>();
        foreach (var item in sekmeler)
        {
            sekmelerBasePosition.Add(item.transform.position);
        }
    }

    private void CloseAllZemin()
    {
        zeminler.ForEach(x => x.SetActive(false));
    }

    private void CloseAllOpenSekmeler()
    {
        for (int i = 0; i < sekmeler.Count; i++)
        { 
            if(sekmeler[i].transform.position != sekmelerBasePosition[i])
                sekmeler[i].transform.DOMove(sekmelerBasePosition[i], .4f);
        }
    }

    private void OpenSekme(int index)
    {
        CloseAllOpenSekmeler();
        CloseAllZemin();
        zeminler[index].SetActive(true);
        sekmeler[index].transform.DOMove(sekmelerBasePosition[index]+(Vector3.up*40), .4f);
    }
}
