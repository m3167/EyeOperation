using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DestroyModelNetworked : MonoBehaviourPun
{
    public PhotonView _photonView;

    public static DestroyModelNetworked instance;

    public void Start()
    {
        instance = this;
        _photonView = PhotonView.Get(this);
    }

    public void OnDestroyModelClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(this.gameObject);
            //_photonView.RPC("Rpc_DestroyModel", RpcTarget.AllBuffered);
        }
    }

    public void OnHideModelClicked()
    {
        _photonView.RPC("Rpc_HideModel", RpcTarget.AllBuffered);
    }

    public void OnHideAllModelClicked()
    {
        _photonView.RPC("Rpc_HideAllModelWithTag", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Rpc_DestroyModel()
    {
        Destroy(this.gameObject);
    }

    [PunRPC]
    public void Rpc_HideModel()
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void Rpc_HideAllModelWithTag()
    {
        GameObject[] objs;
        objs = GameObject.FindGameObjectsWithTag("Tablet");
        foreach (GameObject Tablets in objs)
        {
            Tablets.SetActive(false);
        }
    }
}