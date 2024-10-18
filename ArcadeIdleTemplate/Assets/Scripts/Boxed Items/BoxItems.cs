using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxItems : MonoBehaviour, IItem
{
    [SerializeField] private ItemStatus itemStatus;
    [SerializeField] private TapedItemStatus tapedItemStatus;

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
}