using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TrashCanController : MonoBehaviour
{
    private Coroutine DeleteCorotine;

    [SerializeField] private Transform trashPos;

    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            DeleteCorotine= StartCoroutine(StartDeleting(other.GetComponent<PlayerStackController>()));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (DeleteCorotine != null) StopCoroutine(DeleteCorotine);
        anim.SetBool("open", false);
    }

    private IEnumerator StartDeleting(PlayerStackController stackController)
    {
        anim.SetBool("open", true);
        yield return new WaitForSeconds(.1f);
        List<GameObject> tempList = new(stackController.stackedMaterials);
        foreach (var item in tempList)
        {
            item.transform.DOJump(trashPos.position, .5f, 1, .25f).OnComplete(()=> {
                stackController.stackedMaterials.Remove(item);
                Destroy(item);
                Events.MaterialStackedEvent?.Invoke();
            });
            stackController.StackPositionHandler();
            yield return new WaitForSeconds(.25f);
        }
    }

}
