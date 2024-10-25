using DG.Tweening;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GetMaterialFromMachine : MonoBehaviour
{
    public List<GameObject> singleMaterial;
 

    private MachineController _machineController;

    private const float ProgressTime = .2f;

    [SerializeField] private int indexOfStackSytem;


    private void Start()
    {
        _machineController = GetComponentInParent<MachineController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(TagManager.PLAYER_TAG)) return;
        var iItemList = other.GetComponent<IItemList>();
        iItemList.IsInTrigger = true;
        PlayerGettingStackMaterials(iItemList).Forget();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
            other.GetComponent<IItemList>().IsInTrigger=false;
    }

    private async UniTaskVoid PlayerGettingStackMaterials(IItemList stackController)
    {
        while (stackController.IsInTrigger)
        {
            if (singleMaterial.Count <= 0)
            {
                await UniTask.Delay(150);
                continue;
            }

            if (stackController.CheckPlayerHandMax()) return;

            var currentSingleMaterial = singleMaterial[^1];
            singleMaterial.Remove(currentSingleMaterial);

            _machineController._stackSystems[indexOfStackSytem].SetTheStackPositonBack(singleMaterial.Count);

            var stackTransform = stackController.StackTransforms()[stackController.StackedMaterialList().Count];
            if (currentSingleMaterial.GetComponent<IItem>().CurrentTween() != null)
            {
                currentSingleMaterial.GetComponent<IItem>().CurrentTween().Kill();
            }

            currentSingleMaterial.transform.SetParent(stackTransform);
            currentSingleMaterial.transform.DOLocalJump(Vector3.zero, .5f, 1, ProgressTime);
            currentSingleMaterial.transform.DOLocalRotate(Vector3.zero, ProgressTime);

            stackController.StackedMaterialList().Add(currentSingleMaterial);
            Events.MaterialStackedEvent?.Invoke();
            await UniTask.Delay(50);
        }
    }
}