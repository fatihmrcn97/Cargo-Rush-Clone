using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    private int _score;
    public int Score => _score;

    [SerializeField] private TextMeshProUGUI scoreTxt;

    [SerializeField] private int scoreMultiplier=1;

    public GameObject maxUI;
    
    public Dictionary<string, int> InGameCollectablesCount;

    public List<Sprite> boxSprites;

    [SerializeField] private Button skinSysOpenBtn;
    [SerializeField] private Canvas skinSysUI;
    
    public GlobalVariablesSO globalVars;
    private void Awake()
    {
        
        Screen.SetResolution(Screen.width/2,Screen.height/2,true);
        Application.targetFrameRate = 60;
        if (instance == null)
            instance = this;
        AwakeInitializer();

        InGameCollectablesCount = new Dictionary<string, int>();
        
        skinSysOpenBtn.onClick.AddListener(()=>skinSysUI.enabled=true);
    }

    private void AwakeInitializer()
    {
        if (!PlayerPrefs.HasKey("score"))
            PlayerPrefs.SetInt("score", 0);

        _score = PlayerPrefs.GetInt("score");
        scoreTxt.text = _score.ToString();
         
    } 

    public void ScoreAdd(int a = 1)
    {
        if (_score > 0)
            _score += a * scoreMultiplier;
        else _score += a;

        PlayerPrefs.SetInt("score", _score);
        scoreTxt.text = _score.ToString();
    }

    public void SetSkinSystemImage(Sprite sprite)
    {
        skinSysOpenBtn.GetComponent<Image>().sprite = sprite;
        
    }

    
}
