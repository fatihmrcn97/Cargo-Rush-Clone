using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinSystemView : MonoBehaviour
{  
    [SerializeField] private List<GameObject> skinsObjects;

    [SerializeField] private SkinsSO skinsSO;

    [SerializeField] private TextMeshProUGUI incomeIncreaseTxt, cargoCapasityTxt, moneyBtnText;

    [SerializeField] private Sprite greyButtonSprite, yellowButtonSprite;

    [SerializeField] private GameObject moneySpriteObject;

    [SerializeField] private List<DOTweenAnimation> animations;

    [SerializeField] private Button selectBtn,watchAdBtn;

    [SerializeField] private GameObject unlockMapTxtImage;

    [SerializeField] private GameObject selectedBtnObject;
    

    [SerializeField] private GameObject upArrow,downArrow;
    [SerializeField] private Scrollbar scrollbar;
    
    

    private Button _skinBuyButton;

    private SkinModel skinModel;
    private SkinSystemController skinSystemController;

    private int _choosenSkin;
    private Skin _selectedSkin;

    private void Awake()
    {
        skinModel = new SkinModel(skinsSO);
        skinSystemController = new SkinSystemController(skinsSO, skinModel);
        _skinBuyButton = moneyBtnText.GetComponentInParent<Button>();
        ShowCurrentSkinForBuy(skinModel.currentSkinIndex);
        skinModel.OnSkinChanged += SkinBuyedAndChanged;
        
    }

    private void Start()
    {
        for (int i = 0; i < skinsObjects.Count; i++)
        {
            var skinButton = skinsObjects[i].GetComponentInChildren<Button>();
            int index = i; // Coppy the index otherwise index always be 6
            skinButton.onClick.AddListener(() => ShowCurrentSkinForBuy(index));
        }

        _skinBuyButton.onClick.AddListener(() =>
            skinSystemController.BuySkin(_choosenSkin)); // Its buy skin if its already buyed change

        selectBtn.onClick.AddListener(() =>skinSystemController.BuySkin(_choosenSkin));
        scrollbar.onValueChanged.AddListener((val)=>ScrollBarOnValueChanged(val));
        
        skinSystemController.BuySkin(skinModel.currentSkinIndex);
        SkinBuyedCheckMark();
        CheckIfAlreadyBuyed(); 
    } 

    private void ShowCurrentSkinForBuy(int i)
    {
        var selectedSkin = skinsSO.SkinsList[i];
        _selectedSkin = selectedSkin;
        _choosenSkin = i;
   //Todo :     mainCharaterImage.sprite = selectedSkin.skinImage;
        Events.OnPlayerSkinChangePreview?.Invoke(i);
        incomeIncreaseTxt.text = "+%" + selectedSkin.incomeUpgradeAmount + " Income";
        cargoCapasityTxt.text = "+" + selectedSkin.capasityUpgradeAmount + " Capasity";
        moneyBtnText.text = selectedSkin.skinBuyMoney + "K";
        animations.ForEach(item=>item.DORestart()); 
        CheckIfAlreadyBuyed();
        CheckIfMoneyEnough();
        CheckIfAlreadyBuyedAndSelected();
    }

    public void ReOpenSkinWindowSettings()
    {
        // UI ile atandi
        //ShowCurrentSkinForBuy(_choosenSkin);
        ShowCurrentSkinForBuy(skinModel.currentSkinIndex);
    }
 
    private void ScrollBarOnValueChanged(float value)
    {
        if(!transform.parent.GetComponent<Canvas>().isActiveAndEnabled) return;
        upArrow.SetActive(!(value > 0.75));
        downArrow.SetActive(!(value < .15));
    }

    private void CheckIfMoneyEnough()
    {
        var selectedSkinMoney = _selectedSkin.skinBuyMoney;
        if (skinModel.buyedSkins[_choosenSkin] == 1)
        {
            _skinBuyButton.interactable = true; // Skin zaten satin alinmisa buton aktif kalsın
            return;
        }
        _skinBuyButton.interactable = UIManager.instance.Score >= selectedSkinMoney;
    }

    private void SkinBuyedAndChanged(Skin updatedSkin)
    {
        //OnSkinChanged takip ediyor
        //Todo :    mainCharaterImage.sprite = updatedSkin.skinImage;
        Events.OnPlayerSkinChangePreview?.Invoke(_choosenSkin);
        CheckIfAlreadyBuyed();
        SkinBuyedCheckMark();
        var currentSkinSprite = skinsObjects[_choosenSkin].transform.GetChild(0).GetComponent<Image>().sprite;
        ChoosenButtonBackgroundHandle(skinsObjects[_choosenSkin].GetComponent<Image>());
        UIManager.instance.SetSkinSystemImage(currentSkinSprite);
        selectedBtnObject.SetActive(true);
        CheckIfAlreadyBuyedAndSelected();
    }

    private void CheckIfAlreadyBuyedAndSelected()
    {
        if (skinModel.buyedSkins[_choosenSkin] == 1)
        {
            selectBtn.gameObject.SetActive(_choosenSkin != skinModel.currentSkinIndex);
        }
    }
    private void CheckIfAlreadyBuyed()
    {
        if (skinModel.buyedSkins[_choosenSkin] == 1)
        { 
            selectBtn.gameObject.SetActive(true); 
            moneyBtnText.transform.parent.gameObject.SetActive(false);
            watchAdBtn.gameObject.SetActive(false);
            unlockMapTxtImage.SetActive(false);
        }
        else
        {
            unlockMapTxtImage.SetActive(true);
            selectBtn.gameObject.SetActive(false); 
            moneyBtnText.transform.parent.gameObject.SetActive(true);
            watchAdBtn.gameObject.SetActive(true);
        }
    }

    private void ChoosenButtonBackgroundHandle(Image selectedImage)
    {
        MakeGreyAllButton();
        selectedImage.sprite = yellowButtonSprite;
    }

    private void MakeGreyAllButton()
    {
        skinsObjects.ForEach(obj => obj.GetComponent<Image>().sprite = greyButtonSprite);
    }

    private void SkinBuyedCheckMark()
    {
        // for (int i = 0; i < skinsObjects.Count; i++)
        // {
        //     if (skinModel.buyedSkins[i] == 1)
        //     {
        //         skinsObjects[i].transform.GetChild(1).gameObject.SetActive(true);
        //     }
        // }
        skinsObjects.ForEach(item => item.transform.GetChild(1).gameObject.SetActive(false));
        skinsObjects[skinModel.currentSkinIndex].transform.GetChild(1).gameObject.SetActive(true);  
    }

    private void OnDestroy()
    {
        skinModel.OnSkinChanged -= SkinBuyedAndChanged;
    }
}