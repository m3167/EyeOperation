using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class NetworkedPlayAndPauseAudioDataManager : MonoBehaviourPun
{
    public Button button;
    public Sprite playSprite;
    public Sprite pauseSprite;
    public AudioSource dataAudio;
    public AudioSource tapEffect;
    public void Start()
    {
        button.onClick.AddListener(PlayAndPauseAudioData);
    }

    private void Update()
    {
        if (!dataAudio.isPlaying)
        {
            button.GetComponent<Image>().sprite = playSprite;
        }
        
        if (dataAudio.isPlaying)
        {
            button.GetComponent<Image>().sprite = pauseSprite;
        }
    }

    public void PlayAndPauseAudioData()
    {
        photonView.RPC("Rpc_PlayAndPauseAudioData",RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Rpc_PlayAndPauseAudioData()
    {
        if (button.GetComponent<Image>().sprite == pauseSprite)
        {
            tapEffect.Play();
            dataAudio.Pause();
            button.GetComponent<Image>().sprite = playSprite;
        }
        else
        {
            tapEffect.Play();
            dataAudio.Play();
            button.GetComponent<Image>().sprite = pauseSprite;
        }
    }
}