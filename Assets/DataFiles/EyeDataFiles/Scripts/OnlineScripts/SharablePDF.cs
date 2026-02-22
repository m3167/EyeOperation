using System.Collections;
using System.Collections.Generic;
using Paroxe.PdfRenderer;
using UnityEngine;
using Photon.Pun;
using Paroxe.PdfRenderer.Internal.Viewer;
using UnityEngine.EventSystems;

public class SharablePDF : MonoBehaviour
{ 
    public PDFViewerInternal _pDFViewerInternal;

    public PDFViewerSearchButton pDFViewerSearchButton;
    public PDFSearchPanel _pDFSearchPanel;
    public PDFBookmarkListItem _pDFBookmarkListItem;
    public PDFThumbnailItem _pDFThumbnailItem;

    public PDFViewerLeftPanel _pDFViewerLeftPanel;
    public PDFViewer _pdfViewer;
    
    public PhotonView _photonView;

    public void Start()
    {
        _photonView = PhotonView.Get(this);
    }

    # region Public Methods

    public void OnNextPage()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnNextPage", RpcTarget.AllBuffered);
        }
    }


    public void OnPreviousPage()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnPreviousPage", RpcTarget.AllBuffered);
        }
    }


    public void OnZoomIn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnZoomIn", RpcTarget.AllBuffered);
        }
    }


    public void OnZoomOut()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnZoomOut", RpcTarget.AllBuffered);
        }
    }


    public void OnPageIndexEditEnd()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnPageIndexEditEnd", RpcTarget.AllBuffered);
        }
    }


    public void OnClickSearch()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnClickSearch", RpcTarget.AllBuffered);
        }
    }

    public void OnInputFieldEndEdit()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnInputFieldEndEdit", RpcTarget.AllBuffered);
        }
    }

    public void OnCloseButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnCloseButton", RpcTarget.AllBuffered);
        }
    }

    public void OnMatchCaseClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnMatchCaseClicked", RpcTarget.AllBuffered);
        }
    }

    public void OnMatchWholeWordCliked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnMatchWholeWordCliked", RpcTarget.AllBuffered);
        }
    }

    public void OnNextButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnNextButton", RpcTarget.AllBuffered);
        }
    }

    public void OnPreviousButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnPreviousButton", RpcTarget.AllBuffered);
        }
    }

    public void OnItemClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnItemClicked", RpcTarget.AllBuffered);
        }
    }

    public void OnExpandButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnExpandButton", RpcTarget.AllBuffered);
        }
    }

    public void OnThumbnailClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnThumbnailClicked", RpcTarget.AllBuffered);
        }
    }

    public void OnThumbnailsTabClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnThumbnailsTabClicked", RpcTarget.AllBuffered);
        }
    }


    public void OnBookmarksTabClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnBookmarksTabClicked", RpcTarget.AllBuffered);
        }
    }

    public void OnToggleTabClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_ToggleTabClicked", RpcTarget.AllBuffered);
        }
    }

    public void OnDragClicked(BaseEventData eventData)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnDrag", RpcTarget.AllBuffered);
        }
    }

    public void OnEndDragClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnEndDrag", RpcTarget.AllBuffered);
        }
    }

    public void OnBeginDragClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnBeginDrag", RpcTarget.AllBuffered);
        }
    }

    public void OnPointerEnterClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnPointerEnter", RpcTarget.AllBuffered);
        }
    }

    public void OnPointerExitClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_OnPointerExit", RpcTarget.AllBuffered);
        }
    }
    
    // public void OnUpdateScrollView()
    // {
    //     if (PhotonNetwork.IsMasterClient)
    //     {
    //         _photonView.RPC("RPC_UpdateScrollView", RpcTarget.AllBuffered);
    //     }
    // }

    #endregion

    #region  RPC Methods
    [PunRPC]
    public void RPC_OnNextPage()
    {
        _pDFViewerInternal.OnNextPageButtonClicked();
    }

    [PunRPC]
    public void RPC_OnPreviousPage()
    {
        _pDFViewerInternal.OnPreviousPageButtonClicked();
    }

    [PunRPC]
    public void RPC_OnZoomIn()
    {
        _pDFViewerInternal.OnZoomInButtonClicked();
    }

    [PunRPC]
    public void RPC_OnZoomOut()
    {
        _pDFViewerInternal.OnZoomOutButtonClicked();
    }

    [PunRPC]
    public void RPC_OnPageIndexEditEnd()
    {
        _pDFViewerInternal.OnPageIndexEditEnd();
    }

    [PunRPC]
    public void RPC_OnClickSearch()
    {
        pDFViewerSearchButton.OnClick();
    }

    [PunRPC]
    public void RPC_OnInputFieldEndEdit()
    {
        _pDFSearchPanel.OnInputFieldEndEdit();
    }

    [PunRPC]
    public void RPC_OnCloseButton()
    {
        _pDFSearchPanel.OnCloseButton();
    }

    [PunRPC]
    public void RPC_OnMatchCaseClicked()
    {
        _pDFSearchPanel.OnMatchCaseClicked();
    }

    [PunRPC]
    public void RPC_OnMatchWholeWordCliked()
    {
        _pDFSearchPanel.OnMatchWholeWordCliked();
    }

    [PunRPC]
    public void RPC_OnNextButton()
    {
        _pDFSearchPanel.OnNextButton();
    }

    [PunRPC]
    public void RPC_OnPreviousButton()
    {
        _pDFSearchPanel.OnPreviousButton();
    }

    [PunRPC]
    public void RPC_OnItemClicked()
    {
        _pDFBookmarkListItem.OnItemClicked();
    }

    [PunRPC]
    public void RPC_OnExpandButton()
    {
        _pDFBookmarkListItem.OnExpandButton();
    }

    [PunRPC]
    public void RPC_OnThumbnailClicked()
    {
        _pDFThumbnailItem.OnThumbnailClicked();
    }

    [PunRPC]
    public void RPC_OnThumbnailsTabClicked()
    {
        _pDFViewerLeftPanel.OnThumbnailsTabClicked();
    }

    [PunRPC]
    public void RPC_OnBookmarksTabClicked()
    {
        _pDFViewerLeftPanel.OnBookmarksTabClicked();
    }

    [PunRPC]
    public void RPC_ToggleTabClicked()
    {
        _pDFViewerLeftPanel.Toggle();
    }

    [PunRPC]
    public void RPC_OnDrag(BaseEventData eventData)
    {
        _pDFViewerLeftPanel.OnDrag(eventData);
    }

    [PunRPC]
    public void RPC_OnEndDrag()
    {
        _pDFViewerLeftPanel.OnEndDrag();
    }

    [PunRPC]
    public void RPC_OnBeginDrag()
    {
        _pDFViewerLeftPanel.OnBeginDrag();
    }

    [PunRPC]
    public void RPC_OnPointerEnter()
    {
        _pDFViewerLeftPanel.OnPointerEnter();
    }

    [PunRPC]
    public void RPC_OnPointerExit()
    {
        _pDFViewerLeftPanel.OnPointerExit();
    }
    #endregion
}
