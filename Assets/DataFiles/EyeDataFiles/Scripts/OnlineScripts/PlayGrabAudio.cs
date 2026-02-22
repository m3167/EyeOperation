using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class PlayGrabAudio : MonoBehaviour
{
    public PhotonView _photonView;
    // Start is called before the first frame update
    void Start()
    {
        _photonView = PhotonView.Get(this);
    }

    public void OnGrabModel()
    {
        _photonView.RPC("PlayGrabAudioLocalized",RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void PlayGrabAudioLocalized()
    {
        gameObject.GetComponent<LocalizeAudioClipEvent>().enabled = true;
    }
    
    public void OnReleaseModel()
    {
        _photonView.RPC("StopGrabAudioLocalized",RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void StopGrabAudioLocalized()
    {
        gameObject.GetComponent<LocalizeAudioClipEvent>().enabled = false;
    }


}
