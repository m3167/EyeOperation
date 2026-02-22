using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotManager : MonoBehaviour
{
    [SerializeField] public AudioSource screenShotEffect;
    [SerializeField] public GameObject screenShotMessage;
    
    public void ScreenShot()
    {
        StartCoroutine(TakeScreenShot());
    }
    
    IEnumerator TakeScreenShot()
    {
        yield return new WaitForEndOfFrame();
        print("A screenshot was taken!");
        ScreenCapture.CaptureScreenshot("screenshot " + System.DateTime.Now.ToString("MM-dd-yy (HH-mm-ss)") + ".png");
        screenShotEffect.Play();
        screenShotMessage.SetActive(true);
        yield return new WaitForSeconds(1f);
        screenShotMessage.SetActive(false);
    }
}