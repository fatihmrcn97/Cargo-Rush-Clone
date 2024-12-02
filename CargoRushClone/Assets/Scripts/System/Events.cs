using System;
using UnityEngine;
using UnityEngine.Events;

public static class Events
{ 
    // Sample Events
    //public static Action<Vector3, int> SpawnMoneyEvent;
    //public static Action<string, Vector3, float> SpawnVFXForUIEvent; 
    //public static Action SpecialAnimalStarted;
    public static Action MaterialStackedEvent;
    public static Action<int> OnPlayerSkinChange;
    public static Action<int> OnPlayerSkinChangePreview;

    public static Action OnPackableItemSpawnerStarted; 
}