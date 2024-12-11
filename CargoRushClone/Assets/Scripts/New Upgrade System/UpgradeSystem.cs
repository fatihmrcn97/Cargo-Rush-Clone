using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeSystem<T> : MonoBehaviour
{
    [SerializeField] private List<UpgradeConfig<T>> upgrades;

    private void Awake()
    {
        foreach (var upgrade in upgrades)
        {
            InitializeUpgrade(upgrade);
        }
    }

    private void InitializeUpgrade(UpgradeConfig<T> config)
    {
        // Initialize PlayerPrefs for the upgrade if not set
        if (!PlayerPrefs.HasKey(config.playerPrefKey))
            PlayerPrefs.SetInt(config.playerPrefKey, 0);

        // Set button listener
        config.upgradeButton.onClick.AddListener(() => ApplyUpgrade(config));

        // Initialize UI
        UpdateUpgradeUI(config);
    }

    private void ApplyUpgrade(UpgradeConfig<T> config)
    {
        int currentLevel = PlayerPrefs.GetInt(config.playerPrefKey);

        // Check if upgrade is possible
        if (currentLevel >= config.maxLevel || UIManager.instance.Score < config.costPerLevel[currentLevel])
            return;

        // Deduct cost and increase level
        UIManager.instance.ScoreAdd(-config.costPerLevel[currentLevel]);
        currentLevel++;
        PlayerPrefs.SetInt(config.playerPrefKey, currentLevel);

        // Update the upgrade effect (you can trigger specific actions based on type)
        float newEffectValue = config.baseEffectValue + currentLevel * config.increment;
        PlayerPrefs.SetFloat(config.playerPrefKey + "_Effect", newEffectValue);

        // Trigger specific action for this upgrade type
        OnUpgradeApplied(config, newEffectValue);

        // Update UI
        UpdateUpgradeUI(config);
    }

    private void UpdateUpgradeUI(UpgradeConfig<T> config)
    {
        int currentLevel = PlayerPrefs.GetInt(config.playerPrefKey);
        config.progressSlider.value = (float)currentLevel / config.maxLevel;

        if (currentLevel >= config.maxLevel)
        {
            config.upgradeButton.interactable = false;
            config.costText.text = "MAX";
        }
        else
        {
            config.costText.text = "$" + config.costPerLevel[currentLevel];
        }

        config.levelText.text = $"{currentLevel}/{config.maxLevel}";
    }

    private void OnEnable()
    {
        foreach (var upgrade in upgrades)
        {
            UpdateUpgradeUI(upgrade);
        }
    }
    
    protected abstract void OnUpgradeApplied(UpgradeConfig<T> config, float newEffectValue);
}