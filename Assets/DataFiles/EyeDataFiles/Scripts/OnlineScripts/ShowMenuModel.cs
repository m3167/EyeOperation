using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShowMenuModel : MonoBehaviourPun
{
    public Animation menuModelAnim;
    public Image menuIcon;
    public Sprite showMenuSprite;
    public Sprite hideMenuSprite;
    public AudioSource menuEffect;

    public void OnShowAndHideMenuClicked()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            photonView.RPC("ShowAndHideMenu", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void ShowAndHideMenu()
    {
        if (menuIcon.sprite == showMenuSprite)
        {
            menuModelAnim["MenuModel"].speed = 1;
            menuModelAnim.Play();
            menuEffect.Play();
            menuIcon.sprite = hideMenuSprite;
        }
        else
        {
            menuModelAnim["MenuModel"].speed = -1;
            menuModelAnim["MenuModel"].time = menuModelAnim["MenuModel"].length;
            menuModelAnim.Play();
            menuEffect.Play();
            menuIcon.sprite = showMenuSprite;
        }
    }
}