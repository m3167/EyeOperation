using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WebSocketSharp;
using Random = UnityEngine.Random;

public class NetworkStudentManager : MonoBehaviourPunCallbacks
{
    [Header("Sign In UI Panel")] public GameObject Signin_UI_Panel;
    public InputField userName_InputField;
    public InputField password_InputField;

    [Header("Sign Up Menu UI Panel")] public GameObject signup_UI_Panel;

    [Header("Email Verification UI Panel")]
    public GameObject emailVerification_UI_Panel;

    [Header("Home Panel")] public GameObject home_UI_Panel;

    [Header("Start Session UI Panel")] public GameObject startSession_UI_Panel;

    [Header("Game Option UI Panel")] public GameObject gameOptions_UI_Panel;

    [Header("Create Session UI Panel")] public GameObject createSession_UI_Panel;
    public InputField sessionName_InputField;
    public InputField maxStudents_InputField;


    [Header("Inside Session UI Panel")] public GameObject insideSession_UI_Panel;
    public Text SessionInfo_Text;
    public GameObject studentListPrefab;
    public GameObject studentListParentContent;
    public GameObject startGameButton;

    [Header("Session List UI Panel")] public GameObject sessionList_UI_Panel;
    public GameObject sessionListEntryPrefab;
    public GameObject sessionListParentContent;

    [Header("Join Random Session UI Panel")]
    public GameObject joinRandomSession_UI_Panel;

    [Header("Join Session By Id UI Panel")]
    public GameObject joinSessionById_UI_Panel;

    public GameObject errorMessage;
    public InputField sessionID_InputField;

    [Header("Events UI Panel")] public GameObject events_UI_Panel;

    [Header("My Avatar UI Panel")] public GameObject myAvater_UI_Panel1;
    public GameObject myAvater_UI_Panel2;

    [Header("My Account Profile UI Panel")]
    public GameObject myAccountProfile_UI_Panel;

    [Header("Password UI Panel")] public GameObject password_UI_Panel;

    [Header("Setting UI Panel")] public GameObject setting_UI_Panel;

    [Header("Tutorial UI Panel")] public GameObject tutorial_UI_Panel;

    [Header("Logout UI Panel")] public GameObject logout_UI_Panel;

    public Dictionary<string, RoomInfo> cachedSessionList;
    public Dictionary<string, GameObject> sessionListGameObjects;

    public Dictionary<int, GameObject> studentListGameObjects;

    public bool joinWithMe, joinWithOthers;

    public static bool isStudentLeaveRoom;

    public static NetworkStudentManager instance;

    #region Unity Methods

    public void Start()
    {
        instance = this;
        cachedSessionList = new Dictionary<string, RoomInfo>();
        sessionListGameObjects = new Dictionary<string, GameObject>();
        PhotonNetwork.AutomaticallySyncScene = true;

        if (NetworkStudentManager.isStudentLeaveRoom == true)
        {
            Signin_UI_Panel.SetActive(false);
            home_UI_Panel.SetActive(true);
        }
    }

    #endregion

    # region UI Callbacks

    public void SignInButtonClicked()
    {
        if (!userName_InputField.text.IsNullOrEmpty() && !password_InputField.text.IsNullOrEmpty())
        {
            ActivatePanel("Home Panel");
            // The name come from backend after login 
            PhotonNetwork.LocalPlayer.NickName = userName_InputField.text;
        }
    }

    public void JoinWithOthersButtonClicked()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogError("You already connected to photon");
            ActivatePanel(gameOptions_UI_Panel.name);
        }
        else
        {
            Debug.LogError("You already not connected to photon");
            joinWithOthers = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void JoinWithMeButtonClicked()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogError("You already connected to photon");
            string roomName = "Session " + Random.Range(1000, 10000);
            RoomOptions roomOption = new RoomOptions();
            roomOption.MaxPlayers = 1;
            roomOption.IsVisible = false;
            PhotonNetwork.CreateRoom(roomName, roomOption);
        }
        else
        {
            Debug.LogError("You already not connected to photon");
            joinWithMe = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnBackGameOptionsClicked()
    {
        PhotonNetwork.Disconnect();
        ActivatePanel(startSession_UI_Panel.name);
        joinWithOthers = false;
    }

    public void OnCreatRoomButtonClicked()
    {
        string roomName = sessionName_InputField.text;

        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Session " + Random.Range(1000, 10000);
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)int.Parse(maxStudents_InputField.text);
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void OnShowRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        ActivatePanel(sessionList_UI_Panel.name);
    }

    public void OnBackRoomListsButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        ActivatePanel(gameOptions_UI_Panel.name);
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnJoinedRandomRoomButtonClicked()
    {
        ActivatePanel(joinRandomSession_UI_Panel.name);
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnShowJoinedRoomByIdButtonClick()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        ActivatePanel(joinSessionById_UI_Panel.name);
    }

    public void OnJoinRoomByIdButtonClicked()
    {
        if (!string.IsNullOrEmpty(sessionID_InputField.text))
        {
            string sessionID = sessionID_InputField.text;
            if (cachedSessionList.ContainsKey(sessionID))
            {
                OnJoinRoomButtonClicked(sessionID);
            }
            else
            {
                Debug.Log("There is no session with this id");
                StartCoroutine("ShowErrorMessage");
                errorMessage.GetComponent<Text>().text = "There is no session with this id";
            }
        }
        else
        {
            Debug.Log("Enter the session id");
            StartCoroutine("ShowErrorMessage");
            errorMessage.GetComponent<Text>().text = "Enter the session id";
        }
    }

    public IEnumerator ShowErrorMessage()
    {
        errorMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        errorMessage.SetActive(false);
    }

    public void OnStartGameButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("OnlineLargeEyeSurgery");
        }
    }
    
    
    #endregion

    # region Photon Callbacks

    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to photon Server");
        if (joinWithMe)
        {
            string roomName = "Session " + Random.Range(1000, 10000);
            RoomOptions roomOption = new RoomOptions();
            roomOption.MaxPlayers = 1;
            roomOption.IsVisible = false;
            PhotonNetwork.CreateRoom(roomName, roomOption);
        }

        else if (joinWithOthers)
        {
            ActivatePanel(gameOptions_UI_Panel.name);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + "is created");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + "is joined to " + PhotonNetwork.CurrentRoom.Name);
        ActivatePanel(insideSession_UI_Panel.name);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }

        //Update room info text
        SessionInfo_Text.text = "Session name: " + PhotonNetwork.CurrentRoom.Name + "   " +
                                "Students/Max.Students: " + PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                                PhotonNetwork.CurrentRoom.MaxPlayers;

        if (studentListGameObjects == null)
        {
            studentListGameObjects = new Dictionary<int, GameObject>();
        }

        //Instantiated player list 
        foreach (var player in PhotonNetwork.PlayerList)
        {
            GameObject studentListGameObject = Instantiate(studentListPrefab);
            studentListGameObject.transform.SetParent(studentListParentContent.transform);
            studentListGameObject.transform.localPosition =
                new Vector3(studentListGameObject.transform.localPosition.x,
                    studentListGameObject.transform.localPosition.y, 0);
            studentListGameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            studentListGameObject.transform.localScale = Vector3.one;

            //Fill room list prefab with name and num of players in this room
            studentListGameObject.transform.Find("StudentNameText").GetComponent<Text>().text =
                player.NickName;

            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                studentListGameObject.transform.Find("StudentIndicator").gameObject.SetActive(true);
            }
            else
            {
                studentListGameObject.transform.Find("StudentIndicator").gameObject.SetActive(false);
            }

            studentListGameObjects.Add(player.ActorNumber, studentListGameObject);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Update room info text
        SessionInfo_Text.text = "Session name: " + PhotonNetwork.CurrentRoom.Name + "   " +
                                "Students/Max.Students: " + PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                                PhotonNetwork.CurrentRoom.MaxPlayers;

        GameObject studentListGameObject = Instantiate(studentListPrefab);
        studentListGameObject.transform.SetParent(studentListParentContent.transform);
        studentListGameObject.transform.localPosition =
            new Vector3(studentListGameObject.transform.localPosition.x,
                studentListGameObject.transform.localPosition.y, 0);
        studentListGameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        studentListGameObject.transform.localScale = Vector3.one;

        //Fill room list prefab with name and num of players in this room
        studentListGameObject.transform.Find("StudentNameText").GetComponent<Text>().text =
            newPlayer.NickName;

        if (newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            studentListGameObject.transform.Find("StudentIndicator").gameObject.SetActive(true);
        }
        else
        {
            studentListGameObject.transform.Find("StudentIndicator").gameObject.SetActive(false);
        }

        studentListGameObjects.Add(newPlayer.ActorNumber, studentListGameObject);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Update room info text
        SessionInfo_Text.text = "Session name: " + PhotonNetwork.CurrentRoom.Name + "   " +
                                "Students/Max.Students: " + PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                                PhotonNetwork.CurrentRoom.MaxPlayers;

        Destroy(studentListGameObjects[otherPlayer.ActorNumber]);
        studentListGameObjects.Remove(otherPlayer.ActorNumber);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
    }

    public override void OnLeftRoom()
    {
        if (joinWithMe == true)
        {
            ActivatePanel(startSession_UI_Panel.name);
            PhotonNetwork.Disconnect();
            joinWithMe = false;
        }
        else if (joinWithOthers == true)
        {
            ActivatePanel(gameOptions_UI_Panel.name);
        }

        foreach (GameObject playerListGameobject in studentListGameObjects.Values)
        {
            Destroy(playerListGameobject);
        }

        studentListGameObjects.Clear();
        studentListGameObjects = null;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);

            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (cachedSessionList.ContainsKey(room.Name))
                {
                    cachedSessionList.Remove(room.Name);
                }
            }
            else
            {
                //Update cache room list 
                if (cachedSessionList.ContainsKey(room.Name))
                {
                    cachedSessionList[room.Name] = room;
                }
                //Add the new room to the cache room list
                else
                {
                    cachedSessionList.Add(room.Name, room);
                }
            }
        }

        foreach (RoomInfo room in cachedSessionList.Values)
        {
            GameObject sessionListEntryGameObject = Instantiate(sessionListEntryPrefab);
            sessionListEntryGameObject.transform.SetParent(sessionListParentContent.transform);
            sessionListEntryGameObject.transform.localPosition =
                new Vector3(sessionListEntryGameObject.transform.localPosition.x,
                    sessionListEntryGameObject.transform.localPosition.y, 0);
            sessionListEntryGameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            sessionListEntryGameObject.transform.localScale = Vector3.one;

            //Fill room list prefab with name and num of players in this room
            sessionListEntryGameObject.transform.Find("SessionNameText").GetComponent<Text>().text = room.Name;
            sessionListEntryGameObject.transform.Find("SessionsStudentText").GetComponent<Text>().text
                = room.PlayerCount + " / " + room.MaxPlayers;
            /*sessionListEntryGameObject.transform.Find("OwnerNameText").GetComponent<Text>().text
                = //Name of owner Room*/
            sessionListEntryGameObject.transform.Find("JoinSessionButton").GetComponent<Button>().onClick
                .AddListener(() => OnJoinRoomButtonClicked(room.Name));

            sessionListGameObjects.Add(room.Name, sessionListEntryGameObject);
        }
    }

    public override void OnLeftLobby()
    {
        ClearRoomListView();
        cachedSessionList.Clear();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        string roomName = "Session " + Random.Range(1000, 10000);
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 20;
        PhotonNetwork.CreateRoom(roomName, roomOption);
    }

    #endregion

    #region Public Method

    public void ActivatePanel(string panelToActivated)
    {
        Signin_UI_Panel.SetActive(panelToActivated.Equals(Signin_UI_Panel.name));
        signup_UI_Panel.SetActive(panelToActivated.Equals(signup_UI_Panel.name));
        emailVerification_UI_Panel.SetActive(panelToActivated.Equals(emailVerification_UI_Panel.name));
        home_UI_Panel.SetActive(panelToActivated.Equals(home_UI_Panel.name));
        startSession_UI_Panel.SetActive(panelToActivated.Equals(startSession_UI_Panel.name));
        gameOptions_UI_Panel.SetActive(panelToActivated.Equals(gameOptions_UI_Panel.name));
        createSession_UI_Panel.SetActive(panelToActivated.Equals(createSession_UI_Panel.name));
        insideSession_UI_Panel.SetActive(panelToActivated.Equals(insideSession_UI_Panel.name));
        sessionList_UI_Panel.SetActive(panelToActivated.Equals(sessionList_UI_Panel.name));
        joinRandomSession_UI_Panel.SetActive(panelToActivated.Equals(joinRandomSession_UI_Panel.name));
        joinSessionById_UI_Panel.SetActive(panelToActivated.Equals(joinSessionById_UI_Panel.name));
        events_UI_Panel.SetActive(panelToActivated.Equals(events_UI_Panel.name));
        myAvater_UI_Panel1.SetActive(panelToActivated.Equals(myAvater_UI_Panel1.name));
        myAvater_UI_Panel2.SetActive(panelToActivated.Equals(myAvater_UI_Panel2.name));
        myAccountProfile_UI_Panel.SetActive(panelToActivated.Equals(myAccountProfile_UI_Panel.name));
        password_UI_Panel.SetActive(panelToActivated.Equals(password_UI_Panel.name));
        setting_UI_Panel.SetActive(panelToActivated.Equals(setting_UI_Panel.name));
        tutorial_UI_Panel.SetActive(panelToActivated.Equals(tutorial_UI_Panel.name));
        logout_UI_Panel.SetActive(panelToActivated.Equals(logout_UI_Panel.name));
    }

    #endregion

    #region private Method

    public void OnJoinRoomButtonClicked(string _roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinRoom(_roomName);
    }

    public void ClearRoomListView()
    {
        foreach (GameObject roomListGameobject in sessionListGameObjects.Values)
        {
            Destroy(roomListGameobject);
        }

        sessionListGameObjects.Clear();
    }

    #endregion
}