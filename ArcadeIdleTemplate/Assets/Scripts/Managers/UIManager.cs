using UnityEngine;
using TMPro;
using System.Collections;
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
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        AwakeInitializer();
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

    
}
