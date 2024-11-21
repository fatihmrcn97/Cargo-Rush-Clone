using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinModel
{
    public Action<Skin> OnSkinChanged;
    public SkinsSO SkinsSo { get; private set; }
    public int currentSkinIndex { get; private set; }

    public Dictionary<int, int> buyedSkins;
   
    public SkinModel(SkinsSO skins)
    {
        if (PlayerPrefs.HasKey("UsingSkinIndex"))
            currentSkinIndex = PlayerPrefs.GetInt("UsingSkinIndex");
        else
        {
            currentSkinIndex = 0;
            PlayerPrefs.SetInt("UsingSkinIndex", currentSkinIndex);
        }
        SkinsSo = skins; 
        InitializeBuyedSkins();
    }

    private void InitializeBuyedSkins()
    {
        buyedSkins = new Dictionary<int, int>();
        for (int i = 0; i < SkinsSo.SkinsList.Count; i++)
        {
            buyedSkins.Add(i, PlayerPrefs.GetInt("skin"+i) == 1 ? 1 : 0);
        }

        buyedSkins[0] = 1; // Default skin
    }

    public void SkinBuy(int skinIndex)
    {
        buyedSkins[skinIndex] = 1;
        OnSkinChanged?.Invoke(SkinsSo.SkinsList[skinIndex]);
        currentSkinIndex = skinIndex;
    }
}