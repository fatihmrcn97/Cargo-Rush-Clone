using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachinesUpgradeSystem : MonoBehaviour
{
    public TextMeshProUGUI moneyTxtSpeed;
    public TextMeshProUGUI speedUpgradeLevelText;
    [SerializeField] private Slider sliderSpeed;
    [SerializeField] private Slider sliderAIBuy;

    [SerializeField] private int maxSpeedLevel = 10;

    [SerializeField] private List<int> moneyForAIBUy;

    [SerializeField] private List<int> moneyForSpeed;


    private Button _upgradeSpeedButton, _upgradeCapasityButton;

    private readonly string _speedUpgradePrefName = "MachineSpeedUpgradeLevelAI";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(_speedUpgradePrefName))
            PlayerPrefs.SetInt(_speedUpgradePrefName, 0);

        _upgradeSpeedButton = moneyTxtSpeed.transform.parent.GetComponent<Button>();

        _upgradeSpeedButton.onClick.AddListener(SpeedUpgrade);
    }


    public void SpeedUpgrade()
    {
        int currentLevel = PlayerPrefs.GetInt(_speedUpgradePrefName);
        sliderSpeed.value = currentLevel;
        if (currentLevel >= maxSpeedLevel)
        {
            moneyTxtSpeed.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtSpeed.text = "MAX";
            return;
        }
 
        if (UIManager.instance.Score < moneyForSpeed[currentLevel]) return;


        UIManager.instance.ScoreAdd(-moneyForSpeed[currentLevel]);

        PlayerPrefs.SetInt(_speedUpgradePrefName, currentLevel + 1);
        PlayerPrefs.SetFloat("boxSpeedOnMachine", currentLevel * 0.1f + .1f);

        Events.OnMachineSpeedUpgrade?.Invoke();

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
    }
}