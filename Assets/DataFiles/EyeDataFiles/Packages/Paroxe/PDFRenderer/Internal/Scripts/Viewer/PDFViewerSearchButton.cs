using UnityEngine.EventSystems;
using UnityEngine;

namespace Paroxe.PdfRenderer.Internal.Viewer
{
    public class PDFViewerSearchButton : MonoBehaviour
    {   
        public void OnClick()
        {
#if !UNITY_WEBGL
            GetComponentInParent<PDFViewer>().m_Internal.SearchPanel.GetComponent<PDFSearchPanel>().Toggle();
#endif
        }
    }
}