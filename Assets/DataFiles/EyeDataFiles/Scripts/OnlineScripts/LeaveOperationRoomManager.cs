using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveOperationRoomManager : MonoBehaviourPun
{
    public void LeaveRoomTOSavedScene()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
           
                PhotonNetwork.LoadLevel(SaveLastSceneManager.lastloadedsceneIndex);
        }
        else
        {
          
                VirtualWorldManager.instance.LeaveRoomAndLoadHomeScene();
        }
    }
}