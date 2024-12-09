using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using TMPro;
using UnityEngine;

public class BoxTapingMachine : MachineController, ITriggerInteraction
{
    private GameObject _convertingItem;

    // [SerializeField] private ParticleSystem ps; 
    [SerializeField] private SplineComputer splineComputer;

    [SerializeField] private ItemStatus outItemStatus;
    [SerializeField] private TapedItemStatus outTapedItemStatus;

    [SerializeField] private TextMeshPro remainingTxt;
    [SerializeField] private TextMeshPro surplasBoxesUiText;
    [SerializeField] private GameObject surplasBoxesUIObj;


    [SerializeField] private GameObject paletObj;
    private List<GameObject> _addedPalets;

    [Header("Machine & Box Settings")] [Range(0.25f, 10f)] [SerializeField]
    private float machineWorkingSpeed;

    [Range(0.5f, 5f)] [SerializeField] private float boxSpeedOnBantMachine;
    [SerializeField] private float jumpPower = .25f;
    [SerializeField] private int maxTapedItemCount=50;
    

    [SerializeField] private List<MeshRenderer> meshRenderers;
    [SerializeField] private MeshRenderer corner;
    
    private bool _isMachineWorking = false;
    private List<bool> fakeBoxes;

    [SerializeField] private GameObject stoppedUI;
    
    private WaitForSeconds _waitTime;
    private float upgradeBoxSpeed=0;
    private void Awake()
    {
        _stackSystem = GetComponent<IStackSystem>();
        _stackSystems = new List<IStackSystem> { _stackSystem };
        _addedPalets = new List<GameObject>();
        fakeBoxes = new List<bool>();
        splineComputer = GetComponentInChildren<SplineComputer>();
        anim = GetComponentInChildren<Animator>();
        _addMaterialToMachine = GetComponentInChildren<AddMaterialToMachine>();
        _getMaterialFromMachine = GetComponentInChildren<GetMaterialFromMachine>();
        
        _waitTime = new WaitForSeconds(machineWorkingSpeed);
        StartCoroutine(MachineStartedWorking());
        
        remainingTxt.text = "0/" + maxTapedItemCount;
        if (!PlayerPrefs.HasKey("boxSpeedOnMachine"))
            PlayerPrefs.SetFloat("boxSpeedOnMachine", 0);
        
        upgradeBoxSpeed = PlayerPrefs.GetFloat("boxSpeedOnMachine");
        // animStartValue = anim.GetFloat(TagManager.ANIM_SPEED_FLOAT); 
    }

    private void Update()
    {
        if (!_isMachineWorking) return;
        foreach (var item in meshRenderers)
        {
            item.materials[2].mainTextureOffset += new Vector2(0, Time.deltaTime * -2);
        }

        corner.materials[1].mainTextureOffset += new Vector2(0, Time.deltaTime * -2);
    }

    private IEnumerator MachineStartedWorking()
    {
        while (true)
        {
            if (convertedMaterials.Count <= 0 || _getMaterialFromMachine.singleMaterial.Count>=maxTapedItemCount)
            {
                if (fakeBoxes.Count <= 0) stoppedUI.SetActive(true);
                yield return _waitTime;
            }
            else
            {
                stoppedUI.SetActive(false);
                _isMachineWorking = true;
                //      ps.Play();
                // anim.SetBool(TagManager.WALKING_BOOL_ANIM, true);
                _convertingItem = convertedMaterials[^1];
                convertedMaterials.Remove(_convertingItem);
                fakeBoxes.Add(true);
                remainingTxt.text = convertedMaterials.Count + "/" + maxTapedItemCount;
                _addMaterialToMachine.stackSystem.SetTheStackPositonBack(convertedMaterials.Count);
                _convertingItem.GetComponent<SplineFollower>().followSpeed =boxSpeedOnBantMachine+upgradeBoxSpeed+BoosterSystem.Instance.ProductionBoosterAmount;
                _convertingItem.transform.DOLocalJump(material_machine_enter_pos.position, jumpPower, 1, .15f)
                    .OnComplete(ItemBoxingProcess);
            }

            yield return _waitTime;
        }
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
        speed.followSpeed = .45f;
        yield return new WaitForSeconds(2f);
        speed.followSpeed = boxSpeedOnBantMachine+upgradeBoxSpeed+BoosterSystem.Instance.ProductionBoosterAmount;
    }

    public void PathEnded(GameObject item)
    {
        item.GetComponent<SplineFollower>().enabled = false;
        item.transform.DORotate(_stackSystem.MaterialDropPositon().rotation.eulerAngles, .15f);
        item.GetComponent<IItem>().SetStatus(outItemStatus, outTapedItemStatus);
        item.tag = TagManager.Default;
        fakeBoxes.Remove(fakeBoxes[^1]);
        CheckExtraPaletAddOrDelete(true);
        if (_floorCount >= 3) item.transform.GetChild(0).gameObject.SetActive(false);
        item.GetComponent<IItem>()
            .SetCurrentTween(
                item.transform.DOJump(_stackSystem.MaterialDropPositon().position, 1.15f, 1, .25f));

        if (fakeBoxes.Count <= 0) _isMachineWorking = false;
        _getMaterialFromMachine.singleMaterial.Add(item);
        getMaterialSave.SaveData(0);
        _stackSystem.DropPointHandle();
    }

    public void UpgradeSettings(float upgradeMachineWorkingSpeed, float upgradeProductSpeed)
    {
        boxSpeedOnBantMachine *= upgradeProductSpeed; 
        machineWorkingSpeed /= upgradeMachineWorkingSpeed;
        _waitTime = new WaitForSeconds(machineWorkingSpeed);
        _addMaterialToMachine._maxConvertedMaterial += 25;
        maxTapedItemCount += 50;
        remainingTxt.text = convertedMaterials.Count + "/" + maxTapedItemCount; 
    }


    int _floorCount = 0;

    public void CheckExtraPaletAddOrDelete(bool isAdd)
    {
        if (isAdd)
        {
            if (_getMaterialFromMachine.singleMaterial.Count <= 0) return;
            if (_floorCount < 3)
            {
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
                    _floorCount++;
                }
            }
            else
            {
                surplasBoxesUIObj.SetActive(true);
                surplasBoxesUiText.text = "+ " + (_getMaterialFromMachine.singleMaterial.Count - 48); 
            }
        }
        else
        { 
            // Paleti sil
            if (_getMaterialFromMachine.singleMaterial.Count < 16)
            {
                //tüm paletleri sil
                surplasBoxesUIObj.SetActive(false);
                if (_addedPalets.Count <= 0) return;
                _floorCount = 0;
                Destroy(_addedPalets[0]);
                _addedPalets.Clear();
                return;
            }

            if (_getMaterialFromMachine.singleMaterial.Count % 16 == 0)
            {
                surplasBoxesUIObj.SetActive(false);
                _floorCount--;
                var lastPalet = _addedPalets[^1];
                _addedPalets.Remove(lastPalet);
                Destroy(lastPalet);
            }
        }
    }

    private void OnEnable()
    {
        Events.OnMachineSpeedUpgrade += UpgradeBoxSpeed;
    }

    private void OnDisable()
    {
        Events.OnMachineSpeedUpgrade -= UpgradeBoxSpeed;
    }

    private void UpgradeBoxSpeed()
    {
        upgradeBoxSpeed = PlayerPrefs.GetFloat("boxSpeedOnMachine");
    }
}