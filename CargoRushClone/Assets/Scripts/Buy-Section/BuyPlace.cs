using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyPlace : MonoBehaviour
{
    [SerializeField] private int MoneyForBuy = 25;

    [SerializeField] private TextMeshProUGUI moneyTxt;

    [SerializeField] private Slider slider;

    [SerializeField] private Slider waitBeforeBuySlider;

    private Coroutine BuySeats;
    private Coroutine StartBuy;

    private int _moneyDecreaseAmount = 10;

    private int _value;

    [SerializeField] private string saveName;

    private IBuyTrigger afterBought;

    private int _origMfb;

    private CancellationTokenSource _cts;

    private void Awake()
    {
        afterBought = GetComponent<IBuyTrigger>();
        _cts = new CancellationTokenSource();
    }

    private void Start()
    {
        _origMfb = MoneyForBuy;
        slider.maxValue = MoneyForBuy;
        moneyTxt.text = MoneyForBuy.ToString();
        _value = MoneyForBuy;

        if (PlayerPrefs.HasKey(saveName + "mfb"))
        {
            MoneyForBuy = PlayerPrefs.GetInt(saveName + "mfb");
            slider.value = _origMfb - PlayerPrefs.GetInt(saveName + "mfb");
            moneyTxt.text = MoneyForBuy.ToString();
        }
    }

    private void OnEnable()
    {
        CheckIfAlreadyBuy();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (UIManager.instance.Score > 0)
            StartBuy = StartCoroutine(CheckIfUserStillOn());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        // other.GetComponent<CollectedPeople>().moneyParticle.Stop();
        if (StartBuy != null)
        {
            StopCoroutine(StartBuy);
            StartCoroutine(ResetSlider());
        }

        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();

        if (BuySeats != null)
            StopCoroutine(BuySeats);
    }

    private IEnumerator CheckIfUserStillOn()
    {
        float elapsedTime = 0;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / 1f);
            waitBeforeBuySlider.value = Mathf.Lerp(0, 100, progress);
            yield return null;
        }

        //    BuySeats = StartCoroutine(BuySeat());

        BuySeat().Forget();
    }

    private async UniTaskVoid BuySeat()
    {
        // player.GetComponent<CollectedPeople>().moneyParticle.Play(); 
        while (MoneyForBuy > 0)
        {
            if (MoneyForBuy <= 50)
                _moneyDecreaseAmount = 1;


            if (UIManager.instance.Score <= 0)
            {
                //player.GetComponent<CollectedPeople>().moneyParticle.Stop();
                StartCoroutine(ResetSlider());
                break;
            }

            PlayerPrefs.SetInt(saveName + "mfb", MoneyForBuy);
            UIManager.instance.ScoreAdd(-_moneyDecreaseAmount);
            MoneyForBuy -= _moneyDecreaseAmount;

            Vibration.Vibrate(10);
            slider.value = _value - MoneyForBuy;
            moneyTxt.text = MoneyForBuy.ToString();
            await UniTask.DelayFrame(1, cancellationToken: _cts.Token);
        }
        // player.GetComponent<CollectedPeople>().moneyParticle.Stop();

        afterBought.PlaceBought();

        PlayerPrefs.SetInt(saveName, 1);
    }


    public IEnumerator ResetSlider()
    {
        float elapsedTime = 0;
        var currentValue = waitBeforeBuySlider.value;
        while (elapsedTime < .2f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / .2f);

            waitBeforeBuySlider.value = Mathf.Lerp(currentValue, 0, progress);
            yield return null;
        }
    }

    private void CheckIfAlreadyBuy()
    {
        if (PlayerPrefs.HasKey(saveName))
        {
            afterBought.AlreadyBought();
        }
    }
}