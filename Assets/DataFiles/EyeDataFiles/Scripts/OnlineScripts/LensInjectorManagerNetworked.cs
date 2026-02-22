using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Photon.Pun;
using UnityEngine.Serialization;

public class LensInjectorManagerNetworked : MonoBehaviourPun
{
    public EyeSurgeryManagerNetworked eyeOperation;
    public GameObject rightHand, leftHand;
    public Grabber rightControllerGrabber, leftControllerGrabber;
    public GameObject rLensInjectorPosAndRot, lLensInjectorPosAndRot;
    public Grabbable lensInjectorGrabbable;
    public Transform rightHandRemote, leftHandRemote;
    public GameObject arrowGuid;
    public bool isHoldLensInjector, isColliderWithPlaceNewLens;

    private void Update()
    {
        if (InputBridge.Instance.AButton || Input.GetKey(KeyCode.Z))
        {
            if (isHoldLensInjector == true && isColliderWithPlaceNewLens == true &&
                eyeOperation.lensInjector.enabled == false)
            {
                photonView.RPC("RPC_InsertNewLens", RpcTarget.AllBuffered);
            }
        }

        if (eyeOperation.lensInjector.enabled == true &&
            eyeOperation.lensInjector.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 &&
            !eyeOperation.lensInjector.IsInTransition(0))
        {
            eyeOperation.lensInjector.enabled = false;
            eyeOperation.rightEffect.Play();
            eyeOperation.step5.SetActive(false);
            eyeOperation.step6.SetActive(true);
            eyeOperation.placeNewLensCollider.SetActive(true);
            eyeOperation.lensInInjector.SetActive(false);
            eyeOperation.lensInEye.SetActive(true);
        }
    }

    [PunRPC]
    private void RPC_InsertNewLens()
    {
        eyeOperation.lensInjector.enabled = true;
        eyeOperation.placeInjectionLensCollider.SetActive(false);
        isColliderWithPlaceNewLens = false;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "PlaceInjectionLens")
        {
            photonView.RPC("Rpc_CollideInjectionLens", RpcTarget.AllBuffered);
        }

        if (coll.tag == "WrongCollider")
        {
            photonView.RPC("Rpc_CollideWrongLayer", RpcTarget.AllBuffered);
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "PlaceInjectionLens")
        {
            photonView.RPC("Rpc_NotCollideInjectionLens", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_CollideInjectionLens()
    {
        Debug.LogError("Lens injector collide Place Injection Lens");
        isColliderWithPlaceNewLens = true;
    }

    [PunRPC]
    private void Rpc_NotCollideInjectionLens()
    {
        Debug.LogError("Lens injector not collide Place Injection Lens");
        isColliderWithPlaceNewLens = false;
    }

    [PunRPC]
    private void Rpc_CollideWrongLayer()
    {
        Debug.LogError("Lens injector collide wrong layers or outside eye area");
        eyeOperation.wrongEffect.Play();
    }

    public void OnHoldLensInjector()
    {
        if (rightControllerGrabber.HeldGrabbable == lensInjectorGrabbable)
        {
            isHoldLensInjector = true;
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Tool Holding Idle");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Tool Holding");
            photonView.RPC("Rpc_RightHoldLensInjector", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == lensInjectorGrabbable)
        {
            isHoldLensInjector = true;
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Tool Holding Idle");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Tool Holding");
            photonView.RPC("Rpc_LeftHoldLensInjector", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_RightHoldLensInjector()
    {
        arrowGuid.SetActive(false);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rRightHand = RemotePlayer.transform.GetChild(2);
            rightHandRemote = getChildTransformByName(rRightHand, "CustomHandRightBlackNew");
            rightHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
                Resources.Load<HandPose>("Tool Holding");
            rightHandRemote.transform.localPosition = rLensInjectorPosAndRot.transform.localPosition;
            rightHandRemote.transform.localRotation = rLensInjectorPosAndRot.transform.localRotation;
        }
    }

    [PunRPC]
    private void Rpc_LeftHoldLensInjector()
    {
        arrowGuid.SetActive(false);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rLeftHand = RemotePlayer.transform.GetChild(1);
            leftHandRemote = getChildTransformByName(rLeftHand, "CustomHandLeftBlackNew");
            leftHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
                Resources.Load<HandPose>("Tool Holding");
            leftHandRemote.transform.localPosition = lLensInjectorPosAndRot.transform.localPosition;
            leftHandRemote.transform.localRotation = lLensInjectorPosAndRot.transform.localRotation;
        }
    }

    public void OnUnHoldLensInjector()
    {
        if (rightControllerGrabber.HeldGrabbable == null)
        {
            isHoldLensInjector = false;
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            photonView.RPC("Rpc_RightUnHoldLensInjector", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == null)
        {
            isHoldLensInjector = false;
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            photonView.RPC("Rpc_LeftUnHoldLensInjector", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_RightUnHoldLensInjector()
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
    private void Rpc_LeftUnHoldLensInjector()
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