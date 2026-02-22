using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Photon.Pun;

public class VideoManager : MonoBehaviour
{
    [SerializeField] Sprite m_PlaySprite;
    [SerializeField] Sprite m_PauseSprite;

    [SerializeField] Image m_PlayPauseBG;
    [SerializeField] public VideoPlayer m_VideoPlayer;

    [SerializeField] public List<VideoClip> m_VideoClips;
    public int m_VideoClipIndex = 0;

    [SerializeField] Text m_CurrentTime;
    [SerializeField] Text m_TotalTime;

    [SerializeField] Slider m_CurrentTimeSlider;
    [SerializeField] public Slider m_InteractiveSlider;

    [SerializeField] RawImage m_RawImage;
    [SerializeField] RenderTexture m_RenderTexture;

    [SerializeField] Slider m_VolumeSlider;

    // SHUFFLE //
    [SerializeField] Image m_ShuffleImage;
    [SerializeField] Color32 m_Shuffle_Enabled;
    [SerializeField] Color32 m_Shuffle_Disabled;
    private bool m_Shuffle = false;

    // Loop
    [SerializeField] Image m_LoopImage;
    [SerializeField] Color32 m_Loop_Enabled;
    [SerializeField] Color32 m_Loop_Disabled;
    private bool m_Loop = false;

    // Autoplay //
    [SerializeField] bool m_AutoPlay = false;
    [SerializeField] bool m_LandscapeMode = false;

    // MUTE //
    [SerializeField] Image m_SoundImage;
    [SerializeField] Sprite m_SoundOn;
    [SerializeField] Sprite m_SoundOff;

    public PhotonView _photonView;

    public void Start()
    {
        _photonView = PhotonView.Get(this);

        m_CurrentTimeSlider.value = 0;
        m_InteractiveSlider.value = 0;
        m_VideoPlayer.loopPointReached += VideoComplete;
        // if (m_LandscapeMode)
        // {
        //     _photonView.RPC("OnClick_FlipOrientation", RpcTarget.AllBuffered);
        // }


        // additional fool proofing
        if (m_VideoClips.Count == 0)
        {
            Debug.LogError("Please add videos - One ore more videoClips are missing, be sure to add videos and add them to videomanager.cs in the inspector window of gameobeject VideoManager, also please refer to README");
            return;
        }

        for (int i = 0; i < m_VideoClips.Count; i++)
        {
            if (m_VideoClips[i] == null)
            {
                Debug.LogError("One ore more videoClips are missing, be sure to add videos and add them to videomanager.cs in the inspector window of VideoManager, also please refer to README");
                return;
            }
        }

        //m_VideoPlayer.clip = m_VideoClips[0];
    }

    public void Update()
    {
        if (m_VideoPlayer.isPlaying)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _photonView.RPC("SetCurrentTimeUI", RpcTarget.AllBuffered);
                _photonView.RPC("MovePlayHead", RpcTarget.AllBuffered);
            }
        }
    }

    public void OnUpdateVolumeButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("UpdateVolume", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// Listening to slider update event - attached through inspector
    /// </summary>
    [PunRPC]
    public void UpdateVolume()
    {
        // Volume = m_VolumeSlider.value;
        m_VideoPlayer.SetDirectAudioVolume(0, m_VolumeSlider.value);
    }

    public void OnClickShuffleButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("OnClick_Shuffle", RpcTarget.AllBuffered);
        }
    }

    // enable shuffling
    [PunRPC]
    public void OnClick_Shuffle()
    {
        m_Shuffle = !m_Shuffle;

        switch (m_Shuffle)
        {
            case true:
                m_ShuffleImage.color = m_Shuffle_Enabled;
                break;
            case false:
                m_ShuffleImage.color = m_Shuffle_Disabled;
                break;
        }
    }

    public void OnClickLoopButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("OnClick_Loop", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void OnClick_Loop()
    {
        m_Loop = !m_Loop;

        switch (m_Loop)
        {
            case true:
                m_LoopImage.color = m_Loop_Enabled;
                break;
            case false:
                m_LoopImage.color = m_Loop_Disabled;
                break;
        }
    }

    public void OnClickToggleMuteButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("OnClick_ToggleMute", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void OnClick_ToggleMute()
    {
        bool isMute = m_VideoPlayer.GetDirectAudioMute(0);

        isMute = !isMute;   // flip/toggle mute

        switch (isMute)
        {
            case true:
                m_SoundImage.sprite = m_SoundOff;
                break;
            case false:
                m_SoundImage.sprite = m_SoundOn;
                break;
        }


        m_VideoPlayer.SetDirectAudioMute(0, isMute);
    }

    // public void OnClickFlipOrientationButtonClicked()
    // {
    //     if (PhotonNetwork.IsMasterClient)
    //     {
    //         _photonView.RPC("OnClick_FlipOrientation", RpcTarget.AllBuffered);
    //     }
    // }

    // [PunRPC]
    // public void OnClick_FlipOrientation()
    // {
    //     switch (Screen.orientation)
    //     {
    //         case ScreenOrientation.LandscapeLeft:
    //             Screen.orientation = ScreenOrientation.Portrait;
    //             _photonView.RPC("FlipRenderTexture", RpcTarget.AllBuffered);
    //             break;
    //         case ScreenOrientation.Portrait:
    //             Screen.orientation = ScreenOrientation.LandscapeLeft;
    //             _photonView.RPC("FlipRenderTexture", RpcTarget.AllBuffered);
    //             break;
    //     }
    // }

    // [PunRPC]
    // public void FlipRenderTexture()
    // {
    //     // ****** FLIPPING
    //     int width = m_RenderTexture.height;
    //     int height = m_RenderTexture.width;


    //     m_RenderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
    //     m_RawImage.texture = m_RenderTexture;
    //     m_VideoPlayer.targetTexture = m_RenderTexture;
    // }

    public void OnClickPlayPauseButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("OnClick_PlayPause", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void OnClick_PlayPause()
    {
        if (m_VideoPlayer.isPlaying)
        {
            m_VideoPlayer.Pause();
            _photonView.RPC("SetIsPlayingSprite", RpcTarget.AllBuffered, false);
        }
        else
        {
            m_VideoPlayer.Play();
            _photonView.RPC("SetIsPlayingSprite", RpcTarget.AllBuffered, true);
            _photonView.RPC("SetTotalTimeUI", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isActive"></param>
    [PunRPC]
    public void SetIsPlayingSprite(bool isActive)
    {
        m_PlayPauseBG.sprite = (isActive) ? m_PauseSprite : m_PlaySprite;
    }

    [PunRPC]
    public void ShuffleIndex()
    {

        while (true)
        {
            int randomIndex = Random.Range(0, m_VideoClips.Count);

            if (randomIndex != m_VideoClipIndex)
            {
                m_VideoClipIndex = randomIndex;
                return;
            }
        }
    }

    [PunRPC]
    public void IncrementIndex()
    {
        m_VideoClipIndex++;

        if (m_VideoClipIndex >= m_VideoClips.Count)
        {
            m_VideoClipIndex = m_VideoClipIndex % m_VideoClips.Count;
        }
    }

    [PunRPC]
    public void DecrementIndex()
    {
        m_VideoClipIndex--;

        if (m_VideoClipIndex < 0)
        {
            m_VideoClipIndex = m_VideoClips.Count - 1;
        }
    }

    public void OnClickNextButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("OnClick_Next", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void OnClick_Next()
    {
        m_VideoPlayer.Stop();

        m_CurrentTimeSlider.value = 0;
        m_InteractiveSlider.value = 0;

        if (m_Shuffle)
            _photonView.RPC("ShuffleIndex", RpcTarget.AllBuffered);

        if (!m_Shuffle)
            _photonView.RPC("IncrementIndex", RpcTarget.AllBuffered);

        m_VideoPlayer.clip = m_VideoClips[m_VideoClipIndex];

        _photonView.RPC("SetTotalTimeUI", RpcTarget.AllBuffered);

        m_VideoPlayer.Play();

    }

    public void OnClickPreviousButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("OnClick_Previous", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void OnClick_Previous()
    {
        m_VideoPlayer.Stop();

        m_CurrentTimeSlider.value = 0;
        m_InteractiveSlider.value = 0;

        if (m_Shuffle)
        {
            _photonView.RPC("ShuffleIndex", RpcTarget.AllBuffered);
        }

        if (!m_Shuffle)
        {
            _photonView.RPC("DecrementIndex", RpcTarget.AllBuffered);
        }

        m_VideoPlayer.clip = m_VideoClips[m_VideoClipIndex];

        _photonView.RPC("SetTotalTimeUI", RpcTarget.AllBuffered);

        m_VideoPlayer.Play();
    }

    /// <summary>
    /// Method to skip a few seconds
    /// </summary>
    /// <param name="seconds"></param>
    public void OnClickSkipAFewSecondsButtonClicked(float seconds = 5)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("OnClick_SkipAFewSeconds", RpcTarget.AllBuffered, seconds);
        }
    }

    [PunRPC]
    public void OnClick_SkipAFewSeconds(float seconds = 5)
    {
        // skip forward in time
        if (seconds > 0)
        {
            if (m_CurrentTimeSlider.value + seconds > m_CurrentTimeSlider.maxValue)
                m_VideoPlayer.time = m_CurrentTimeSlider.maxValue;
            else
                m_VideoPlayer.time = m_CurrentTimeSlider.value + seconds;
        }

        // skip backwards in time
        if (seconds < 0)
        {
            if (m_CurrentTimeSlider.value - seconds <= 0)
                m_VideoPlayer.time = m_CurrentTimeSlider.minValue;
            else
                m_VideoPlayer.time = m_CurrentTimeSlider.value + seconds;
        }

        if (seconds == 0.0f)
        {
            Debug.LogError("past in seconds should be equal to 0");
        }
    }

    public void OnClickResetButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("OnClick_Reset", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void OnClick_Reset()
    {
        m_CurrentTimeSlider.value = 0;
        m_VideoPlayer.Stop();
        m_VideoPlayer.Play();
    }

    [PunRPC]
    public void SetCurrentTimeUI()
    {
        string minutes = Mathf.Floor((int)m_VideoPlayer.time / 60).ToString("00");
        string seconds = ((int)m_VideoPlayer.time % 60).ToString("00");

        m_CurrentTime.text = minutes + " : " + seconds;
    }

    [PunRPC]
    public void SetTotalTimeUI()
    {
        string minutes = Mathf.Floor((int)m_VideoPlayer.clip.length / 60).ToString("00");
        string seconds = ((int)m_VideoPlayer.clip.length % 60).ToString("00");

        m_TotalTime.text = minutes + " : " + seconds;
    }

    [PunRPC]
    public void MovePlayHead()
    {
        m_CurrentTimeSlider.value = (float)m_VideoPlayer.time;
        m_CurrentTimeSlider.maxValue = (float)m_VideoPlayer.clip.length;

        m_InteractiveSlider.maxValue = Mathf.Floor((int)m_VideoPlayer.clip.length);

    }

    public void OnClickScrubBarHeadMoveButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("ScrubBarHeadMove", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void ScrubBarHeadMove()
    {

        m_CurrentTimeSlider.value = m_InteractiveSlider.value;
        m_VideoPlayer.time = m_InteractiveSlider.value;


        if (!m_VideoPlayer.isPlaying)
        {

            m_VideoPlayer.Play();

            m_PlayPauseBG.sprite = m_PauseSprite;
        }
    }

    [PunRPC]
    public void VideoComplete(UnityEngine.Video.VideoPlayer vp)
    {
        if (m_AutoPlay)
        {
            _photonView.RPC("SetIsPlayingSprite", RpcTarget.AllBuffered, false);

            Debug.Log("VideoComplete");
            m_VideoPlayer.Stop();

            if (m_Shuffle)
            {
                _photonView.RPC("ShuffleIndex", RpcTarget.AllBuffered);
            }

            if (!m_Shuffle)
            {
                if (m_VideoClipIndex + 1 >= m_VideoClips.Count && !m_Loop)
                {
                    // if we reached the end of the line - i
                    Debug.Log("Reached the end");
                    return;
                }

                _photonView.RPC("IncrementIndex", RpcTarget.AllBuffered);
            }

            m_CurrentTimeSlider.value = 0;

            m_InteractiveSlider.value = 0;

            m_VideoPlayer.clip = m_VideoClips[m_VideoClipIndex];

            _photonView.RPC("SetTotalTimeUI", RpcTarget.AllBuffered);

            m_VideoPlayer.Play();

            _photonView.RPC("SetIsPlayingSprite", RpcTarget.AllBuffered, true);

            Debug.Log("Play");
        }
    }
}
