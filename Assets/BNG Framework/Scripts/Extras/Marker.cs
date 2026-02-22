using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace BNG
{
    public class Marker : GrabbableEvents
    {
        [SerializeField]
        private UnityEngine.UI.Slider fontSlider;
        public MarkerColor markerColor;
        public Transform DrawLine;
        public Transform root;
        public Material DrawMaterial;
        public Color DrawColor = Color.red;
        public float LineWidth = 0.02f;
        public Transform RaycastStart;
        public LayerMask DrawingLayers;
        public float RaycastLength = 0.01f;

        /// <summary>
        /// Minimum distance required from points to place drawing down
        /// </summary>
        public float MinDrawDistance = 0.02f;
        public float ReuseTolerance = 0.001f;

        bool IsNewDraw = false;
        Vector3 lastDrawPoint;
        LineRenderer LineRenderer;

        // Use this to store our Marker's LineRenderers 
        Transform lastTransform;
        Coroutine drawRoutine = null;
        float lastLineWidth = 0;
        int renderLifeTime = 0;


        public override void OnGrab(Grabber grabber)
        {
            if (drawRoutine == null)
            {
                drawRoutine = StartCoroutine(WriteRoutine());
            }

            base.OnGrab(grabber);
        }

        public override void OnRelease()
        {
            if (drawRoutine != null)
            {
                StopCoroutine(drawRoutine);
                drawRoutine = null;
            }
            base.OnRelease();
        }

        IEnumerator WriteRoutine()
        {
            while (true)
            {
                if (Physics.Raycast(RaycastStart.position, RaycastStart.up, out RaycastHit hit, RaycastLength, DrawingLayers, QueryTriggerInteraction.Ignore))
                {
                    float tipDistance = Vector3.Distance(hit.point, RaycastStart.transform.position);
                    float tipDercentage = tipDistance / RaycastLength;
                    Vector3 drawStart = hit.point + (-RaycastStart.up * 0.0005f);
                    Quaternion drawRotation = Quaternion.FromToRotation(Vector3.back, hit.normal);
                    float lineWidth = LineWidth * (1 - tipDercentage);
                    InitDraw(drawStart, drawRotation, lineWidth, DrawColor);
                }
                else
                {
                    IsNewDraw = true;
                }
                yield return new WaitForFixedUpdate();
            }
        }

        void InitDraw(Vector3 position, Quaternion rotation, float lineWidth, Color lineColor)
        {
            if (IsNewDraw)
            {
                lastDrawPoint = position;
                DrawPoint(lastDrawPoint, position, lineWidth, lineColor, rotation);
                IsNewDraw = false;
            }
            else
            {
                float dist = Vector3.Distance(lastDrawPoint, position);
                if (dist > MinDrawDistance)
                {
                    lastDrawPoint = DrawPoint(lastDrawPoint, position, lineWidth, DrawColor, rotation);
                }
            }
        }

        Vector3 DrawPoint(Vector3 lastDrawPoint, Vector3 endPosition, float lineWidth, Color lineColor, Quaternion rotation)
        {
            var dif = Mathf.Abs(lastLineWidth - lineWidth);
            lastLineWidth = lineWidth;
            if (dif > ReuseTolerance || renderLifeTime >= 98)
            {
                LineRenderer = null;
                renderLifeTime = 0;
            }
            else
            {
                renderLifeTime += 1;
            }
            if (IsNewDraw || LineRenderer == null)
            {
                //lastTransform = new GameObject().transform;
                lastTransform = PhotonNetwork.Instantiate(DrawLine.name, DrawLine.localPosition, DrawLine.localRotation).transform;

                //lastTransform.GetComponent<LineRenderer>();

                BNG.NetworkPlayer playerNetworkSetup = GameObject.Find("MyRemotePlayer").GetComponent<BNG.NetworkPlayer>();

                int markerIndex;
                if (markerColor == MarkerColor.black) markerIndex = 0;
                else if (markerColor == MarkerColor.Blue) markerIndex = 1;
                else markerIndex = 2;

                int LineId = lastTransform.GetComponent<PhotonView>().ViewID;

                playerNetworkSetup.DrawLine_Remote(LineId, markerIndex, lastDrawPoint.x, lastDrawPoint.y, lastDrawPoint.z, endPosition.x, endPosition.y, endPosition.z,
                    lineWidth, lineColor.r, lineColor.g, lineColor.b, rotation.x, rotation.y, rotation.z, rotation.w);

                //DrawLineViaNetwork(lastDrawPoint, endPosition, lineWidth, lineColor, rotation);
            }
            else
            {
                if (LineRenderer != null)
                {
                    BNG.NetworkPlayer playerNetworkSetup = GameObject.Find("MyRemotePlayer").GetComponent<BNG.NetworkPlayer>();

                    int markerIndex;
                    if (markerColor == MarkerColor.black) markerIndex = 0;
                    else if (markerColor == MarkerColor.Blue) markerIndex = 1;
                    else markerIndex = 2;

                    playerNetworkSetup.ContinueDrawLine_Remote(markerIndex, endPosition.x, endPosition.y, endPosition.z, lineWidth);
                }
            }
            return endPosition;
        }

        public void ContinueDrawLineViaNetwork(float endPositionX, float endPositionY, float endPositionZ, float lineWidth)
        {
            Vector3 endPosition = new Vector3(endPositionX, endPositionY, endPositionZ);


            LineRenderer.widthMultiplier = 1;
            LineRenderer.positionCount += 1;
            var curve = LineRenderer.widthCurve;
            curve.AddKey((LineRenderer.positionCount - 1) / 100, lineWidth);
            LineRenderer.widthCurve = curve;
            LineRenderer.SetPosition(LineRenderer.positionCount - 1, endPosition);
        }

        public void DrawLineViaNetwork(int lineId, float lastDrawPointX, float lastDrawPointY, float lastDrawPointZ, float endPositionX,
            float endPositionY, float endPositionZ, float lineWidth, float lineColorR, float lineColorG, float lineColorB,
            float rotationX, float rotationY, float rotationZ, float rotationW)
        {
            Vector3 endPosition = new Vector3(endPositionX, endPositionY, endPositionZ);
            Quaternion rotation = new Quaternion(rotationX, rotationY, rotationZ, rotationW);

            Debug.Log("here");

            lastTransform = PhotonView.Find(lineId).transform;

            lastTransform.name = "DrawLine";
            lastTransform.parent = root;
            lastTransform.position = endPosition;
            lastTransform.rotation = rotation;
            lastTransform.gameObject.tag = "DrawLine";
            LineRenderer = lastTransform.gameObject.GetComponent<LineRenderer>();

            Color newColor = new Color(lineColorR, lineColorG, lineColorB, 1);
            LineRenderer.startColor = newColor;
            LineRenderer.endColor = newColor;
            LineRenderer.startWidth = lineWidth;
            LineRenderer.endWidth = lineWidth;
            var curve = new AnimationCurve();
            curve.AddKey(0, lineWidth);
            //curve.AddKey(1, lineWidth);
            LineRenderer.widthCurve = curve;
            if (DrawMaterial)
            {
                LineRenderer.material = DrawMaterial;
            }
            LineRenderer.numCapVertices = 5;
            //LineRenderer.alignment = LineAlignment.TransformZ;
            LineRenderer.useWorldSpace = true;

            Vector3 lastDrawPoint = new Vector3(lastDrawPointX, lastDrawPointY, lastDrawPointZ);

            LineRenderer.SetPosition(0, lastDrawPoint);
            LineRenderer.SetPosition(1, endPosition);
        }

        void OnDrawGizmosSelected()
        {
            // Show Grip Point
            Gizmos.color = Color.green;
            Gizmos.DrawLine(RaycastStart.position, RaycastStart.position + RaycastStart.up * RaycastLength);
        }
        [PunRPC]
        public void DeleteAll()
        {
            foreach (Transform child in root)
            {
                Destroy(child.gameObject);
            }
        }
        [PunRPC]
        public void ChangeFontSize()
        {
            LineWidth = fontSlider.value;
        }

        public void OnDeleteAllClicked()
        {
            DeleteAll();
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("DeleteAll", RpcTarget.AllBuffered);
        }
        public void OnChangeFontSizeClicked()
        {
            ChangeFontSize();
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("ChangeFontSize", RpcTarget.AllBuffered);
        }
    }

}



[System.Serializable]
public enum MarkerColor
{
    black,
    Blue,
    red
}