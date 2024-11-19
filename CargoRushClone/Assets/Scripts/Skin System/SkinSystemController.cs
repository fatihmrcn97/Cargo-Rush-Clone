using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSystemController  
{
    private SkinsSO _skinsSO;
    private SkinModel _skinModel;
    public SkinSystemController(SkinsSO skinsSo,SkinModel skinModel)
    {
        _skinsSO = skinsSo;
        _skinModel = skinModel;
        //First skin already buyed
        PlayerPrefs.SetInt("skin"+0,1);
    }
    public void BuySkin(int skinIndex)
    {
        // Burası Buy aynı zamanda Para Check yap
        
        var choosenToBuySkin = _skinsSO.SkinsList[skinIndex];   
        PlayerPrefs.SetInt("skin"+skinIndex,1);
        
        // Skin Modelde (data olarak) bir değişim olması gerekiyor
        _skinModel.SkinBuy(skinIndex);
        

        // Oyun içi karakterde değişecek
        PlayerSkinHandler.Instance.ChangeSkinBody(skinIndex);
    }

   
     
}
