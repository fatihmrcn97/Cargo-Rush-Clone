
using DG.Tweening;
using UnityEngine;

public interface IItem
{
    ItemStatus ItemStatus();

    TapedItemStatus TapedItemStatus();

    void SetStatus(ItemStatus iStatus, TapedItemStatus tapedStatus);

    Tween CurrentTween();
    void SetCurrentTween(Tween tw);

    Transform ResetObj();
}
