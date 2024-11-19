using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCollectPoint : MonoBehaviour
{
    public List<GameObject> singleMaterial;

    private bool isInTrigger;

    private ItemProducer truckController;

    [SerializeField] private float timeToStack;

    private void Start()
    {
        truckController = GetComponent<ItemProducer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            isInTrigger = true;
            StartCoroutine(PlayerGettingStackMaterials(other.GetComponent<PlayerStackController>()));
        }
    }
 
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            isInTrigger = false;
        }

    }
    private IEnumerator PlayerGettingStackMaterials(PlayerStackController stackController)
    {
        if (singleMaterial.Count <= 0 || !isInTrigger || stackController.stackedMaterials.Count >= stackController.maxStackCount) yield break;
        GameObject currentSingleMaterial = singleMaterial[^1];
        singleMaterial.Remove(currentSingleMaterial);
        truckController._stackSystem.SetTheStackPositonBack(singleMaterial.Count);
        currentSingleMaterial.transform.SetParent(stackController.stackTransform[stackController.stackedMaterials.Count]); 
        stackController.stackedMaterials.Add(currentSingleMaterial);
        Events.MaterialStackedEvent?.Invoke();
        currentSingleMaterial.transform.DOLocalRotate(Vector3.zero, .3f);
        currentSingleMaterial.transform.DOLocalJump(Vector3.zero, .5f, 1, .2f);
        yield return new WaitForSeconds(timeToStack);

        StartCoroutine(PlayerGettingStackMaterials(stackController));
    }


}
