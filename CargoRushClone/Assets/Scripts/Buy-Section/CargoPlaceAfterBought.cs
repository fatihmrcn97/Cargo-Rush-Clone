using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Events;

public class CargoPlaceAfterBought : MonoBehaviour , IBuyTrigger
{
    [SerializeField] private GameObject kargolamaParentObj;
    
    [Header("CargoSystems")] 
    [SerializeField] private GameObject currentCargoSystem;
    [Space] 
    [SerializeField] private GameObject nextObjectToBuy;
    [SerializeField] private GameObject nextVehicle;
    
    
    [SerializeField] UnityEvent u_event;

    [SerializeField] private bool isStart;
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
        StartCoroutine(PlaceAlreadyBought());
    }

    private IEnumerator PlaceAlreadyBought()
    {
        kargolamaParentObj.SetActive(true);  
        u_event?.Invoke();
        yield return null;
        VibrationOld.Vibrate(50); 
        OpenNextObjectToBuy();
        transform.gameObject.SetActive(false);
    }

    private IEnumerator PlaceActivateWithScale()
    {
        kargolamaParentObj.SetActive(true);
        currentCargoSystem.SetActive(false);
        
        transform.GetChild(0).GetComponent<Canvas>().enabled = false;
        transform.GetChild(1).GetComponent<Canvas>().enabled = false;
        // currentCargoSystem.GetComponentInChildren<SplineFollower>().follow = true;
        // currentCargoSystem.GetComponentInChildren<SplineFollower>().Restart();
        
        yield return new WaitForSeconds(2.5f);
        
        var nextVehicObj = Instantiate(nextVehicle);
        nextVehicObj.transform.position = nextVehicle.transform.position;
        var position = nextVehicObj.transform.position;
        position += new Vector3(-2, 0, 0);
        nextVehicObj.transform.position = position;
        nextVehicObj.transform.DOMove(position + new Vector3(2, 0, 0), .75f);
        nextVehicObj.transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(.75f);
        Destroy(nextVehicObj);
        currentCargoSystem.SetActive(true);
        currentCargoSystem.GetComponentInChildren<SplineFollower>().follow = false; 
        u_event?.Invoke();
        VibrationOld.Vibrate(50);
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
