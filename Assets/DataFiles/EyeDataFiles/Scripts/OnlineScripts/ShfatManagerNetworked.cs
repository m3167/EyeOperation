using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Photon.Pun;
using UnityEngine.Serialization;

public class ShfatManagerNetworked : MonoBehaviourPun
{
    public EyeSurgeryManagerNetworked eyeOperation;
    public GameObject rightHand, leftHand;
    public Grabber rightControllerGrabber, leftControllerGrabber;
    public GameObject rShfatPosAndRot, lShfatPosAndRot;
    public Grabbable ShfatGrabbable;
    public Transform rightHandRemote, leftHandRemote;
    public GameObject arrowGuid;
    public AudioSource shafatAudio;
    public bool isHoldShfat, isCollideWhiteWaterPlace;

    public ControllerHand rightControllerHand;

    private void Update()
    {
        if (InputBridge.Instance.AButtonDown || Input.GetKeyDown(KeyCode.Z))
        {
            if (isHoldShfat == true && isCollideWhiteWaterPlace == true)
            {
                InputBridge.Instance.VibrateController(0.3f, 0.1f, 0.1f, rightControllerHand);
                photonView.RPC("RPC_PlayShfat", RpcTarget.AllBuffered);
            }
        }

        if (InputBridge.Instance.AButtonUp || Input.GetKeyUp(KeyCode.Z))
        {
            InputBridge.Instance.VibrateController(0f, 0f, 0f, rightControllerHand);
            photonView.RPC("RPC_StopShfat", RpcTarget.AllBuffered);
        }

        if (eyeOperation.whiteWaterParticle.GetComponent<Animator>().enabled == true && eyeOperation.whiteWaterParticle
                .GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 &&
            !eyeOperation.whiteWaterParticle.GetComponent<Animator>().IsInTransition(0))
        {
            eyeOperation.whiteWaterParticle.GetComponent<Animator>().enabled = false;
            eyeOperation.step4.SetActive(false);
            eyeOperation.step5.SetActive(true);
            eyeOperation.rightEffect.Play();
            shafatAudio.Stop();
            isCollideWhiteWaterPlace = false;
            eyeOperation.whiteWaterParticle.SetActive(false);
            eyeOperation.placeInjectionLensCollider.SetActive(true);
        }
    }

    [PunRPC]
    private void RPC_PlayShfat()
    {
        shafatAudio.Play();
        eyeOperation.whiteWaterParticle.GetComponent<Animator>().enabled = true;
        eyeOperation.whiteWaterParticle.GetComponent<Animator>().speed = 1;
    }

    [PunRPC]
    private void RPC_StopShfat()
    {
        shafatAudio.Stop();
        eyeOperation.whiteWaterParticle.GetComponent<Animator>().speed = 0;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "WhiteWaterCollider")
        {
            photonView.RPC("Rpc_CollideWithWhiteWater", RpcTarget.AllBuffered);
        }

        if (coll.tag == "WrongCollider")
        {
            photonView.RPC("Rpc_CollideWrongLayer", RpcTarget.AllBuffered);
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "WhiteWaterCollider")
        {
            photonView.RPC("Rpc_NotCollideWithWhiteWater", RpcTarget.AllBuffered);
        }
    }


    [PunRPC]
    private void Rpc_CollideWithWhiteWater()
    {
        Debug.LogError("Shfat collide with white water place");
        isCollideWhiteWaterPlace = true;
    }

    [PunRPC]
    private void Rpc_NotCollideWithWhiteWater()
    {
        Debug.LogError("Shfat not collide with white water place");
        isCollideWhiteWaterPlace = false;
    }

    [PunRPC]
    private void Rpc_CollideWrongLayer()
    {
        Debug.LogError("Shfat collide wrong layers or outside eye area");
        eyeOperation.wrongEffect.Play();
    }

    public void OnHoldShfat()
    {
        if (rightControllerGrabber.HeldGrabbable == ShfatGrabbable)
        {
            isHoldShfat = true;
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Tool Holding Idle");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Tool Holding");
            photonView.RPC("Rpc_RightHoldShfat", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == ShfatGrabbable)
        {
            isHoldShfat = true;
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Tool Holding Idle");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Tool Holding");
            photonView.RPC("Rpc_LeftHoldShfat", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_RightHoldShfat()
    {
        arrowGuid.SetActive(false);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rRightHand = RemotePlayer.transform.GetChild(2);
            rightHandRemote = getChildTransformByName(rRightHand, "CustomHandRightBlackNew");
            rightHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
                Resources.Load<HandPose>("Tool Holding");
            rightHandRemote.transform.localPosition = rShfatPosAndRot.transform.localPosition;
            rightHandRemote.transform.localRotation = rShfatPosAndRot.transform.localRotation;
        }
    }

    [PunRPC]
    private void Rpc_LeftHoldShfat()
    {
        arrowGuid.SetActive(false);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rLeftHand = RemotePlayer.transform.GetChild(1);
            leftHandRemote = getChildTransformByName(rLeftHand, "CustomHandLeftBlackNew");
            leftHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
                Resources.Load<HandPose>("Tool Holding");
            leftHandRemote.transform.localPosition = lShfatPosAndRot.transform.localPosition;
            leftHandRemote.transform.localRotation = lShfatPosAndRot.transform.localRotation;
        }
    }

    public void OnUnHoldShfat()
    {
        if (rightControllerGrabber.HeldGrabbable == null)
        {
            isHoldShfat = false;
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            photonView.RPC("Rpc_RightUnHoldShfat", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == null)
        {
            isHoldShfat = false;
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            photonView.RPC("Rpc_LeftUnHoldShfat", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_RightUnHoldShfat()
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
    private void Rpc_LeftUnHoldShfat()
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