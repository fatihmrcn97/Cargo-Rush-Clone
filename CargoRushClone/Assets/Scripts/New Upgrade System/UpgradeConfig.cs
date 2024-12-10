using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class UpgradeConfig
{
    public UpgradeType type;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public Slider progressSlider;
    public Button upgradeButton;
    public int maxLevel = 10;
    public List<int> costPerLevel; // Cost to upgrade for each level
    public string playerPrefKey;  // Key for PlayerPrefs to store level
    public float baseEffectValue; // Base value for the upgrade effect
    public float increment;       // Increment per level
}