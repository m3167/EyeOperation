using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ScrubBarTimer : MonoBehaviour
{
    [SerializeField] Text m_ScrubTime;
    [SerializeField] Slider m_ScrubBarSlider;

    private float m_CooldownTime = .5f;
    private float m_CurrentTimer = 0;
    public PhotonView _photonView;

    private void Start()
    {
        _photonView = PhotonView.Get(this);

        m_ScrubTime.text = "";
    }

    public void OnScrubBarSliderMoveButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("OnScrubBarSliderMove", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void OnScrubBarSliderMove()
    {
        float timeInSeconds = m_ScrubBarSlider.value;

        string minutes = Mathf.Floor(timeInSeconds / 60).ToString("00");
        string seconds = (timeInSeconds % 60).ToString("00");


        m_ScrubTime.text = minutes + " : " + seconds;

        m_CurrentTimer = m_CooldownTime;
    }

    public void Update()
    {
        if (m_CurrentTimer > 0)
        {
            m_CurrentTimer -= Time.deltaTime;

            if (m_CurrentTimer <= 0)
            {
                m_ScrubTime.text = "";
            }
        }
    }
}
