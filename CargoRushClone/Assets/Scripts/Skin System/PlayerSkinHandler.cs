using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinHandler : MonoBehaviour
{ 
    [SerializeField] private GameObject defaultSkin;

    [SerializeField] private List<GameObject> skinList;

    private GameObject _currentSkin;

    protected void Awake()
    {
        _currentSkin = defaultSkin;
    }

    private void OnEnable()
    {
        Events.OnPlayerSkinChange += ChangeSkinBody;
    }

    private void OnDisable()
    {
        Events.OnPlayerSkinChange -= ChangeSkinBody;
    }


    private void ChangeSkinBody(int skinIndex)
    {
        _currentSkin.SetActive(false);
        _currentSkin = skinList[skinIndex];
        _currentSkin.SetActive(true);
    }
 
}
