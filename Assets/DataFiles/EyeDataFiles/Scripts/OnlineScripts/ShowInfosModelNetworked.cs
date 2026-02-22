using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ShowInfosModelNetworked : MonoBehaviourPun
{
    public GameObject[] infos;
    public Image infoIcon;
    public Sprite showInfoSprite;
    public Sprite hideInfoSprite;
    public AudioSource infoEffect;

    public PhotonView _photonView;
    public static ShowInfosModelNetworked instance;

    private void Awake()
    {
        instance = this;
        _photonView = PhotonView.Get(this);
    }

    public void ShowAndHideInfo()
    {
       _photonView.RPC("Rpc_ShowAndHideInfo", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Rpc_ShowAndHideInfo()
    {
        if (infoIcon.sprite == showInfoSprite)
        {
            infoEffect.Play();
            infoIcon.sprite = hideInfoSprite;
            foreach (GameObject info in infos)
            {
                info.SetActive(true);
            }
        }
        else
        {
            infoEffect.Play();
            infoIcon.sprite = showInfoSprite;
            foreach (GameObject info in infos)
            {
                info.SetActive(false);
            }
        }
    }
}
