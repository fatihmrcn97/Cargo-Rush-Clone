using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;

public class BoosterOnMap : MonoBehaviour
{
    [SerializeField] private Slider waitBeforeBuySlider;

    private Coroutine StartBoosterOpen;

    [SerializeField] private BoosterTypes boosterType;
    

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        StartBoosterOpen = StartCoroutine(CheckIfUserStillOn());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return; 
        if (StartBoosterOpen != null)
        {
            StopCoroutine(StartBoosterOpen);
            StartCoroutine(ResetSlider());
        }
    }

    private IEnumerator CheckIfUserStillOn()
    {
        float elapsedTime = 0;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / 1f);
            waitBeforeBuySlider.value = Mathf.Lerp(0, 100, progress);
            yield return null;
        }
        
        OpenBoosterUI();
        Destroy(gameObject,.05f);
    }


    private void OpenBoosterUI()
    {
        BoosterSystem.Instance.OpenBoosterUI(boosterType);
    }
    
    
    
    private IEnumerator ResetSlider()
    {
        float elapsedTime = 0;
        var currentValue = waitBeforeBuySlider.value;
        while (elapsedTime < .2f)
        {
            elapsedTime += Time.deltaTime;
            var progress = Mathf.Clamp01(elapsedTime / .2f);

            waitBeforeBuySlider.value = Mathf.Lerp(currentValue, 0, progress);
            yield return null;
        }
    }

}