using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUpgSystemNew : MonoBehaviour
{
    public TextMeshProUGUI moneyTxtSpeed, moneyTxtCapasity;
    public TextMeshProUGUI speedUpgradeLevelText, capasityUpgradeLevelText;
    [SerializeField] private Slider sliderSpeed, sliderCapacity;

    [SerializeField] private PlayerStackController _playerStackController;
    [SerializeField] private PlayerMovement playerMovement;
    

    [Header("Capasity Upgrade Settings")] [SerializeField]
    private int maxCapasityLevel = 10;
    [SerializeField] private int maxSpeedLevel = 10;
    [SerializeField] private List<int> moneyForCapasity;
    [SerializeField] private List<int> moneyForSpeed;
    


    private Button _upgradeSpeedButton, _upgradeCapasityButton;
    private readonly string _capasityUpgradePrefName = "CapasityUpgradeLevel";
    private readonly string _speedUpgradePrefName = "SpeedUpgradeLevel";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(_capasityUpgradePrefName))
            PlayerPrefs.SetInt(_capasityUpgradePrefName, 0);

        if (!PlayerPrefs.HasKey(_speedUpgradePrefName))
            PlayerPrefs.SetInt(_speedUpgradePrefName, 0);

        _upgradeSpeedButton = moneyTxtSpeed.transform.parent.GetComponent<Button>();
        _upgradeCapasityButton = moneyTxtCapasity.transform.parent.GetComponent<Button>();

        _upgradeSpeedButton.onClick.AddListener(SpeedUpgrade);
        _upgradeCapasityButton.onClick.AddListener(MaxFollowerCapasityUpgrade);
    }


    public void MaxFollowerCapasityUpgrade()
    {
        int currentLevel = PlayerPrefs.GetInt(_capasityUpgradePrefName);
        sliderCapacity.value = currentLevel;
        if (currentLevel >= maxCapasityLevel)
        {
            moneyTxtCapasity.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtCapasity.text = "MAX";
        }

        if (UIManager.instance.Score < moneyForCapasity[currentLevel]) return;
        if (currentLevel >= maxCapasityLevel) return;


        UIManager.instance.ScoreAdd(-moneyForCapasity[currentLevel]);

        PlayerPrefs.SetInt(_capasityUpgradePrefName, currentLevel + 1);
        PlayerPrefs.SetInt("maxCollected", PlayerPrefs.GetInt("maxCollected") + 2);

        _playerStackController.MaxStackCountUpdated();

        currentLevel = PlayerPrefs.GetInt(_capasityUpgradePrefName);
        capasityUpgradeLevelText.text = currentLevel + "/" + maxCapasityLevel;
        sliderCapacity.value = currentLevel;
        if (currentLevel >= maxCapasityLevel)
        {
            moneyTxtCapasity.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtCapasity.text = "MAX";
        }
        else
            moneyTxtCapasity.text = moneyForCapasity[currentLevel].ToString();
    }


    public void SpeedUpgrade()
    {
        int currentLevel = PlayerPrefs.GetInt(_speedUpgradePrefName);
        sliderSpeed.value = currentLevel;
        if (currentLevel >= maxSpeedLevel)
        {
            moneyTxtSpeed.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtSpeed.text = "MAX";
        }

        if (currentLevel >= maxSpeedLevel) return;
        if (UIManager.instance.Score < moneyForSpeed[currentLevel]) return;


        UIManager.instance.ScoreAdd(-moneyForSpeed[currentLevel]);

        PlayerPrefs.SetInt(_speedUpgradePrefName, currentLevel + 1);
        PlayerPrefs.SetFloat("speedUpgrade", currentLevel * 0.25f + .25f);

        playerMovement.SpeedUpdated();

        currentLevel = PlayerPrefs.GetInt(_speedUpgradePrefName);
        speedUpgradeLevelText.text = currentLevel + "/" + maxSpeedLevel;
        sliderSpeed.value = currentLevel;
        if (currentLevel >= maxSpeedLevel)
        {
            moneyTxtSpeed.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtSpeed.text = "MAX";
        }
        else
            moneyTxtSpeed.text = moneyForSpeed[currentLevel].ToString();
    }

    private void OnEnable()
    {
        int currentLevel = PlayerPrefs.GetInt(_speedUpgradePrefName);
        sliderSpeed.value = currentLevel;
        if (currentLevel >= maxSpeedLevel)
        {
            moneyTxtSpeed.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtSpeed.text = "MAX";
        }
        else
            moneyTxtSpeed.text = moneyForSpeed[currentLevel].ToString();

        speedUpgradeLevelText.text = currentLevel + "/" + maxSpeedLevel;

        int currentLevel2 = PlayerPrefs.GetInt(_capasityUpgradePrefName);
        sliderCapacity.value = currentLevel2;
        capasityUpgradeLevelText.text = currentLevel2 + "/" + maxCapasityLevel;
        if (currentLevel2 >= maxCapasityLevel)
        {
            moneyTxtCapasity.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtCapasity.text = "MAX";
        }
        else
            moneyTxtCapasity.text = moneyForCapasity[currentLevel2].ToString();
    }


  
}