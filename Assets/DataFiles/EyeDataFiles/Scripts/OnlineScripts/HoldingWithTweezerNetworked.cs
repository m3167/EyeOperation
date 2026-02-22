using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Photon.Pun;

public class HoldingWithTweezerNetworked : MonoBehaviour
{
    public GameObject targetMicrobeHighlight;
    public SkinnedMeshRenderer tweezer;
    public AudioSource tweezerSoundEffect;
    public bool isHoldTweezer;
    public int tweezerStatue = 0;
    public GameObject rightHand, leftHand;
    public Grabber rightControllerGrabber, leftControllerGrabber;
    public GameObject rTwezzerPosAndRot, lTwezzerPosAndRot;
    public Grabbable scalpelGrabbable, tweezerGrabbable, syrgineGrabbable;
    public Transform rightHandRemote, leftHandRemote;
    
    public PhotonView _photonView;
    public static HoldingWithTweezerNetworked instance;

    private void Awake()
    {
        instance = this;
        _photonView = PhotonView.Get(this);
    }

    private void Update()
    {
        if (InputBridge.Instance.AButton || Input.GetKey(KeyCode.Z))
        {
            if (isHoldTweezer == true)
            {
                _photonView.RPC("Rpc_CloseTweezer", RpcTarget.AllBuffered);
            }
        }

        else
        {
            if (isHoldTweezer == true)
            {
                _photonView.RPC("Rpc_OpenTweezer", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void Rpc_CloseTweezer()
    {
        if (tweezer.GetBlendShapeWeight(0) == 100f)
        {
            return;
        }
        else
        {
            tweezerSoundEffect.Play();
        }

        tweezer.SetBlendShapeWeight(0, 100);
        tweezerStatue = 1;
    }

    [PunRPC]
    public void Rpc_OpenTweezer()
    {
        tweezer.SetBlendShapeWeight(0, 60);
        tweezerStatue = 0;
    }

    public void HoldingTweezer()
    {
        if (rightControllerGrabber.HeldGrabbable == tweezerGrabbable)
        {
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Scalpel Holding Idle");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Scalpel Holding");
            _photonView.RPC("Rpc_RightHoldTweezer", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == tweezerGrabbable)
        {
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Scalpel Holding Idle");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Scalpel Holding");
            _photonView.RPC("Rpc_LeftHoldTweezer", RpcTarget.AllBuffered);
        }
    }
    
    [PunRPC]
    public void Rpc_RightHoldTweezer()
    {
        isHoldTweezer = true;
        targetMicrobeHighlight.SetActive(true);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        Transform rRightHand = RemotePlayer.transform.GetChild(2);
        rightHandRemote = getChildTransformByName(rRightHand, "CustomHandRightBlackNew");
        rightHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
            Resources.Load<HandPose>("Scalpel Holding");
        rightHandRemote.transform.localPosition = rTwezzerPosAndRot.transform.localPosition;
        rightHandRemote.transform.localRotation = rTwezzerPosAndRot.transform.localRotation;
    }

    [PunRPC]
    public void Rpc_LeftHoldTweezer()
    {
        isHoldTweezer = true;
        targetMicrobeHighlight.SetActive(true);
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        Transform rLeftHand = RemotePlayer.transform.GetChild(1);
        leftHandRemote = getChildTransformByName(rLeftHand, "CustomHandLeftBlackNew");
        leftHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose =
            Resources.Load<HandPose>("Scalpel Holding");
        leftHandRemote.transform.localPosition = lTwezzerPosAndRot.transform.localPosition;
        leftHandRemote.transform.localRotation = lTwezzerPosAndRot.transform.localRotation;
    }

    public void UnHoldingTweezer()
    {
        if (rightControllerGrabber.HeldGrabbable == null)
        {
            rightHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            rightHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            _photonView.RPC("Rpc_RightUnHoldTweezer", RpcTarget.AllBuffered);
        }

        if (leftControllerGrabber.HeldGrabbable == null)
        {
            leftHand.GetComponent<HandPoseBlender>().Pose1 = Resources.Load<HandPose>("Open");
            leftHand.GetComponent<HandPoseBlender>().Pose2 = Resources.Load<HandPose>("Closed");
            _photonView.RPC("Rpc_LeftUnHoldTweezer", RpcTarget.AllBuffered);
        }
    }
    
    [PunRPC]
    public void Rpc_RightUnHoldTweezer()
    {
        isHoldTweezer = false;
        targetMicrobeHighlight.SetActive(false);
        tweezerStatue = 0;
        GameObject RemotePlayer = GameObject.Find("RemotePlayer");
        Transform rRightHand = RemotePlayer.transform.GetChild(2);
        rightHandRemote = getChildTransformByName(rRightHand, "CustomHandRightBlackNew");
        rightHandRemote.gameObject.GetComponent<HandPoser>().CurrentPose = Resources.Load<HandPose>("Open");
    }

    [PunRPC]
    public void Rpc_LeftUnHoldTweezer()
    {
        isHoldTweezer = false;
        targetMicrobeHighlight.SetActive(false);
        tweezerStatue = 0;
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