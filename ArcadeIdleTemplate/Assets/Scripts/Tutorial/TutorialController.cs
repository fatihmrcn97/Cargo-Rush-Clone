using UnityEngine;
using UnityEngine.Events;

public class TutorialController : MonoBehaviour
{

    [SerializeField] string biOnceki;
    [SerializeField] bool isBionceki;

    public UnityEvent a;
    public UnityEvent ifAlreadyBought;

    private void Start()
    {
        if (PlayerPrefs.HasKey(gameObject.name))
            ifAlreadyBought?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isBionceki)
        {
            if (!PlayerPrefs.HasKey(biOnceki)) return;

            if (PlayerPrefs.HasKey(gameObject.name)) return;

            if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
                a?.Invoke();

            PlayerPrefs.SetInt(gameObject.name, 0);
        }
        else
        {

            if (PlayerPrefs.HasKey(gameObject.name)) return;

            if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
                a?.Invoke();

            PlayerPrefs.SetInt(gameObject.name, 0);
        }
    }
}
