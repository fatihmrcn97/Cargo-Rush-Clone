using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCharacterSystem : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerStackController _playerStackController;

    public TextMeshProUGUI moneyTxtSpeed, moneyTxtCapasity;

    public GameObject uiOpen;

    [SerializeField] private Slider sliderSpeed, sliderCapacity;

    
    private void Start()
    {
        if (!PlayerPrefs.HasKey("maxCollectedLevel"))
            PlayerPrefs.SetInt("maxCollectedLevel", 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            _playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            _playerStackController = other.gameObject.GetComponent<PlayerStackController>();
            uiOpen.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            uiOpen.SetActive(false);
    }

    public void MaxFollowerCapasityUpgrade()
    {

        int currentLevel = PlayerPrefs.GetInt("maxCollectedLevel");
        sliderCapacity.value = currentLevel;
        if (currentLevel >= 4)
        {
            moneyTxtCapasity.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtCapasity.text = "MAX";
        }
        if (UIManager.instance.Score < MoneyToBuy(currentLevel)) return;
        if (currentLevel >= 4) return;

        UIManager.instance.ScoreAdd(-MoneyToBuy(currentLevel));

        PlayerPrefs.SetInt("maxCollectedLevel", currentLevel + 1);
        PlayerPrefs.SetInt("maxCollected", PlayerPrefs.GetInt("maxCollected") + 5);

        _playerStackController.MaxStackCountUpdated();

        currentLevel = PlayerPrefs.GetInt("maxCollectedLevel");
        sliderCapacity.value = currentLevel;
        if (currentLevel >= 4)
        {
            moneyTxtCapasity.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtCapasity.text = "MAX";
        }
        else
            moneyTxtCapasity.text = MoneyToBuy(currentLevel).ToString();
    }


    public void SpeedUpgrade()
    {
        int currentLevel = (int)PlayerPrefs.GetFloat("upgradeSpeed");
        sliderSpeed.value = currentLevel;
        if (currentLevel >= 4)
        {
            moneyTxtSpeed.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtSpeed.text = "MAX";
        }
        if (currentLevel >= 4) return;
        if (UIManager.instance.Score < MoneyToBuy(currentLevel)) return;

        UIManager.instance.ScoreAdd(-MoneyToBuy(currentLevel));

        PlayerPrefs.SetFloat("upgradeSpeed", currentLevel + 1);
        PlayerPrefs.SetFloat("speedUpgrade", currentLevel*0.25f + .25f);

        _playerMovement.SpeedUpdated();

        currentLevel = (int)PlayerPrefs.GetFloat("upgradeSpeed");
        sliderSpeed.value = currentLevel;
        if (currentLevel >= 4)
        {
            moneyTxtSpeed.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtSpeed.text = "MAX";
        }
        else
            moneyTxtSpeed.text = MoneyToBuy(currentLevel).ToString();

    }
    private void OnEnable()
    {
        int currentLevel = (int)PlayerPrefs.GetFloat("upgradeSpeed");
        sliderSpeed.value = currentLevel;
        if (currentLevel >= 4)
        {

            moneyTxtSpeed.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtSpeed.text = "MAX";
        }
        else
            moneyTxtSpeed.text = MoneyToBuy(currentLevel).ToString();

        int currentLevel2 = PlayerPrefs.GetInt("maxCollectedLevel");
        sliderCapacity.value = currentLevel;
        if (currentLevel2 >= 4)
        {
            moneyTxtCapasity.transform.parent.GetComponent<Button>().interactable = false;
            moneyTxtCapasity.text = "MAX";
        }
        else
            moneyTxtCapasity.text = MoneyToBuy(currentLevel2).ToString();

    }



    private int MoneyToBuy(int currentLevel)
    {
        return currentLevel switch
        {
            0 => 25,
            1 => 75,
            2 => 150,
            3 => 250,
            4 => 500,
            5 => 600,
            _ => 0,
        };
    }
 

}