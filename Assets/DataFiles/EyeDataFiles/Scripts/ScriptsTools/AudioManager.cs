using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void PlayAudio(AudioSource source)
    {
        source.Play();
    }
}
