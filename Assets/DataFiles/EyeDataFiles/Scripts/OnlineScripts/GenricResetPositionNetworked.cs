using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Photon.Pun;

public class GenricResetPositionNetworked : MonoBehaviourPun
{
    public GameObject[] GameObjectParts;
    private Vector3[] initialpos;
    private Quaternion[] initialRot;
    public AudioSource resetEffect;

    public PhotonView _photonView;
    
    public static GenricResetPositionNetworked instance;

    private void Awake()
    {
        instance = this;
        _photonView = PhotonView.Get(this);
    }
    
    public void TakePositionsAndRotations()
    {
        _photonView.RPC("Rpc_TakePositionsAndRotations",RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Rpc_TakePositionsAndRotations()
    {
        initialpos = new Vector3[GameObjectParts.Length];
        initialRot = new Quaternion[GameObjectParts.Length];
        for (int i = 0; i < GameObjectParts.Length; i++)
        {
            initialpos[i] = GameObjectParts[i].transform.position;
            initialRot[i] = GameObjectParts[i].transform.rotation;
        }
    }

    public void ResetGameObject()
    {
        _photonView.RPC("Rpc_ResetGameObject",RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Rpc_ResetGameObject()
    {
        for (int i = 0; i < GameObjectParts.Length; i++)
        {
            resetEffect.Play();
            LeanTween.move(GameObjectParts[i], initialpos[i], 2f).setEase(LeanTweenType.linear);
            GameObjectParts[i].transform.rotation = initialRot[i];
        }
    }
    
    public void ResetGameObjectWithOutTween()
    {
        _photonView.RPC("Rpc_ResetGameObjectWithOutTween",RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Rpc_ResetGameObjectWithOutTween()
    {
        for (int i = 0; i < GameObjectParts.Length; i++)
        {
            GameObjectParts[i].transform.position = initialpos[i];
            GameObjectParts[i].transform.rotation = initialRot[i];
        }
    }
}
