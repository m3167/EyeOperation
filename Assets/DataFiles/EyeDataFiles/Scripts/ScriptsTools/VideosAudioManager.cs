using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideosAudioManager : MonoBehaviour
{
    public Sprite mute, unMute;
    public Image sourceImg;

    public void Mute_unMuteAudioOfVideo(AudioSource soundVideo)
    {
        if (soundVideo.mute == true)
        {
            soundVideo.mute = false;
            sourceImg.sprite = mute;
        }
        else
        {
            soundVideo.mute = true;
            sourceImg.sprite = unMute;
        }
    }
}