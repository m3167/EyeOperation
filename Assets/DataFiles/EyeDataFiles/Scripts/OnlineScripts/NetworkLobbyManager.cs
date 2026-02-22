using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkLobbyManager : MonoBehaviourPun
{
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnChooseEnvironment(string nameOfEnvironment)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Loaded " + nameOfEnvironment + " environment");
            PhotonNetwork.LoadLevel(nameOfEnvironment);
        }
    }
}