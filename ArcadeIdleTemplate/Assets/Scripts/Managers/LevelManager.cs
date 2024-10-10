using System.Collections;  
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{


    [HideInInspector] public int currentLevel;

    [SerializeField] private int loopStartIndex;

    [SerializeField] private float waitBeforeLoading = 2f;

    [SerializeField] private GameObject loadingScreen;

    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private bool isRandom;

    [SerializeField] private Slider loadingBar;

    private int loopCount;

    public static LevelManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        AwakeInitializer();
    }

    private void AwakeInitializer()
    { 
         
        if (!PlayerPrefs.HasKey("Level")) PlayerPrefs.SetInt("Level", 1);
        currentLevel = PlayerPrefs.GetInt("Level");

        //Set Loop Count
        if (!PlayerPrefs.HasKey("LoopCount")) PlayerPrefs.SetInt("LoopCount", 0);
        loopCount = PlayerPrefs.GetInt("LoopCount");



        levelText.text = "Level " + (currentLevel + 1);

    }


    private IEnumerator Start()
    {
        var sceneIndex = currentLevel % SceneManager.sceneCountInBuildSettings;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
        loadingBar.maxValue = waitBeforeLoading;
        var timer = 0f;
        op.allowSceneActivation = false;
        while (timer <= waitBeforeLoading)
        {
            loadingBar.value = timer;
            timer += Time.deltaTime;
            yield return null;
        }
        op.allowSceneActivation = true;
        yield return new WaitForSeconds(1f);
        loadingScreen.SetActive(false);
    }

    public void NextLevel()
    {

        // TODO : Level geçiþ UI animasyonlarý 
         

        // 
        if (SceneManager.sceneCountInBuildSettings <= SceneManager.GetActiveScene().buildIndex + 1)
        {
            loopCount++; 

            PlayerPrefs.SetInt("LoopCount", loopCount);

            SceneManager.LoadScene(loopStartIndex);
        }
        else if (isRandom && loopCount > 0)
        {
            if (isRandom)
            {
                int randomScene = Random.Range(loopStartIndex, SceneManager.sceneCountInBuildSettings);
                if (randomScene == SceneManager.GetActiveScene().buildIndex)
                {
                    if (randomScene + 1 < SceneManager.sceneCountInBuildSettings) randomScene++;
                    else randomScene--;
                }
                SceneManager.LoadScene(randomScene);
            }
        }
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        levelText.text = "Level " + (currentLevel + 1); 

    }

    public void LevelCompleted()
    { 
           
        currentLevel++;
        PlayerPrefs.SetInt("Level", currentLevel);

        if (SceneManager.GetActiveScene().buildIndex % SceneManager.sceneCountInBuildSettings !=
            currentLevel + (loopCount * loopStartIndex) % SceneManager.sceneCountInBuildSettings && !isRandom) return;
         
    }

    public void RestartLevel()
    { 
        // Animasyonlarý yap

        // Sahneyi yeniden yukle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
