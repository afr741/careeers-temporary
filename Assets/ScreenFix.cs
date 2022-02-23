using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFix : MonoBehaviour {

    // Use this for initialization

    public int targetDisplay = 1;
    public int resolutionWidth = 1920;
    public int resolutionHeight = 1080;
    public float delay = 0f;

    void Start () {

        Invoke("run",delay); 

    }

    private void run() {
        StartCoroutine(TargetDisplayHack());
    }


    private IEnumerator TargetDisplayHack()
    {
       // Debug.LogError("Setting Resolution");

        // Get the current screen resolution.
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        // Set the target display and a low resolution.
        


        Screen.SetResolution(600, 800, Screen.fullScreen);

     
        // - MH  one frame doesn't seem to be enough sometimes -
        Invoke("resolveResolution", 1f);
        // Wait a frame.
        yield return null;

        // Restore resolution.
       // Screen.SetResolution(screenWidth, screenHeight, Screen.fullScreen);
       

        //Application.targetFrameRate = 100;
       // QualitySettings.vSyncCount = 0;
    }

    private void resolveResolution()
    {

        int targetWidth = resolutionWidth; //CONFIG
        int targetHeight = resolutionHeight; // CONFIG

        Screen.SetResolution(targetWidth, targetHeight, Screen.fullScreen);




    }
}
