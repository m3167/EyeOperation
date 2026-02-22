using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class VirtualWorldManager : MonoBehaviourPunCallbacks
{
    public static VirtualWorldManager instance;
    private void Awake()
    {
        instance = this;
    }

    //Player decide to leave room
    [ContextMenu("LeaveRoom")]
    public void LeaveRoomAndLoadHomeScene()
    {
        PhotonNetwork.LeaveRoom();
    }

    #region Photon Callback Methods

    //Call if New Player joined room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "Joined To Room " + "and now " + "Players count equal " +
                  PhotonNetwork.CurrentRoom.PlayerCount);
    }

    //Call if Player left room
    public override void OnLeftRoom()
    {
        Debug.Log("Player left the room");
        PhotonNetwork.Disconnect(); //disconnect from photon server make things clear
    }

    //Call if player disconnected from photon server
    public override void OnDisconnected(DisconnectCause cause)
    {
        NetworkStudentManager.isStudentLeaveRoom = true;
        if (Application.platform == RuntimePlatform.WindowsEditor||
            Application.platform == RuntimePlatform.WindowsPlayer)
        {
            PhotonNetwork.LoadLevel("RegisterScene");
        }
        else
        {
            PhotonNetwork.LoadLevel("RegisterScene");
        }
    }

    #endregion
}