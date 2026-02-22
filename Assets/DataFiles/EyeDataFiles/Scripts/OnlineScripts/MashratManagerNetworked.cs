using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Photon.Pun;
using UnityEngine.Serialization;

public class MashratManagerNetworked : MonoBehaviourPun
{
    public EyeSurgeryManagerNetworked eyeOperation;
    public GameObject rightHand, leftHand;
    public Grabber rightControllerGrabber, leftControllerGrabber;
    public GameObject rMashratPosAndRot, lMashratPosAndRot;
    public Grabbable MashratGrabbable;
    public Transform rightHandRemote, leftHandRemote;
    public GameObject arrowGuid;
    public NetworkedGrabbable eyeGrabbable;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "RightCorneaCutting")
        {
            photonView.RPC("Rpc_CollideRightCorneaCutting", RpcTarget.AllBuffered);
        }

        if (coll.tag == "LeftCorneaCutting")
        {
            photonView.RPC("Rpc_CollideLeftCorneaCutting", RpcTarget.AllBuffered);
        }

        if (coll.tag == "WrongCollider")
        {
            photonView.RPC("Rpc_CollideWrongLayer", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_CollideRightCorneaCutting()
    {
        Debug.LogError("Mashrat collide right cornea cutting");
        eyeOperation.rightEffect.Play();
        eyeOperation.rightCorneaCutting.SetActive(false);
        eyeOperation.leftCorneaCutting.SetActive(true);
        eyeOperation.corneaMid.GetComponent<Animation>().Play("MidCorneaNewRight");
        eyeOperation.corneaLiquid.GetComponent<Animation>().Play("CorneaLiquidNewRight");
    }

    [PunRPC]
    private void Rpc_CollideLeftCorneaCutting()
    {
        Debug.LogError("Mashrat collide left cornea cutting");
        eyeOperation.rightEffect.Play();
        eyeOperation.leftCorneaCutting.SetActive(false);
        eyeOperation.step1.SetActive(false);
        eyeOperation.step2.SetActive(true);
        eyeOperation.corneaMid.GetComponent<Animation>().Play("MidCorneaNewLeft");
        eyeOperation.corneaLiquid.GetComponent<Animation>().Play("CorneaLiquidNewLeft");
        eyeOperation.placeLiquidCollider.SetActive(true);
    }

    [PunRPC]
    private void Rpc_CollideWrongLayer()
    {
        Debug.LogError("Mashrat collide wrong layers or outside eye area");
        eyeOperation.wrongEffect.Play();
    }

    public void OnHoldMashrat()
    {
        if (rightControllerGrabber.HeldGrabbable == MashratGrabbable)
        {
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Tool Holding Idle");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Tool Holding");
            photonView.RPC("Rpc_RightHoldMashrat", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == MashratGrabbable)
        {
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Tool Holding Idle");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Tool Holding");
            photonView.RPC("Rpc_LeftHoldMashrat", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_RightHoldMashrat()
    {
        arrowGuid.SetActive(false);
        eyeGrabbable.enabled = false;
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rRightHand = RemotePlayer.transform.GetChild(2);
            rightHandRemote = getChildTransformByName(rRightHand, "CustomHandRightBlackNew");
            rightHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
                Resources.Load<HandPose>("Tool Holding");
            rightHandRemote.transform.localPosition = rMashratPosAndRot.transform.localPosition;
            rightHandRemote.transform.localRotation = rMashratPosAndRot.transform.localRotation;
        }
    }

    [PunRPC]
    private void Rpc_LeftHoldMashrat()
    {
        arrowGuid.SetActive(false);
        eyeGrabbable.enabled = false;
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rLeftHand = RemotePlayer.transform.GetChild(1);
            leftHandRemote = getChildTransformByName(rLeftHand, "CustomHandLeftBlackNew");
            leftHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
                Resources.Load<HandPose>("Tool Holding");
            leftHandRemote.transform.localPosition = lMashratPosAndRot.transform.localPosition;
            leftHandRemote.transform.localRotation = lMashratPosAndRot.transform.localRotation;
        }
    }

    public void OnUnHoldMashrat()
    {
        if (rightControllerGrabber.HeldGrabbable == null)
        {
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            photonView.RPC("Rpc_RightUnHoldMashrat", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == null)
        {
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            photonView.RPC("Rpc_LeftUnHoldMashrat", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_RightUnHoldMashrat()
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
    private void Rpc_LeftUnHoldMashrat()
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