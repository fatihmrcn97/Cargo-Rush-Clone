using System.Collections.Generic;
using UnityEngine;

public interface IItemList
{
    List<GameObject> StackedMaterialList();
    List<Transform> StackTransforms();
  
    void StackPositionHandler();

    bool CheckPlayerHandMax();

    bool IsInTrigger { get; set; }

    float StackMovementSpeed();
    
    float StackGetGiveDelaySpeed();
}