using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BNG;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using UnityEngine.Video;
using NetworkPlayer = BNG.NetworkPlayer;

public class PlayerTabletManager : MonoBehaviourPunCallbacks
{
    [Header("Tablet Master")] public GameObject tabletMaster;
    [Header("Tablet NonMaster")] public GameObject tabletNonMaster;

    [Header("Intro Panel")] public GameObject intro_UI_Panel;

    [Header("Home Panel")] public GameObject home_UI_Panel;

    [Header("Environment Panel")] public GameObject environment_UI_Panel;
    public GameObject environmentParentButtons;

    [Header("parts Of Eye Panel")] public GameObject partsOfEye_UI_Panel;

    [Header("Anatomy and coats Of Eye Panel")]
    public GameObject anatomyAndCoatsOfEye_UI_Panel;

    [Header("parts Of Eye Panel")] public GameObject conjunctiva_UI_Panel;

    [Header("parts Of Eye Panel")] public GameObject inframmationConjunctiva_UI_Panel;

    [Header("parts Of Eye Panel")] public GameObject partsConjunctiva_UI_Panel;

    [Header("parts Of Eye Panel")] public GameObject anatomyConjunctiva_UI_Panel;

    [Header("Category Videos Panel")] public GameObject categoryVideos_UI_Panel;

    [Header("Biological Videos Panel")] public GameObject biologicalVideos_UI_Panel;

    [Header("Canvas Video Biological Clips")]
    public GameObject canvasBiologicalVideoClips_UI_Panel;

    [Header("Canvas Web")] public GameObject canvasWeb_UI_Panel;

    [Header("Canvas Pdf")] public GameObject canvasPdf_UI_Panel;

    public GameObject localVRPlayerCamera;
    public GameObject tabletMasterPrefab, tabletNonMasterPrefab;
    private GameObject player;
    private Vector3 playerPos, playerDirection;
    private Quaternion playerRotation;
    public float spawnDistance;
    private Vector3 spawnPos;
    private GameObject instantiatedPrefab, instantiatedTabletNonMasterPrefab;
    public PlayerRotation playerRotationScript;

    public static PlayerTabletManager instance;

    public void Start()
    {
        if (photonView.IsMine)
        {
            // Enable the component for the local player
            gameObject.GetComponent<PlayerTabletManager>().enabled = true;
        }
        else
        {
            // Disable the component for remote players
            gameObject.GetComponent<PlayerTabletManager>().enabled = false;
        }

        player = GameObject.Find("CenterEyeAnchor");
        HideTabletClicked();
        tabletNonMaster.SetActive(false);
    }

    void Update()
    {
        if (InputBridge.Instance.XButton || Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Button Down");
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (Application.platform == RuntimePlatform.WindowsPlayer ||
                    Application.platform == RuntimePlatform.WindowsEditor)
                {
                    if (GameObject.FindGameObjectsWithTag("Tablet").Length == 0)
                    {
                        Debug.Log("Not exist game object with tag tablet");
                    }
                    else
                    {
                        Debug.Log("Exist game object with tag tablet");
                        DestroyModelNetworked.instance.OnHideAllModelClicked();
                    }

                    playerPos = player.transform.position;
                    playerDirection = player.transform.forward;
                    playerRotation = player.transform.rotation;
                    spawnPos = playerPos + playerDirection * spawnDistance;

                    GameObject instantiatedModel =
                        PhotonNetwork.Instantiate(tabletMasterPrefab.name,
                            spawnPos, playerRotation);
                    instantiatedModel.transform.localScale += new Vector3(0.7f, 0.7f, 0.7f);
                }
                else
                {
                    if (photonView.IsMine)
                    {
                        ShowTabletClicked();
                    }
                    else
                    {
                        HideTabletClicked();
                    }
                }
            }
            else
            {
                if (Application.platform == RuntimePlatform.WindowsPlayer ||
                    Application.platform == RuntimePlatform.WindowsEditor)
                {
                    if (instantiatedTabletNonMasterPrefab != null)
                    {
                        Destroy(instantiatedTabletNonMasterPrefab.gameObject);
                    }

                    playerPos = player.transform.position;
                    playerDirection = player.transform.forward;
                    playerRotation = player.transform.rotation;
                    spawnPos = playerPos + playerDirection * spawnDistance;

                    instantiatedTabletNonMasterPrefab = Instantiate(tabletNonMasterPrefab, spawnPos, playerRotation);
                    instantiatedTabletNonMasterPrefab.transform.localScale += new Vector3(0.7f, 0.7f, 0.7f);
                }
                else
                {
                    tabletNonMaster.SetActive(true);
                }
            }
        }

        if (InputBridge.Instance.YButton || Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Button Down");
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (Application.platform == RuntimePlatform.WindowsPlayer ||
                    Application.platform == RuntimePlatform.WindowsEditor)
                {
                    DestroyModelNetworked.instance.OnHideAllModelClicked();
                }
                else
                {
                    HideTabletClicked();
                }
            }
            else
            {
                if (Application.platform == RuntimePlatform.WindowsPlayer ||
                    Application.platform == RuntimePlatform.WindowsEditor)
                {
                    Destroy(instantiatedTabletNonMasterPrefab.gameObject);
                }
                else
                {
                    tabletNonMaster.SetActive(false);
                }
            }
        }
    }

    #region Public Methods

    public void ShowTabletClicked()
    {
        photonView.RPC("ShowTablet", RpcTarget.AllBuffered);
    }

    public void HideTabletClicked()
    {
        photonView.RPC("HideTablet", RpcTarget.AllBuffered);
    }

    public void ActivePanelClicked(string activatedPanel)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            photonView.RPC("ActivatePanel", RpcTarget.AllBuffered, activatedPanel);
        }
    }

    public void OnChooseEnvironmentButtonClicked(string nameOfEnvironment)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(nameOfEnvironment);
        }
    }

    public void OnDisableTabletCurrentScenes()
    {
        photonView.RPC("DisableTabletCurrentScene", RpcTarget.AllBuffered);
    }

    public void SharePDFButtonClicked()
    {
        ShareManager.instance.OnSharePDFClicked();
    }

    public void ShareWebButtonClicked()
    {
        ShareManager.instance.OnShareWebClicked();
    }

    public void ShareVideoCanvasButtonClicked()
    {
        ShareManager.instance.OnShareVideoClicked();
    }


    public void AssignBiologyVideosClipsToCanvasVideoClicked(int videoClipIndex)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            photonView.RPC("AssignBiologyVideoClips", RpcTarget.AllBuffered, videoClipIndex);
        }
    }

    public void InstantiateModel(GameObject prefabModel)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Debug.Log("well Instantiated " + prefabModel.name);
            localVRPlayerCamera = GameObject.Find("PlayerController");
            playerPos = localVRPlayerCamera.transform.position;
            playerDirection = localVRPlayerCamera.transform.forward;
            playerRotation = localVRPlayerCamera.transform.rotation;
            spawnPos = playerPos + playerDirection * (spawnDistance + 0.5f);
            instantiatedPrefab = PhotonNetwork.Instantiate(prefabModel.name, spawnPos, playerRotation);
            PhotonView tempView = instantiatedPrefab.GetComponent<PhotonView>();
            if (tempView != null)
                photonView.RPC("AssignModelToRotatedObj", RpcTarget.AllBuffered, tempView.ViewID);
        }
    }

    [PunRPC]
    public void AssignModelToRotatedObj(int viewId)
    {
        playerRotationScript = (PlayerRotation)FindObjectOfType(typeof(PlayerRotation));
        PhotonView photonViewModel = PhotonView.Find(viewId);
        playerRotationScript.rotatedObj = photonViewModel.gameObject;
    }

    public void ExitRoomButton()
    {
        VirtualWorldManager.instance.LeaveRoomAndLoadHomeScene();
    }

    #endregion

    #region Rpc Methods

    [PunRPC]
    public void ShowTablet()
    {
        tabletMaster.SetActive(true);
    }

    [PunRPC]
    public void HideTablet()
    {
        tabletMaster.SetActive(false);
    }

    [PunRPC]
    public void AssignBiologyVideoClips(int videoClipIndex)
    {
        canvasBiologicalVideoClips_UI_Panel.transform.GetChild(0).GetComponent<VideoManager>().m_VideoPlayer.clip
            = canvasBiologicalVideoClips_UI_Panel.transform.GetChild(0).GetComponent<VideoManager>()
                .m_VideoClips[videoClipIndex];
    }

    [PunRPC]
    public void DisableTabletCurrentScene()
    {
        Scene m_Scene;
        string sceneName;
        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;
        Debug.Log("Active scene >> " + sceneName);

        if (sceneName == "OnlineLectureRoom")
        {
            environmentParentButtons.transform.GetChild(1).GetComponent<Button>().interactable = false;
        }

        else if (sceneName == "OnlineConferenceRoom")
        {
            environmentParentButtons.transform.GetChild(2).GetComponent<Button>().interactable = false;
        }
    }

    [PunRPC]
    public void ActivatePanel(string panelToActivated)
    {
        intro_UI_Panel.SetActive(panelToActivated.Equals(intro_UI_Panel.name));
        home_UI_Panel.SetActive(panelToActivated.Equals(home_UI_Panel.name));
        environment_UI_Panel.SetActive(panelToActivated.Equals(environment_UI_Panel.name));
        partsOfEye_UI_Panel.SetActive(panelToActivated.Equals(partsOfEye_UI_Panel.name));
        anatomyAndCoatsOfEye_UI_Panel.SetActive(panelToActivated.Equals(anatomyAndCoatsOfEye_UI_Panel.name));
        conjunctiva_UI_Panel.SetActive(panelToActivated.Equals(conjunctiva_UI_Panel.name));
        inframmationConjunctiva_UI_Panel.SetActive(panelToActivated.Equals(inframmationConjunctiva_UI_Panel.name));
        partsConjunctiva_UI_Panel.SetActive(panelToActivated.Equals(partsConjunctiva_UI_Panel.name));
        anatomyConjunctiva_UI_Panel.SetActive(panelToActivated.Equals(anatomyConjunctiva_UI_Panel.name));
        categoryVideos_UI_Panel.SetActive(panelToActivated.Equals(categoryVideos_UI_Panel.name));
        biologicalVideos_UI_Panel.SetActive(panelToActivated.Equals(biologicalVideos_UI_Panel.name));
        canvasBiologicalVideoClips_UI_Panel.SetActive(
            panelToActivated.Equals(canvasBiologicalVideoClips_UI_Panel.name));
        canvasWeb_UI_Panel.SetActive(panelToActivated.Equals(canvasWeb_UI_Panel.name));
        canvasPdf_UI_Panel.SetActive(panelToActivated.Equals(canvasPdf_UI_Panel.name));
    }

    #endregion
}