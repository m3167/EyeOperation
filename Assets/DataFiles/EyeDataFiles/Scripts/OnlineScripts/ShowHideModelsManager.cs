using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ShowHideModelsManager : MonoBehaviourPun
{
    [Header("AudioSnapped")] public AudioSource snappedEffect;

    #region Public Methods

    public void OnHideObjectWhenSnappedClicked(string objectName)
    {
        photonView.RPC("HideObjectWhenSnapped", RpcTarget.AllBuffered, objectName);
    }

    public void OnShowObjectWhenSnappedClicked(string objectName)
    {
        photonView.RPC("ShowObjectWhenSnapped", RpcTarget.AllBuffered, objectName);
    }

    #endregion

    #region RPC Methods

    [PunRPC]
    public void ShowObjectWhenSnapped(string objectName)
    {
        GameObject objectToShow = GameObject.Find(objectName);
        objectToShow.transform.GetChild(0).gameObject.SetActive(true);
        snappedEffect.Play();
    }

    [PunRPC]
    public void HideObjectWhenSnapped(string objectName)
    {
        GameObject objectToHide = GameObject.Find(objectName);
        objectToHide.SetActive(false);
    }

    #endregion
}