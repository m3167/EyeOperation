using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using UnityEngine.Video;

public class ShareManager : MonoBehaviour
{
    [SerializeField] GameObject sharedPdf, sharedWeb, sharedVideos;
    [SerializeField] Animation screenHolder;
    [SerializeField] AudioSource HolderEffect;
    [SerializeField] public bool isScreenOpened;

    //public float timeToShare;
    PhotonView _PhotonView;

    public static ShareManager instance;

    void Start()
    {
        instance = this;
        _PhotonView = PhotonView.Get(this);
    }

    #region Public Methods

    public void OnSharePDFClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _PhotonView.RPC("SharePDFFromTablet", RpcTarget.AllBuffered);
        }
    }

    public void OnShareWebClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _PhotonView.RPC("ShareWebFromTablet", RpcTarget.AllBuffered);
        }
    }

    public void OnShareVideoClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _PhotonView.RPC("ShareVideoFromTablet", RpcTarget.AllBuffered);
        }
    }

    public void OnCloseScreen()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _PhotonView.RPC("CloseScreenInEnvironment", RpcTarget.AllBuffered);
        }
    }

    #endregion

    #region RPC Methods

    [PunRPC]
    public void SharePDFFromTablet()
    {
        if (isScreenOpened == false)
        {
            screenHolder [screenHolder.clip.name].speed = 1;
            //screenHolder [screenHolder.clip.name].time = screenHolder [screenHolder.clip.name].length;
            screenHolder.Play (screenHolder.clip.name);
            HolderEffect.Play();
            Invoke("SharePdf", screenHolder.clip.length);
            isScreenOpened = true;
        }
        else
        {
            SharePdf();
        }
    }

    void SharePdf()
    {
        sharedWeb.SetActive(false);
        sharedVideos.SetActive(false);
        sharedPdf.SetActive(true);
    }

    [PunRPC]
    public void ShareWebFromTablet()
    {
        if (isScreenOpened == false)
        {
            screenHolder [screenHolder.clip.name].speed = 1;
            //screenHolder [screenHolder.clip.name].time = screenHolder [screenHolder.clip.name].length;
            screenHolder.Play (screenHolder.clip.name);
            HolderEffect.Play();
            Invoke("ShareWeb", screenHolder.clip.length);
            isScreenOpened = true;
        }
        else
        {
            ShareWeb();
        }
    }

    void ShareWeb()
    {
        sharedPdf.SetActive(false);
        sharedVideos.SetActive(false);
        sharedWeb.SetActive(true);
    }

    [PunRPC]
    public void ShareVideoFromTablet()
    {
        if (isScreenOpened == false)
        {
            screenHolder [screenHolder.clip.name].speed = 1;
            //screenHolder [screenHolder.clip.name].time = screenHolder [screenHolder.clip.name].length;
            screenHolder.Play (screenHolder.clip.name);
            HolderEffect.Play();
            StartCoroutine(AssingVideo.PlayVideo(screenHolder.clip.length));
            isScreenOpened = true;
        }
        else
        {
            StartCoroutine(AssingVideo.PlayVideo(0f));
        }
    }

    [PunRPC]
    public void CloseScreenInEnvironment()
    {
        if (isScreenOpened == true)
        {
            screenHolder [screenHolder.clip.name].speed = -1;
            screenHolder [screenHolder.clip.name].time = screenHolder [screenHolder.clip.name].length;
            screenHolder.Play (screenHolder.clip.name);
            HolderEffect.Play();
            sharedPdf.SetActive(false);
            sharedVideos.SetActive(false);
            sharedWeb.SetActive(false);
            isScreenOpened = false;
        }
    }

    #endregion

    public class AssingVideo : MonoBehaviour
    {
        public static IEnumerator PlayVideo(float time)
        {
            yield return new WaitForSeconds(time);
            ShareManager.instance.sharedPdf.SetActive(false);
            ShareManager.instance.sharedWeb.SetActive(false);
            ShareManager.instance.sharedVideos.SetActive(true);
        }
    }
}