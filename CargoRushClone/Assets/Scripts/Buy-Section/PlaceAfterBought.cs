using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlaceAfterBought : MonoBehaviour, IBuyTrigger
{
    [SerializeField] private GameObject placeActivate;
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

    private IEnumerator PlaceActivateWithScale()
    { 
        transform.GetChild(0).GetComponent<Canvas>().enabled = false;
        transform.GetChild(1).GetComponent<Canvas>().enabled = false; 
        placeActivate.transform.localScale = new Vector3(0, 0, 0);
        placeActivate.SetActive(true);
        placeActivate.transform.DOScale(Vector3.one, .75f).SetEase(Ease.InBounce);
        yield return new WaitForSeconds(1f);

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
