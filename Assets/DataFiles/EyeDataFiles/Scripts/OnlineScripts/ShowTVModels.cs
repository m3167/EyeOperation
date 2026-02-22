using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ShowTVModels : MonoBehaviourPun
{
    public GameObject tV;
    public Image tVIcon;
    public Sprite showTVSprite;
    public Sprite hideTVSprite;
    public AudioSource tVEffect;
    public GameObject informations;

    public void OnShowAndHideMenuClicked()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            photonView.RPC("ShowAndHideTV", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void ShowAndHideTV()
    {
        for (int i = 0; i < informations.transform.childCount; i++)
        {
            informations.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (tVIcon.sprite == showTVSprite)
        {
            tV.SetActive(true);
            tVEffect.Play();
            tVIcon.sprite = hideTVSprite;
        }
        else
        {
            tV.SetActive(false);
            tVEffect.Play();
            tVIcon.sprite = showTVSprite;
        }
    }
}