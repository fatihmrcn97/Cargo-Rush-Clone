using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using TMPro;
using UnityEngine;

public class BoxTapingMachine : MachineController, ITriggerInteraction
{
    private GameObject _convertingItem;

    [SerializeField] private float jumpPower = .25f;

    // [SerializeField] private ParticleSystem ps; 
    [SerializeField] private SplineComputer splineComputer;

    [SerializeField] private ItemStatus outItemStatus;
    [SerializeField] private TapedItemStatus outTapedItemStatus;

    [SerializeField] private TextMeshPro remainingTxt;

    [SerializeField] private GameObject paletObj;
    private List<GameObject> _addedPalets;


    private void Awake()
    {
        _stackSystem = GetComponent<IStackSystem>();
        _stackSystems = new List<IStackSystem> { _stackSystem };
        _addedPalets = new List<GameObject>();


        splineComputer = GetComponentInChildren<SplineComputer>();
        anim = GetComponentInChildren<Animator>();
        _addMaterialToMachine = GetComponentInChildren<AddMaterialToMachine>();
        _getMaterialFromMachine = GetComponentInChildren<GetMaterialFromMachine>();
        InvokeRepeating(nameof(MachineStartedWorking), 1f, 3f);

        remainingTxt.text = "0/" + _addMaterialToMachine._maxConvertedMaterial;
        // animStartValue = anim.GetFloat(TagManager.ANIM_SPEED_FLOAT); 
    }

    private void MachineStartedWorking()
    {
        if (convertedMaterials.Count <= 0) return;

        //      ps.Play();
        // anim.SetBool(TagManager.WALKING_BOOL_ANIM, true);
        _convertingItem = convertedMaterials[^1];
        convertedMaterials.Remove(_convertingItem);
        remainingTxt.text = convertedMaterials.Count + "/" + _addMaterialToMachine._maxConvertedMaterial;
        _addMaterialToMachine.stackSystem.SetTheStackPositonBack(convertedMaterials.Count);
        _convertingItem.transform.DOLocalJump(material_machine_enter_pos.position, jumpPower, 1, .15f)
            .OnComplete(ItemBoxingProcess);
    }

    private void ItemBoxingProcess()
    {
        _convertingItem.GetComponent<SplineFollower>().spline = splineComputer;
        _convertingItem.GetComponent<SplineFollower>().enabled = true;
    }


    public void OnTriggerBanting(GameObject item)
    {
        StartCoroutine(ReduceSplineSpeedForSec(item.GetComponent<SplineFollower>()));
        // var packBox = Instantiate(newProduct, lastBoxPosition.position, Quaternion.Euler(0, 90, 0), null);
    }

    private IEnumerator ReduceSplineSpeedForSec(SplineFollower speed)
    {
        speed.followSpeed = .55f;
        yield return new WaitForSeconds(2f);
        speed.followSpeed = 1;
    }

    public void PathEnded(GameObject item)
    {
        item.GetComponent<SplineFollower>().enabled = false;
        item.transform.DORotate(_stackSystem.MaterialDropPositon().rotation.eulerAngles, .15f);
        item.GetComponent<IItem>().SetStatus(outItemStatus, outTapedItemStatus);
        item.tag = TagManager.Default;
     
        CheckExtraPaletAddOrDelete(true);
        item.GetComponent<IItem>()
            .SetCurrentTween(
                item.transform.DOJump(_stackSystem.MaterialDropPositon().position, 1.15f, 1, .25f));
        
        _getMaterialFromMachine.singleMaterial.Add(item);
        _stackSystem.DropPointHandle();
        
    }

    public void CheckExtraPaletAddOrDelete(bool isAdd)
    {
        if (isAdd)
        {
            if (_getMaterialFromMachine.singleMaterial.Count <= 0) return;
            // Paleti ekle
            int paletCount = _getMaterialFromMachine.singleMaterial.Count;
            if (paletCount % 16 == 0)
            {
                int heightMultiplier = paletCount / 16; //.65f Ekelenecek her bir palet için
                var addedPalet = Instantiate(paletObj);
                addedPalet.transform.SetPositionAndRotation(
                    paletObj.transform.position + new Vector3(0, 0.48f * heightMultiplier, 0),
                    paletObj.transform.rotation);
                _addedPalets.Add(addedPalet);
            }
        }
        else
        {
            // Paleti sil
            if (_getMaterialFromMachine.singleMaterial.Count < 16)
            {
                //tüm paletleri sil
                _addedPalets.ForEach(Destroy);
                _addedPalets.Clear();
                return;
            }

            if (_getMaterialFromMachine.singleMaterial.Count % 16 == 0)
            {
                var lastPalet = _addedPalets[^1];
                _addedPalets.Remove(lastPalet);
                Destroy(lastPalet);
            }
        }
    }
}