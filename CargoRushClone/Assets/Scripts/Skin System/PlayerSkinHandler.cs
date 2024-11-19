using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinHandler : SingletonMonoBehaviour<PlayerSkinHandler>
{ 
    [SerializeField] private GameObject defaultSkin;

    [SerializeField] private List<GameObject> skinList;

    private GameObject _currentSkin;

    private void Start()
    {
        _currentSkin = defaultSkin;
    }

    public void ChangeSkinBody(int skinIndex)
    {
        _currentSkin.SetActive(false);
        _currentSkin = skinList[skinIndex];
        _currentSkin.SetActive(true);
    }
}
