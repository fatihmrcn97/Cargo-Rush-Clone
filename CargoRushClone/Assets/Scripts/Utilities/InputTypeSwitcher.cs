using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTypeSwitcher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            Events.OnWorldCanvasOpened?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        Events.OnWorldCanvasOpened?.Invoke();
    }
}