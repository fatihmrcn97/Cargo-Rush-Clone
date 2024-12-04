using System.Collections.Generic;
using TMPro;
using UnityEngine; 
using UnityEngine.UI;

public class CustomerUpgradeSystem : MonoBehaviour
{
    public TextMeshProUGUI moneyTxtSpeed;
    public TextMeshProUGUI moneyTxtWaitTime;

    public TextMeshProUGUI speedUpgradeLevelText;
    public TextMeshProUGUI waitTimeUpgradeLevelText;

    [SerializeField] private Slider sliderSpeed;
    [SerializeField] private Slider sliderWaitTime;


    [Header("Capasity Upgrade Settings")] [SerializeField]
    private int maxWaitTimeLevel = 10; 
    [SerializeField] private int maxMoneyEarnAmountLevel = 10;
    [SerializeField] private List<int> moneyForWaitTime;
    [SerializeField] private List<int> moneyForMoneyEarnAmount;
 
    private Button _upgradeSpeedButton, _upgradeWaitTimeButton;

    private readonly string
        _speedUpgradePrefName =
            "MoneyEarnAmountUpgradeLevel"; // upgradeTotalDurationDecraser  PREF FOR WAİT TİME DECREASE

    private readonly string _waitTimeUpgradePrefName = "WaitTimeUpgradeLevel";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(_waitTimeUpgradePrefName))
            PlayerPrefs.SetInt(_waitTimeUpgradePrefName, 0);

        if (!PlayerPrefs.HasKey(_speedUpgradePrefName))
            PlayerPrefs.SetInt(_speedUpgradePrefName, 0);

        _upgradeSpeedButton = moneyTxtSpeed.transform.parent.GetComponent<Button>();
        _upgradeWaitTimeButton = moneyTxtWaitTime.transform.parent.GetComponent<Button>();

        _upgradeSpeedButton.onClick.AddListener(SpeedUpgrade);
        _upgradeWaitTimeButton.onClick.AddListener(CustomerWaitTimeUpgrade);
    }


    public void CustomerWaitTimeUpgrade()
    {
        int currentLevel = PlayerPrefs.GetInt(_waitTimeUpgradePrefName);
        sliderWaitTime.value = currentLevel;
        if (currentLevel >= maxWaitTimeLevel)
        {
            moneyTxtWaitTime.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtWaitTime.text = "MAX";
        }

        if (UIManager.instance.Score < moneyForWaitTime[currentLevel]) return;
        if (currentLevel >= maxWaitTimeLevel) return;


        UIManager.instance.ScoreAdd(-moneyForWaitTime[currentLevel]);

        PlayerPrefs.SetInt(_waitTimeUpgradePrefName, currentLevel + 1);
        PlayerPrefs.SetFloat("upgradeTotalDurationDecraser",
            PlayerPrefs.GetFloat("upgradeTotalDurationDecraser") + .5f);

        Events.OnUpgradeWaitTime?.Invoke();

        currentLevel = PlayerPrefs.GetInt(_waitTimeUpgradePrefName);
        waitTimeUpgradeLevelText.text = currentLevel + "/" + maxWaitTimeLevel;
        sliderWaitTime.value = currentLevel;
        if (currentLevel >= maxWaitTimeLevel)
        {
            moneyTxtWaitTime.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtWaitTime.text = "MAX";
        }
        else
            moneyTxtWaitTime.text = moneyForWaitTime[currentLevel].ToString();
    }


    public void SpeedUpgrade()
    {
        int currentLevel = PlayerPrefs.GetInt(_speedUpgradePrefName);
        sliderSpeed.value = currentLevel;
        if (currentLevel >= maxMoneyEarnAmountLevel)
        {
            moneyTxtSpeed.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtSpeed.text = "MAX";
        }

        if (currentLevel >= maxMoneyEarnAmountLevel) return;
        if (UIManager.instance.Score < moneyForMoneyEarnAmount[currentLevel]) return;
        
        UIManager.instance.ScoreAdd(-moneyForMoneyEarnAmount[currentLevel]);
        PlayerPrefs.SetInt(_speedUpgradePrefName, currentLevel + 1);
        currentLevel = PlayerPrefs.GetInt(_speedUpgradePrefName);
        PlayerPrefs.SetInt("moneyEarnAmountUpgrade", currentLevel * 2);
        speedUpgradeLevelText.text = currentLevel + "/" + maxMoneyEarnAmountLevel;
        Events.OnMoneyEarnAmountUpgrade?.Invoke();
        sliderSpeed.value = currentLevel;
        if (currentLevel >= maxMoneyEarnAmountLevel)
        {
            moneyTxtSpeed.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtSpeed.text = "MAX";
        }
        else
            moneyTxtSpeed.text = moneyForMoneyEarnAmount[currentLevel].ToString();
    }

    private void OnEnable()
    {
        int currentLevel = PlayerPrefs.GetInt(_speedUpgradePrefName);
        sliderSpeed.value = currentLevel;
        if (currentLevel >= maxMoneyEarnAmountLevel)
        {
            moneyTxtSpeed.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtSpeed.text = "MAX";
        }
        else
            moneyTxtSpeed.text = moneyForMoneyEarnAmount[currentLevel].ToString();

        speedUpgradeLevelText.text = currentLevel + "/" + maxMoneyEarnAmountLevel;

        int currentLevel2 = PlayerPrefs.GetInt(_waitTimeUpgradePrefName);
        sliderWaitTime.value = currentLevel;
        waitTimeUpgradeLevelText.text = currentLevel2 + "/" + maxWaitTimeLevel;
        if (currentLevel2 >= maxWaitTimeLevel)
        {
            moneyTxtWaitTime.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtWaitTime.text = "MAX";
        }
        else
            moneyTxtWaitTime.text = moneyForWaitTime[currentLevel2].ToString();
    }


  
}