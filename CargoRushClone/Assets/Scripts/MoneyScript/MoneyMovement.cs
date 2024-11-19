using System.Collections;
using UnityEngine;
using DG.Tweening;
public class MoneyMovement : MonoBehaviour
{
    private const float MaxExclusive = .25f;

    public void MoneyCollectedMovement(GameObject player)
    { 
        StartCoroutine(MoneyStartGoingUp(player));
    }

    private IEnumerator MoneyStartGoingUp(GameObject player)
    { 
        transform.DOScale(new Vector3(1.35f, 1.35f, 1.35f), .25f); 
        float elapsedTime = 0;
        var origPos = transform.position;
        var origRot = transform.GetChild(0).transform.rotation;
        Vector3 targetPosition = player.transform.position + new Vector3(Random.Range(-.25f, MaxExclusive), Random.Range(2.75f, 4), Random.Range(-.25f, MaxExclusive));
        Quaternion targetRotation = Quaternion.Euler(Random.Range(100, 230), 180, Random.Range(120, 260));
        while (elapsedTime < .25f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / .25f);
            transform.position = Vector3.Lerp(origPos, targetPosition, progress);
            transform.GetChild(0).transform.rotation = Quaternion.Slerp(origRot, targetRotation, progress);
            yield return null;
        }

        yield return new WaitForSeconds(.15f);
        elapsedTime = 0;
        origPos = transform.position;
        var origScale = transform.localScale; 
        transform.DOScale(Vector3.zero, .5f).OnComplete(()=>Destroy(gameObject,.05f));
        while (elapsedTime < .25f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / .25f);
            transform.position = Vector3.Lerp(origPos, player.transform.position + new Vector3(0, .75f, 0), progress);
            yield return null;
        }
        UIManager.instance.ScoreAdd(1);
         
    }
}