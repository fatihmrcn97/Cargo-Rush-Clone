using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSystemController
{
    private SkinsSO _skinsSO;
    private SkinModel _skinModel;

    public SkinSystemController(SkinsSO skinsSo, SkinModel skinModel)
    {
        _skinsSO = skinsSo;
        _skinModel = skinModel;
        //First skin already buyed
        PlayerPrefs.SetInt("skin" + 0, 1);
    }

    public void BuySkin(int skinIndex)
    {
        if (_skinModel.buyedSkins[skinIndex] == 1)
        {
            // Skin zaten satin alinmis
          //  PlayerSkinHandler.Instance.ChangeSkinBody(skinIndex);
            Events.OnPlayerSkinChange?.Invoke(skinIndex);
            _skinModel.SkinBuy(skinIndex);
            PlayerPrefs.SetInt("UsingSkinIndex", skinIndex);
        }
        else
        {
            var skin = _skinsSO.SkinsList[skinIndex];
            if (UIManager.instance.Score <= skin.skinBuyMoney) return;
            UIManager.instance.ScoreAdd(-skin.skinBuyMoney);
            Events.OnPlayerSkinChange?.Invoke(skinIndex);
           // PlayerSkinHandler.Instance.ChangeSkinBody(skinIndex);
            PlayerPrefs.SetInt("skin" + skinIndex, 1);
            _skinModel.SkinBuy(skinIndex);
            PlayerPrefs.SetInt("UsingSkinIndex", skinIndex);
        }
    }
}