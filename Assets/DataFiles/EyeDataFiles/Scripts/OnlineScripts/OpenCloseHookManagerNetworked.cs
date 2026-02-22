using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Photon.Pun;
//using UnityEngine.Formats.Alembic.Importer;
using UnityEngine.Serialization;

public class OpenCloseHookManagerNetworked : MonoBehaviourPun
{
    public EyeSurgeryManagerNetworked eyeOperation;
    public GameObject rightHand, leftHand;
    public Grabber rightControllerGrabber, leftControllerGrabber;
    public GameObject rOpenCloseHookPosAndRot, lOpenCloseHookPosAndRot;
    public Grabbable OpenCloseHookGrabbable;
    public Transform rightHandRemote, leftHandRemote;
    public GameObject arrowGuid;
    public AudioSource hookAudio;
    public GameObject[] irisCollider;
    public int counterCollider;
    public bool isHoldOpenCloseHook, isHoldAButton;

    private void Update()
    {
        if (InputBridge.Instance.AButtonDown || Input.GetKeyDown(KeyCode.Z))
        {
            if (isHoldOpenCloseHook == true)
            {
                Debug.Log("Click down on AButton and ZButton");
                photonView.RPC("Rpc_CloseHook", RpcTarget.AllBuffered);
            }
        }

        if (InputBridge.Instance.AButtonUp || Input.GetKeyUp(KeyCode.Z))
        {
            Debug.Log("Click up on AButton and ZButton");
            photonView.RPC("Rpc_OpenHook", RpcTarget.AllBuffered);
        }

        if (counterCollider >= 34)
        {
            Debug.Log("iris timeline is done");
            eyeOperation.step3.SetActive(false);
            eyeOperation.step4.SetActive(true);
            eyeOperation.placeWhiteWaterCollider.SetActive(true);
            eyeOperation.rightEffect.Play();
            counterCollider = 0;
            eyeOperation.irisTimeline.SetActive(false);
        }

        // if (eyeOperation.irisTimeline.GetComponent<AlembicStreamPlayer>().CurrentTime == 2.5f)
        // {
        //     Debug.Log("iris timeline is done");
        //     eyeOperation.irisTimeline.SetActive(false);
        //     eyeOperation.step2Object.SetActive(false);
        //     eyeOperation.step3Object.SetActive(true);
        // }
    }

    [PunRPC]
    public void Rpc_CloseHook()
    {
        isHoldAButton = true;
        gameObject.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0);
        hookAudio.Play();
    }

    [PunRPC]
    public void Rpc_OpenHook()
    {
        isHoldAButton = false;
        gameObject.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100);
        hookAudio.Stop();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "PeelBlendShape")
        {
            if (isHoldOpenCloseHook == true && isHoldAButton == true)
            {
                photonView.RPC("Rpc_CuttingPeel", RpcTarget.AllBuffered);
            }
        }

        if (coll.tag == "IrisTimeline")
        {
            if (isHoldOpenCloseHook == true && isHoldAButton == true)
            {
                photonView.RPC("Rpc_CuttingIris", RpcTarget.AllBuffered);
            }
        }

        if (coll.tag == "WrongCollider")
        {
            photonView.RPC("Rpc_CollideWrongLayer", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void Rpc_CuttingPeel()
    {
        Debug.Log("CuttingPeel");
        eyeOperation.peelBlendShape.GetComponent<Animation>().Play();
        Invoke("ShowIris", eyeOperation.peelBlendShape.GetComponent<Animation>().clip.length);
    }

    public void ShowIris()
    {
        eyeOperation.peelBlendShape.GetComponent<SphereCollider>().enabled = false;
        eyeOperation.peelBlendShape.transform.GetChild(0).gameObject.SetActive(false);
        eyeOperation.irisTimeline.SetActive(true);
    }

    [PunRPC]
    public void Rpc_CuttingIris()
    {
        Debug.Log("CuttingIris");
        eyeOperation.peelBlendShape.SetActive(false);
        irisCollider[counterCollider].SetActive(false);
        //eyeOperation.irisTimeline.GetComponent<AlembicStreamPlayer>().CurrentTime += 0.1666667f;
        eyeOperation.irisImg.fillAmount -= 0.02857143f;
        counterCollider++;
        irisCollider[counterCollider].SetActive(true);
    }

    [PunRPC]
    private void Rpc_CollideWrongLayer()
    {
        Debug.LogError("Open Close Hook collide wrong layers or outside eye area");
        eyeOperation.wrongEffect.Play();
    }

    public void OnHoldOpenCloseHook()
    {
        if (rightControllerGrabber.HeldGrabbable == OpenCloseHookGrabbable)
        {
            isHoldOpenCloseHook = true;
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Tool Holding Idle");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Tool Holding");
            photonView.RPC("Rpc_RightHoldOpenCloseHook", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == OpenCloseHookGrabbable)
        {
            isHoldOpenCloseHook = true;
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Tool Holding Idle");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Tool Holding");
            photonView.RPC("Rpc_LeftHoldOpenCloseHook", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_RightHoldOpenCloseHook()
    {
        arrowGuid.SetActive(false);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rRightHand = RemotePlayer.transform.GetChild(2);
            rightHandRemote = getChildTransformByName(rRightHand, "CustomHandRightBlackNew");
            rightHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
                Resources.Load<HandPose>("Tool Holding");
            rightHandRemote.transform.localPosition = rOpenCloseHookPosAndRot.transform.localPosition;
            rightHandRemote.transform.localRotation = rOpenCloseHookPosAndRot.transform.localRotation;
        }
    }

    [PunRPC]
    private void Rpc_LeftHoldOpenCloseHook()
    {
        arrowGuid.SetActive(false);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        if (RemotePlayer != null)
        {
            Transform rLeftHand = RemotePlayer.transform.GetChild(1);
            leftHandRemote = getChildTransformByName(rLeftHand, "CustomHandLeftBlackNew");
            leftHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
                Resources.Load<HandPose>("Tool Holding");
            leftHandRemote.transform.localPosition = lOpenCloseHookPosAndRot.transform.localPosition;
            leftHandRemote.transform.localRotation = lOpenCloseHookPosAndRot.transform.localRotation;
        }
    }

    public void OnUnHoldOpenCloseHook()
    {
        if (rightControllerGrabber.HeldGrabbable == null)
        {
            isHoldOpenCloseHook = false;
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            photonView.RPC("Rpc_RightUnHoldOpenCloseHook", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == null)
        {
            isHoldOpenCloseHook = false;
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            photonView.RPC("Rpc_LeftUnHoldOpenCloseHook", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Rpc_RightUnHoldOpenCloseHook()
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
    private void Rpc_LeftUnHoldOpenCloseHook()
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