using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Photon.Pun;

public class PumpingLiquidNetworked : MonoBehaviourPun
{
    public GameObject pumpObj, posOfPumpObj, waterDropPrefab,jelPlaceHighlight;
    public AudioSource pumpingSoundEffect;
    public bool isHoldSyringe;
    public GameObject rightHand ,leftHand;
    public Grabber rightControllerGrabber, leftControllerGrabber;
    public GameObject rSyringePosAndRot, lSyringePosAndRot;
    public Grabbable scalpelGrabbable, tweezerGrabbable, syrgineGrabbable;
    public Transform rightHandRemote, leftHandRemote;
    
    public PhotonView _photonView;

    private void Start()
    {
        _photonView = PhotonView.Get(this);
    }

    private void Update()
    {
        if (InputBridge.Instance.AButton || Input.GetKeyDown(KeyCode.Z))
        {
            if (isHoldSyringe == true)
            {
                PumpingWaterDrop();
            }
        }
    }
    
    
    public void PumpingWaterDrop()
    {
        Debug.Log("Pumping water drops");
        pumpingSoundEffect.Play();
        GameObject waterDropObject = PhotonNetwork.Instantiate(waterDropPrefab.name,Vector3.zero,Quaternion.identity);
        waterDropObject.transform.position = posOfPumpObj.transform.position;
        waterDropObject.transform.forward = pumpObj.transform.forward;
    }

    public void HoldingSyringe()
    {
        if (rightControllerGrabber.HeldGrabbable == syrgineGrabbable)
        {
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Scalpel Holding Idle");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Scalpel Holding");
            _photonView.RPC("Rpc_RightHoldSyringe", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == syrgineGrabbable)
        {
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Scalpel Holding Idle");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Scalpel Holding");
            _photonView.RPC("Rpc_LeftHoldSyringe", RpcTarget.AllBuffered);
        }
    }
    
    [PunRPC]
    public void Rpc_RightHoldSyringe()
    {
        isHoldSyringe = true;
        jelPlaceHighlight.SetActive(true);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        Transform rRightHand = RemotePlayer.transform.GetChild(2);
        rightHandRemote = getChildTransformByName(rRightHand, "CustomHandRightBlackNew");
        rightHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
            Resources.Load<HandPose>("Scalpel Holding");
        rightHandRemote.transform.localPosition = rSyringePosAndRot.transform.localPosition;
        rightHandRemote.transform.localRotation = rSyringePosAndRot.transform.localRotation;
    }

    [PunRPC]
    public void Rpc_LeftHoldSyringe()
    {
        isHoldSyringe = true;
        jelPlaceHighlight.SetActive(true);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        Transform rLeftHand = RemotePlayer.transform.GetChild(1);
        leftHandRemote = getChildTransformByName(rLeftHand, "CustomHandLeftBlackNew");
        leftHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
            Resources.Load<HandPose>("Scalpel Holding");
        leftHandRemote.transform.localPosition = lSyringePosAndRot.transform.localPosition;
        leftHandRemote.transform.localRotation = lSyringePosAndRot.transform.localRotation;
    }

    public void UnHoldingSyringe()
    {
        if (rightControllerGrabber.HeldGrabbable == null)
        {
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            _photonView.RPC("Rpc_RightUnHoldSyringe", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == null)
        {
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            _photonView.RPC("Rpc_LeftUnHoldSyringe", RpcTarget.AllBuffered);
        }
    }
    
    [PunRPC]
    public void Rpc_RightUnHoldSyringe()
    {
        isHoldSyringe = false;
        jelPlaceHighlight.SetActive(false);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        Transform rRightHand = RemotePlayer.transform.GetChild(2);
        rightHandRemote = getChildTransformByName(rRightHand, "CustomHandRightBlackNew");
        rightHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose = Resources.Load<HandPose>("Open");
    }

    [PunRPC]
    public void Rpc_LeftUnHoldSyringe()
    {
        isHoldSyringe = false;
        jelPlaceHighlight.SetActive(false);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        Transform rLeftHand = RemotePlayer.transform.GetChild(1);
        leftHandRemote = getChildTransformByName(rLeftHand, "CustomHandLeftBlackNew");
        leftHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose = Resources.Load<HandPose>("Open");
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
