using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Global Variables/GlobalVariables")]
public class GlobalVariablesSO : ScriptableObject
{
 
    [Tooltip("Trigger alanina girdikten ne kadar sonra delay sureleri")]
    public float PlayerStackSpeed;
    public float AIStackSpeed;
    [Space]
    [Tooltip("Her bir stack urunun hareket hızı")]
    public float PlayerStackMovementSpeed;
    public float AIStackMovementSpeed;
    [Space][Header("Worker after job wait times")]
    public float AICargoAfterWorkWaitTime;
    public float AICollectableAfterWorkWaitTime;
}
