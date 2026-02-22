using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ScrollBarNetworked : MonoBehaviourPun
{
    public ScrollRect scrollRect;
    public Scrollbar scrollbar;
    private Vector2 previousScrollValue;


    private void Start()
    {
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

        previousScrollValue = scrollRect.normalizedPosition;
    }

    private void OnScrollValueChanged(Vector2 scrollValue)
    {
        if (scrollValue != previousScrollValue)
        {
            previousScrollValue = scrollValue;
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("UpdateScrollUI", RpcTarget.All, scrollValue.y);
            }
        }

        Debug.Log(scrollValue.y);
    }

    [PunRPC]
    private void UpdateScrollUI(float scrollY)
    {
        scrollbar.value = scrollY;
    }
}