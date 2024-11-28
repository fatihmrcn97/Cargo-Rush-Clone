using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Events;

public class PlaceAfterUpgBantMachine : MonoBehaviour, IBuyTrigger
{
    [SerializeField] private GameObject nextObjectToBuy;
    [SerializeField] private bool isStart;
 
    [SerializeField] UnityEvent u_event;
    private void Awake()
    {
        if (isStart) return;
        transform.GetComponent<BoxCollider>().enabled = false;
        transform.transform.GetChild(0).gameObject.SetActive(false);
        transform.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void PlaceBought()
    {
        StartCoroutine(PlaceActivateWithScale());
    }

    public void AlreadyBought()
    {
        StartCoroutine(PlaceActivateWithScale());
    }

    private IEnumerator PlaceActivateWithScale()
    {
        transform.GetChild(0).GetComponent<Canvas>().enabled = false;
        transform.GetChild(1).GetComponent<Canvas>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        u_event?.Invoke();
        Vibration.Vibrate(50);
        yield return null;
        OpenNextObjectToBuy();
        transform.gameObject.SetActive(false);
    }

    private void OpenNextObjectToBuy()
    {
        if (nextObjectToBuy == null) return;
        nextObjectToBuy.GetComponent<BoxCollider>().enabled = true;
        nextObjectToBuy.transform.GetChild(0).gameObject.SetActive(true);
        nextObjectToBuy.transform.GetChild(1).gameObject.SetActive(true);
    }
}
