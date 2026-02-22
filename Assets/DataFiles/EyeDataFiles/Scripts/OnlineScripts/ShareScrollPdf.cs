using Paroxe.PdfRenderer.Internal.Viewer;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShareScrollPdf : MonoBehaviourPun
{
    public GameObject canvasPdf;
    public ScrollRect scrollRectPdf;
    public Scrollbar scrollbarPdf;
    private Vector2 previousScrollValue;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            canvasPdf.GetComponent<GraphicRaycaster>().enabled = true;
        }
        else
        {
            canvasPdf.GetComponent<GraphicRaycaster>().enabled = false;
        }

        scrollRectPdf.onValueChanged.AddListener(OnScrollValueChangedpdf);

        previousScrollValue = scrollRectPdf.normalizedPosition;
    }

    private void OnScrollValueChangedpdf(Vector2 scrollValue)
    {
        if (scrollValue != previousScrollValue)
        {
            previousScrollValue = scrollValue;
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("UpdatePdf", RpcTarget.All, scrollValue.y);
            }
        }

        Debug.Log(scrollValue.y);
    }

    [PunRPC]
    private void UpdatePdf(float scrollY)
    {
        scrollbarPdf.value = scrollY;
    }
}