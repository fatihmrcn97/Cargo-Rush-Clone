using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine; 

public class FpsCounter : MonoBehaviour
{
 
    public TextMeshProUGUI Text;
 
   
    private int screenLongSide; 

    // for fps calculation.
    private int frameCount;
    private float elapsedTime;
    private double frameRate;
 
    private void Update()
    {
        // FPS calculation
        frameCount++;
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.5f)
        {
            frameRate = System.Math.Round(frameCount / elapsedTime, 1, System.MidpointRounding.AwayFromZero);
            frameCount = 0;
            elapsedTime = 0;

            // Update the UI size if the resolution has changed
            if (screenLongSide != Mathf.Max(Screen.width, Screen.height))
            {
                Text.text = frameRate+"";
            }
        }
    }
 
}