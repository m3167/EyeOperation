using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ResetModelTweenNetworked : MonoBehaviourPun
{
    private Vector3 originalPos;
    private Quaternion originalRot;
    public AudioSource resetEffect;

    #region Public Methods

    public void OnResetPositionRotationClicked()
    {
        photonView.RPC("RPCResetPosition_Rotation",RpcTarget.AllBuffered); 
    }
    
    public void TakePositionsAndRotations()
    {
        photonView.RPC("RPCTakePositionsAndRotations",RpcTarget.AllBuffered); 
    }

    #endregion
    
    #region RPC Methods

    [PunRPC]
    public void RPCResetPosition_Rotation()
    {
        resetEffect.Play();
        LeanTween.move(gameObject, originalPos, 2f).setEase(LeanTweenType.linear);
        gameObject.transform.rotation = originalRot;
    }

    [PunRPC]
    public void RPCTakePositionsAndRotations()
    {
        originalPos = gameObject.transform.position;
        originalRot = gameObject.transform.rotation;
    }

    #endregion
}
