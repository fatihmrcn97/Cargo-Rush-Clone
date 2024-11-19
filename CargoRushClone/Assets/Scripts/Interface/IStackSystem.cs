
using UnityEngine;

public interface IStackSystem 
{
    void DropPointHandle();

    void SetTheStackPositonBack(int stackMaterialCount);

    Transform MaterialDropPositon();
}
