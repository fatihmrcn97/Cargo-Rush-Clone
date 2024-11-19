using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cargo RushClone/Skin System")]
public class SkinsSO : ScriptableObject
{
    public List<Skin> SkinsList;
}

[System.Serializable]
public class Skin
{
    public Sprite skinImage;
    public int skinBuyMoney;
    public float incomeUpgradeAmount;
    public int capasityUpgradeAmount;
}