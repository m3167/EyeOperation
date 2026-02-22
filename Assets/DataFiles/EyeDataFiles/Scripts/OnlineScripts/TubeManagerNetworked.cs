using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Photon.Pun;
using UnityEngine.Serialization;

public class TubeManagerNetworked : MonoBehaviourPun
{
    public EyeSurgeryManagerNetworked eyeOperation;
    public GameObject rightHand, leftHand;
    public Grabber rightControllerGrabber, leftControllerGrabber;
    public GameObject rTubePosAndRot, lTubePosAndRot;
    public Grabbable TubeGrabbable;
    public Transform rightHandRemote, leftHandRemote;
    public GameObject arrowGuid;
    public AudioSource tubeLiquidAudio;
    public GameObject liquidTube;
    public bool isHoldTube;

    private void Update()
    {
        if (InputBridge.Instance.AButtonDown || Input.GetKeyDown(KeyCode.Z))
        {
            if (isHoldTube == true)
            {
                photonView.RPC("RPC_PlayTubeLiquid", RpcTarget.AllBuffered);
            }
        }

        if (InputBridge.Instance.AButtonUp || Input.GetKeyUp(KeyCode.Z))
        {
            photonView.RPC("RPC_StopTubeLiquid", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void RPC_PlayTubeLiquid()
    {
        tubeLiquidAudio.Play();
        liquidTube.SetActive(true);
        liquidTube.GetComponent<ParticleSystem>().Play();
    }

    [PunRPC]
    public void RPC_StopTubeLiquid()
    {
        tubeLiquidAudio.Stop();
        liquidTube.GetComponent<ParticleSystem>().Stop();
        liquidTube.SetActive(false);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "PlaceLiquidCollider")
        {
            if (liquidTube.activeSelf == true)
            {
                photonView.RPC("Rpc_CollideLiquidPlace", RpcTarget.AllBuffered);
            }
        }

        if (coll.tag == "WrongCollider")
        {
            photonView.RPC("Rpc_CollideWrongLayer", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_CollideLiquidPlace()
    {
        Debug.LogError("Tube collide liquid place");
        tubeLiquidAudio.Stop();
        eyeOperation.rightEffect.Play();
        eyeOperation.placeLiquidCollider.SetActive(false);
        eyeOperation.placeLiquidCollider.transform.GetChild(0).gameObject.SetActive(false);
        eyeOperation.step2.SetActive(false);
        eyeOperation.step3.SetActive(true);
        eyeOperation.peelBlendShape.GetComponent<SphereCollider>().enabled = true;
        eyeOperation.peelBlendShape.transform.GetChild(0).gameObject.SetActive(true);
    }
    
    [PunRPC]
    private void Rpc_CollideWrongLayer()
    {
        Debug.LogError("Tube collide wrong layers or outside eye area");
        eyeOperation.wrongEffect.Play();
    }

    public void OnHoldTube()
    {
        if (rightControllerGrabber.HeldGrabbable == TubeGrabbable)
        {
            isHoldTube = true;
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Tool Holding Idle");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Tool Holding");
            photonView.RPC("Rpc_RightHoldTube", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == TubeGrabbable)
        {
            isHoldTube = true;
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Tool Holding Idle");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Tool Holding");
            photonView.RPC("Rpc_LeftHoldTube", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_RightHoldTube()
    {
        arrowGuid.SetActive(false);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rRightHand = RemotePlayer.transform.GetChild(2);
            rightHandRemote = getChildTransformByName(rRightHand, "CustomHandRightBlackNew");
            rightHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
                Resources.Load<HandPose>("Tool Holding");
            rightHandRemote.transform.localPosition = rTubePosAndRot.transform.localPosition;
            rightHandRemote.transform.localRotation = rTubePosAndRot.transform.localRotation;
        }
    }

    [PunRPC]
    private void Rpc_LeftHoldTube()
    {
        arrowGuid.SetActive(false);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rLeftHand = RemotePlayer.transform.GetChild(1);
            leftHandRemote = getChildTransformByName(rLeftHand, "CustomHandLeftBlackNew");
            leftHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
                Resources.Load<HandPose>("Tool Holding");
            leftHandRemote.transform.localPosition = lTubePosAndRot.transform.localPosition;
            leftHandRemote.transform.localRotation = lTubePosAndRot.transform.localRotation;
        }
    }

    public void OnUnHoldTube()
    {
        if (rightControllerGrabber.HeldGrabbable == null)
        {
            isHoldTube = false;
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            photonView.RPC("Rpc_RightUnHoldTube", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == null)
        {
            isHoldTube = false;
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            photonView.RPC("Rpc_LeftUnHoldTube", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_RightUnHoldTube()
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
    private void Rpc_LeftUnHoldTube()
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