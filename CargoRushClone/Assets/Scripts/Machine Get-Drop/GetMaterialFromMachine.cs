using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GetMaterialFromMachine : MonoBehaviour
{
    public List<GameObject> singleMaterial;

    private MachineController _machineController;

    private const float ProgressTime = .9f;

    [SerializeField] private int indexOfStackSytem;

    [SerializeField] private string saveIndex;

    private GetMaterialSave _getMaterialSave;

    private BoxTapingMachine _boxTapingMachine;

    private IEnumerator Start()
    {
        if ((int.Parse(saveIndex) > 2))
            _boxTapingMachine = GetComponentInParent<BoxTapingMachine>();

        _machineController = GetComponentInParent<MachineController>();
        _getMaterialSave = new GetMaterialSave(_boxTapingMachine, _machineController, singleMaterial, saveIndex);
        _machineController.getMaterialSave = _getMaterialSave;
        yield return new WaitForSeconds(.5f);
        _getMaterialSave.LoadData();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            var iItemList = other.GetComponent<IItemList>();
            iItemList.IsInTrigger = true;
            PlayerGettingStackMaterials(iItemList).Forget();
        }

        if (other.CompareTag(TagManager.AI_TAG))
        {
            if (other.GetComponent<IAIWorker>().GetMachineController() == this)
            {
                var iItemList = other.GetComponent<IItemList>();
                iItemList.IsInTrigger = true;
                PlayerGettingStackMaterials(iItemList).Forget();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG) || other.CompareTag(TagManager.AI_TAG))
            other.GetComponent<IItemList>().IsInTrigger = false;
    }

    private async UniTaskVoid PlayerGettingStackMaterials(IItemList stackController)
    {
        var speedDelay = (int)(1000 * stackController.StackGetGiveDelaySpeed());
        await UniTask.Delay(speedDelay);

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

            _getMaterialSave.RemoveData();
            currentSingleMaterial.transform.GetChild(0).gameObject.SetActive(true);
            _machineController._stackSystems[indexOfStackSytem].SetTheStackPositonBack(singleMaterial.Count);

            var stackTransform = stackController.StackTransforms()[stackController.StackedMaterialList().Count];
            if (currentSingleMaterial.GetComponent<IItem>().CurrentTween() != null)
            {
                currentSingleMaterial.GetComponent<IItem>().CurrentTween().Kill();
            }


            currentSingleMaterial.transform.SetParent(stackTransform);
            currentSingleMaterial.transform.DOLocalJump(Vector3.zero, .5f, 1, ProgressTime);
            currentSingleMaterial.transform.DOLocalRotate(new Vector3(0, Random.Range(-90, 90), 0), ProgressTime);

            stackController.StackedMaterialList().Add(currentSingleMaterial);
            Events.MaterialStackedEvent?.Invoke();
            await UniTask.Delay(speedDelay);
        }
    }
}