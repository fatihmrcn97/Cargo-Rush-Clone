using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkerUpgradeSystem : MonoBehaviour
{
    public List<GameObject> AIs;
    
    public TextMeshProUGUI moneyTxtSpeed, moneyTxtCapasity,moneyTxtAIbuy;
    public TextMeshProUGUI speedUpgradeLevelText, capasityUpgradeLevelText;
    public TextMeshProUGUI aiBuyUpgradeLevelText;  
    
    
    [SerializeField] private Slider sliderSpeed, sliderCapacity;
    [SerializeField] private Slider sliderAIBuy;
    
    
    [Header("Capasity Upgrade Settings")] 
    [SerializeField] private int maxCapasityLevel = 10;
    [SerializeField] private int maxAIBuyLevel = 10;
    [SerializeField] private int maxSpeedLevel = 10;
    
    [SerializeField] private List<int> moneyForCapasity;
    [SerializeField] private List<int> moneyForSpeed;
    [SerializeField] private List<int> moneyForAIBUy;


    private Button _upgradeSpeedButton, _upgradeCapasityButton, _upgradeAIBuy;
    private readonly string _capasityUpgradePrefName = "CapasityUpgradeLevelAI";
    private readonly string _speedUpgradePrefName = "SpeedUpgradeLevelAI";
    private readonly string _AIBuyPrefName = "AIBuyPref";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(_AIBuyPrefName))
            PlayerPrefs.SetInt(_AIBuyPrefName, 0);
        
        if (!PlayerPrefs.HasKey(_capasityUpgradePrefName))
            PlayerPrefs.SetInt(_capasityUpgradePrefName, 0);

        if (!PlayerPrefs.HasKey(_speedUpgradePrefName))
            PlayerPrefs.SetInt(_speedUpgradePrefName, 0);

        _upgradeSpeedButton = moneyTxtSpeed.transform.parent.GetComponent<Button>();
        _upgradeCapasityButton = moneyTxtCapasity.transform.parent.GetComponent<Button>();
        _upgradeAIBuy = moneyTxtAIbuy.transform.parent.GetComponent<Button>();

        _upgradeSpeedButton.onClick.AddListener(SpeedUpgrade);
        _upgradeCapasityButton.onClick.AddListener(MaxFollowerCapasityUpgrade);
        _upgradeAIBuy.onClick.AddListener(AIBuy);
        
        sliderAIBuy.maxValue = maxAIBuyLevel;
        sliderSpeed.maxValue = maxSpeedLevel;
        sliderCapacity.maxValue = maxCapasityLevel;
    }

    public void AIBuy()
    {
        int currentLevel = PlayerPrefs.GetInt(_AIBuyPrefName);
        sliderAIBuy.value = currentLevel;
        if (currentLevel >= maxAIBuyLevel)
        {
            moneyTxtAIbuy.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtAIbuy.text = "MAX";
        }

        if (UIManager.instance.Score < moneyForAIBUy[currentLevel]) return;
        if (currentLevel >= maxAIBuyLevel) return;


        UIManager.instance.ScoreAdd(-moneyForAIBUy[currentLevel]);

        PlayerPrefs.SetInt(_AIBuyPrefName, currentLevel + 1);


        currentLevel = PlayerPrefs.GetInt(_AIBuyPrefName);
        AIs[currentLevel - 1].SetActive(true);
        aiBuyUpgradeLevelText.text = currentLevel + "/" + maxAIBuyLevel;
        sliderAIBuy.value = currentLevel;
        if (currentLevel >= maxAIBuyLevel)
        {
            moneyTxtAIbuy.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtAIbuy.text = "MAX";
        }
        else
            moneyTxtAIbuy.text = moneyForAIBUy[currentLevel].ToString();
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
        PlayerPrefs.SetInt("maxCollectedAI", PlayerPrefs.GetInt("maxCollectedAI") + 1);

        Events.OnCapasityUpgradeForAI?.Invoke();

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
        PlayerPrefs.SetFloat("AgentSpeed", currentLevel * 0.15f + .15f);

        Events.OnSpeedUpgradeForAI?.Invoke();

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
        
        
        int currentLevel3 = PlayerPrefs.GetInt(_AIBuyPrefName);
        sliderAIBuy.value = currentLevel3;
        aiBuyUpgradeLevelText.text = currentLevel3 + "/" + maxAIBuyLevel;

        for (int i = 0; i < currentLevel3; i++)
            AIs[i].SetActive(true);

        if (currentLevel3 >= maxAIBuyLevel)
        {
            moneyTxtAIbuy.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtAIbuy.text = "MAX";
        }
        else
            moneyTxtAIbuy.text = moneyForAIBUy[currentLevel3].ToString();
    }
}