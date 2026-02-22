using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using Photon.Pun;


public class MoveToChairManager : MonoBehaviourPun
{
    [SerializeField] private GameObject player, target;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private Quaternion originalRotation;

    public PhotonView _PhotonView;
    private void Start()
    {
        _PhotonView = PhotonView.Get(this);
        player = GameObject.Find("XR Rig Advanced");
        originalPosition = player.transform.localPosition;
        originalRotation = player.transform.localRotation;
    }

    private void Update()
    {
        if (target.transform.childCount == 1)
        {
            if (InputBridge.Instance.AButtonDown || Input.GetKeyDown(KeyCode.Escape))
            {
                UnSittingChair();
                _PhotonView.RPC("RPC_EnableChairCollider",RpcTarget.AllBuffered,target.name);
            }
        }
    }

    public void SittingChair()
    {
        if (target.transform.childCount == 0)
        {
            player.transform.parent = target.transform;
            if (Application.platform == RuntimePlatform.Android)
            {
                player.transform.localPosition = new Vector3(0f, 0.5f, -0.1f);
                player.transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
                player.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                player.transform.localPosition = new Vector3(0f, 0f, -0.1f);
                player.transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
                player.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            player.transform.GetChild(0).localPosition = Vector3.zero;
            player.transform.GetChild(0).localRotation = quaternion.identity;
            player.transform.GetChild(0).localScale = Vector3.one;
            player.transform.GetChild(0).GetComponent<CharacterController>().enabled = false;
        }
    }

    public void UnSittingChair()
    {
        player.transform.parent = null;
        player.transform.localPosition = originalPosition;
        player.transform.localRotation = originalRotation;
        player.transform.GetChild(0).localPosition = Vector3.zero;
        player.transform.GetChild(0).localRotation = quaternion.identity;
        player.transform.GetChild(0).localScale = Vector3.one;
        player.transform.GetChild(0).GetComponent<CharacterController>().enabled = true;
    }

    public void OnSelectChair(GameObject chairObj)
    {
        _PhotonView.RPC("RPC_EnableChairCollider", RpcTarget.AllBuffered, target.name);
        target = chairObj;
        SittingChair();
    }
    
    public void OnDisableChairCollider()
    {
       _PhotonView.RPC("RPC_DisableChairCollider",RpcTarget.AllBuffered,target.name);
    }

    [PunRPC]
    public void RPC_DisableChairCollider(string chairName)
    {
        GameObject chair = GameObject.Find(chairName);
        chair.GetComponent<MeshCollider>().enabled = false;
    }
    
    [PunRPC]
    public void RPC_EnableChairCollider(string chairName)
    {
        GameObject chair = GameObject.Find(chairName);
        chair.GetComponent<MeshCollider>().enabled = true;
    }
}