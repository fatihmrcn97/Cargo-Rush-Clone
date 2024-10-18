
public interface IItem
{
    ItemStatus ItemStatus();

    TapedItemStatus TapedItemStatus();

    void SetStatus(ItemStatus iStatus, TapedItemStatus tapedStatus);
}
