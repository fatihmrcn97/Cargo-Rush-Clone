using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
 
public class AddMaterialToRecycle : MonoBehaviour
{
    [HideInInspector] public IStackSystem stackSystem;
  
    private bool isInTrigger;

    public int _maxConvertedMaterial = 50;

    private Coroutine DropMaterialCorotine;

    [SerializeField] private ItemStatus itemStatus;

    public List<GameObject> recycleMaterials;
    private void Start()
    {
        recycleMaterials = new List<GameObject>();
        stackSystem = GetComponent<IStackSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            isInTrigger = true;
            DropMaterialCorotine =
                StartCoroutine(PlayerDroppingMaterialsToTheMachine(other.GetComponent<PlayerStackController>()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            if (DropMaterialCorotine != null) StopCoroutine(DropMaterialCorotine);
            isInTrigger = false;
        }
    }

    private IEnumerator PlayerDroppingMaterialsToTheMachine(PlayerStackController stackController)
    {
        float progressionTime = .1f;
        List<GameObject> tempList = new(stackController.stackedMaterials);
        tempList.Reverse();
        if (stackController.stackedMaterials.Count <= 0) yield break;

        var iItem = stackController.stackedMaterials[0].GetComponent<IItem>();

        if (itemStatus != iItem.ItemStatus()) yield break;

        foreach (var currentSingleMaterial in tempList)
        {
            if (!isInTrigger) yield break;
            if (itemStatus != iItem.ItemStatus()) yield break;
            currentSingleMaterial.transform.SetParent(null);

            stackController.stackedMaterials.Remove(currentSingleMaterial);
            stackController.StackPositionHandler();
            recycleMaterials.Add(currentSingleMaterial);
            currentSingleMaterial.transform.DOLocalRotate(Vector3.zero, progressionTime);
            currentSingleMaterial.transform.DOLocalJump(stackSystem.MaterialDropPositon().position, .5f, 1,
                progressionTime);
            stackSystem.DropPointHandle();
            Events.MaterialStackedEvent?.Invoke();
            yield return new WaitForSeconds(progressionTime + .02f);
        }
    }
}