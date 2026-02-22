using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class PlayerTabletNotMaster : MonoBehaviour
{
    public Sprite startRecordingSprite, stopRecordingSprite;
    public Button recordingBtn, exitBtn;
    public AudioSource screenShotEffect;


    public void ExitRoomButton()
    {
        VirtualWorldManager.instance.LeaveRoomAndLoadHomeScene();
    }

    /*public void Recording()
    {
        if (recordingBtn.GetComponent<Image>().sprite == startRecordingSprite)
        {
            GameObject.Find("Screen Recording Manager").GetComponent<ReplayCam>().StartRecording();
            recordingBtn.GetComponent<Image>().sprite = stopRecordingSprite;
        }
        else
        {
            GameObject.Find("Screen Recording Manager").GetComponent<ReplayCam>().StopRecording();
            recordingBtn.GetComponent<Image>().sprite = startRecordingSprite;
        }
    }*/


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
    }
}