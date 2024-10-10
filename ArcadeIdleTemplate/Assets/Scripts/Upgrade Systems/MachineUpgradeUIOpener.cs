using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MachineUpgradeUIOpener : MonoBehaviour
{
    [SerializeField] private GameObject uiSection;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            Level_TxtSpeed.transform.parent.gameObject.GetComponent<Button>().onClick.AddListener(BuyUpgradeSpeed);
            Level_TxtCapasity.transform.parent.gameObject.GetComponent<Button>().onClick.AddListener(MaxFollowerCapasityUpgrade);

            CheckStart();

            uiSection.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            Level_TxtSpeed.transform.parent.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            Level_TxtCapasity.transform.parent.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();

            uiSection.SetActive(false); 
        }
    }


    [SerializeField] private string machine_upgrade_name;


    [SerializeField] private TextMeshProUGUI moneyTxtSpeed , moneyTxtCapasity;
    [SerializeField] private TextMeshProUGUI Level_TxtSpeed , Level_TxtCapasity;

    private MachineController machineController;

    [SerializeField] private int machineMaxStackCountStartValue=12;

    [SerializeField] private AddMaterialToMachine addMaterialToMachine;
    private void Awake()
    {
        machineController = GetComponentInParent<MachineController>();

        if (!PlayerPrefs.HasKey("maxCollectedLevel" + machine_upgrade_name))
            PlayerPrefs.SetInt("maxCollectedLevel" + machine_upgrade_name, 0);

        if (!PlayerPrefs.HasKey("upgSpeed" + machine_upgrade_name))
            PlayerPrefs.SetInt("upgSpeed" + machine_upgrade_name, 0);

    }
    private void OnEnable()
    {
        StartCoroutine(RaceConditionHandler());
    }

    private IEnumerator RaceConditionHandler()
    {
        yield return new WaitForSeconds(.01f); 
        CheckStart();
    }

    private void CheckStart()
    {
        int currentLevel = (int)PlayerPrefs.GetFloat("upgSpeed" + machine_upgrade_name);
        Level_TxtSpeed.text = currentLevel.ToString();

        machineController.MachineUpgradeUpdate(currentLevel);

        if (currentLevel >= 5)
        {
            moneyTxtSpeed.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtSpeed.text = "MAX";
        }
        else
            moneyTxtSpeed.text = MoneyToBuy(currentLevel).ToString();

        int currentLevel2 = PlayerPrefs.GetInt("maxCollectedLevel" + machine_upgrade_name);
        Level_TxtCapasity.text = currentLevel2.ToString();
        addMaterialToMachine.maxConvertedMaterial = machineMaxStackCountStartValue + (currentLevel2 * 2);
        if (currentLevel2 >= 5)
        {
            moneyTxtCapasity.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtCapasity.text = "MAX";
        }
        else
            moneyTxtCapasity.text = MoneyToBuy(currentLevel2).ToString();
    }


    public void BuyUpgradeSpeed()
    {
        int currentLevel = (int)PlayerPrefs.GetFloat("upgSpeed"+machine_upgrade_name);
        Level_TxtSpeed.text = currentLevel.ToString();
        if (currentLevel >= 5)
        {
            moneyTxtSpeed.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtSpeed.text = "MAX";
        }
        if (currentLevel >= 5) return;
        if (UIManager.instance.Score < MoneyToBuy(currentLevel)) return;

        UIManager.instance.ScoreAdd(-MoneyToBuy(currentLevel));

        PlayerPrefs.SetFloat("upgSpeed" + machine_upgrade_name, currentLevel + 1);
        PlayerPrefs.SetFloat("speedUpgrade" + machine_upgrade_name, currentLevel + .5f);

        currentLevel = (int)PlayerPrefs.GetFloat("upgSpeed" + machine_upgrade_name);
        Level_TxtSpeed.text = currentLevel.ToString();
        machineController.anim.SetFloat(TagManager.ANIM_SPEED_FLOAT, 1 + currentLevel);

        if (currentLevel >= 5)
        {
            moneyTxtSpeed.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtSpeed.text = "MAX";
        }
        else
            moneyTxtSpeed.text = MoneyToBuy(currentLevel).ToString();
    }

    public void MaxFollowerCapasityUpgrade()
    {

        int currentLevel = PlayerPrefs.GetInt("maxCollectedLevel"+ machine_upgrade_name);
        Level_TxtCapasity.text = currentLevel.ToString();
        if (currentLevel >= 5)
        {
            moneyTxtCapasity.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtCapasity.text = "MAX";
        }
        if (UIManager.instance.Score < MoneyToBuy(currentLevel)) return;
        if (currentLevel >= 5) return;

        UIManager.instance.ScoreAdd(-MoneyToBuy(currentLevel));

        PlayerPrefs.SetInt("maxCollectedLevel" + machine_upgrade_name, currentLevel + 1);
        PlayerPrefs.SetInt("maxCollected" + machine_upgrade_name, PlayerPrefs.GetInt("maxCollected" + machine_upgrade_name) + 2);

        currentLevel = PlayerPrefs.GetInt("maxCollectedLevel" + machine_upgrade_name);
        Level_TxtCapasity.text = currentLevel.ToString();
        addMaterialToMachine.maxConvertedMaterial = machineMaxStackCountStartValue + (currentLevel * 2);

        if (currentLevel >= 5)
        {
            moneyTxtCapasity.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtCapasity.text = "MAX";
        }
        else
            moneyTxtCapasity.text = MoneyToBuy(currentLevel).ToString();
    }



    private int MoneyToBuy(int currentLevel)
    {
        return currentLevel switch
        {
            0 => 30,
            1 => 50,
            2 => 100,
            3 => 200,
            4 => 500,
            5 => 600,
            _ => 0,
        };
    }
}
