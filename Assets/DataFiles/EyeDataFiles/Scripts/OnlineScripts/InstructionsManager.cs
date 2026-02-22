using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Serialization;

public class InstructionsManager : MonoBehaviourPunCallbacks
{
    public GameObject instructionQuest, instructionWindows;
    public bool is360Scene;
    public bool isoutsideScene;

    private void Start()
    {
        if (is360Scene == true || isoutsideScene == true)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.WindowsPlayer)
            {
                Debug.Log("Platform is " + Application.platform);
                StartCoroutine("ShowInstructionWindows");
            }
            else
            {
                Debug.Log("Platform is " + Application.platform);
                StartCoroutine("ShowInstructionQuest");
            }
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor ||
                    Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    Debug.Log("Platform is " + Application.platform);
                    StartCoroutine("ShowInstructionWindows");
                }
                else
                {
                    Debug.Log("Platform is " + Application.platform);
                    StartCoroutine("ShowInstructionQuest");
                }
            }
        }
    }

    public IEnumerator ShowInstructionQuest()
    {
        yield return new WaitForSeconds(15f);
        instructionQuest.SetActive(true);
        StartCoroutine("HideInstructionQuest");
    }

    public IEnumerator HideInstructionQuest()
    {
        yield return new WaitForSeconds(5f);
        instructionQuest.SetActive(false);
    }

    public IEnumerator ShowInstructionWindows()
    {
        yield return new WaitForSeconds(15f);
        instructionWindows.SetActive(true);
        StartCoroutine("HideInstructionWindows");
    }

    public IEnumerator HideInstructionWindows()
    {
        yield return new WaitForSeconds(5f);
        instructionWindows.SetActive(false);
    }
}