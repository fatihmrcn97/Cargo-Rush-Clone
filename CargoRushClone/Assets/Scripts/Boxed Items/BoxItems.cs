using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoxItems : MonoBehaviour, IItem
{
    [SerializeField] private ItemStatus itemStatus;
    [SerializeField] private TapedItemStatus tapedItemStatus;

    private Tween _currentTween;

    public ItemStatus ItemStatus()
    {
        return itemStatus;
    }

    public TapedItemStatus TapedItemStatus()
    {
        return tapedItemStatus;
    }

    public void SetStatus(ItemStatus iStatus, TapedItemStatus tapedStatus)
    {
        itemStatus = iStatus;
        tapedItemStatus = tapedStatus;
    }

    public Tween CurrentTween()
    {
        return _currentTween;
    }

    public void SetCurrentTween(Tween tw)
    {
        _currentTween = tw;
    }

    public Transform ResetObj()
    {
        // itemi geri poola dondurceksem resetleme işlemeri burda olacak
        return transform;
    }
}