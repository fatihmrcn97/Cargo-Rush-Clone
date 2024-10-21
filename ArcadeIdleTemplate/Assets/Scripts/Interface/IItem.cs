
using DG.Tweening;

public interface IItem
{
    ItemStatus ItemStatus();

    TapedItemStatus TapedItemStatus();

    void SetStatus(ItemStatus iStatus, TapedItemStatus tapedStatus);

    Tween CurrentTween();
    void SetCurrentTween(Tween tw);
}
