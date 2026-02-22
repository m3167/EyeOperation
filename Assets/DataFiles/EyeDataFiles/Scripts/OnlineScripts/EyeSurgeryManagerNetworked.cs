using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EyeSurgeryManagerNetworked : MonoBehaviourPun
{
    [Header("Step1")] public GameObject step1;
    public GameObject rightCorneaCutting;
    public GameObject leftCorneaCutting;
    public SkinnedMeshRenderer corneaMid;
    public SkinnedMeshRenderer corneaLiquid;

    [Header("Step2")] public GameObject step2;
    public GameObject placeLiquidCollider;

    [Header("Step3")] public GameObject step3;
    public GameObject peelBlendShape;
    public GameObject irisTimeline;
    public Image irisImg;
    
    [Header("Step4")] public GameObject step4;
    public GameObject whiteWaterParticle;
    public GameObject placeWhiteWaterCollider;

    [Header("Step5")] public GameObject step5;
    public GameObject placeInjectionLensCollider;
    public Animator lensInjector;
    public GameObject lensInInjector,lensInEye;
    
    [Header("Step6")] public GameObject step6;
    public GameObject placeNewLensCollider;
    public Animation newLens;

    [Header("Audios Effect")] public AudioSource rightEffect;
    public AudioSource wrongEffect;

    public void Update()
    {
        if (InputBridge.Instance.BButton || Input.GetKey(KeyCode.E))
        {
            PhotonNetwork.LoadLevel("OnlineLargeEyeSurgery");
        }
    }
}
