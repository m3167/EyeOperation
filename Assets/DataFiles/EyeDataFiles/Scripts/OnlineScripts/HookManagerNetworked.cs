using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Photon.Pun;
using UnityEngine.Serialization;

public class HookManagerNetworked : MonoBehaviourPun
{
    public EyeSurgeryManagerNetworked eyeOperation;
    public GameObject rightHand, leftHand;
    public Grabber rightControllerGrabber, leftControllerGrabber;
    public GameObject rHookPosAndRot,lHookPosAndRot;
    public Grabbable HookGrabbable;
    public Transform rightHandRemote, leftHandRemote;
    public GameObject arrowGuid;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "PlaceNewLens")
        {
            photonView.RPC("Rpc_AdjustNewLens", RpcTarget.AllBuffered);
        }
        
        if (coll.tag == "WrongCollider")
        {
            photonView.RPC("Rpc_CollideWrongLayer", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_AdjustNewLens()
    {
        Debug.LogError("Hook collide new lens");
        eyeOperation.rightEffect.Play();
        eyeOperation.step6.SetActive(false);
        eyeOperation.placeNewLensCollider.SetActive(false);
        eyeOperation.newLens.Play();
    }
    
    [PunRPC]
    private void Rpc_CollideWrongLayer()
    {
        Debug.LogError("Hook collide wrong layers or outside eye area");
        eyeOperation.wrongEffect.Play();
    }

    public void OnHoldHook()
    {
        if (rightControllerGrabber.HeldGrabbable == HookGrabbable)
        {
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Tool Holding Idle");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Tool Holding");
            photonView.RPC("Rpc_RightHoldHook", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == HookGrabbable)
        {
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Tool Holding Idle");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Tool Holding");
            photonView.RPC("Rpc_LeftHoldHook", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_RightHoldHook()
    {
        arrowGuid.SetActive(false);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rRightHand = RemotePlayer.transform.GetChild(2);
            rightHandRemote = getChildTransformByName(rRightHand, "CustomHandRightBlackNew");
            rightHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
                Resources.Load<HandPose>("Tool Holding");
            rightHandRemote.transform.localPosition = rHookPosAndRot.transform.localPosition;
            rightHandRemote.transform.localRotation = rHookPosAndRot.transform.localRotation;
        }
    }

    [PunRPC]
    private void Rpc_LeftHoldHook()
    {
        arrowGuid.SetActive(false);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rLeftHand = RemotePlayer.transform.GetChild(1);
            leftHandRemote = getChildTransformByName(rLeftHand, "CustomHandLeftBlackNew");
            leftHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
                Resources.Load<HandPose>("Tool Holding");
            leftHandRemote.transform.localPosition = lHookPosAndRot.transform.localPosition;
            leftHandRemote.transform.localRotation = lHookPosAndRot.transform.localRotation;
        }
    }

    public void OnUnHoldHook()
    {
        if (rightControllerGrabber.HeldGrabbable == null)
        {
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            photonView.RPC("Rpc_RightUnHoldHook", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == null)
        {
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            photonView.RPC("Rpc_LeftUnHoldHook", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_RightUnHoldHook()
    {
        arrowGuid.SetActive(true);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rRightHand = RemotePlayer.transform.GetChild(2);
            rightHandRemote = getChildTransformByName(rRightHand, "CustomHandRightBlackNew");
            rightHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose = Resources.Load<HandPose>("Open");
        }
    }

    [PunRPC]
    private void Rpc_LeftUnHoldHook()
    {
        arrowGuid.SetActive(true);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rLeftHand = RemotePlayer.transform.GetChild(1);
            leftHandRemote = getChildTransformByName(rLeftHand, "CustomHandLeftBlackNew");
            leftHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose = Resources.Load<HandPose>("Open");
        }
    }

    Transform getChildTransformByName(Transform search, string name)
    {
        Transform[] children = search.GetComponentsInChildren<Transform>();
        for (int x = 0; x < children.Length; x++)
        {
            Transform child = children[x];
            if (child.name == name)
            {
                return child;
            }
        }

        return null;
    }
}

