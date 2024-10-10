using System.Collections.Generic;
using UnityEngine;

public interface IItemList
{
    List<GameObject> StackedMaterialList();
    List<Transform> StackTransforms();

    int MaxStackCount();

    void StackPositionHandler();
}