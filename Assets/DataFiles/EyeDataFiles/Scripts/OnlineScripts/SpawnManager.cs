using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace BNG
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] GameObject genericVRPlayerPrefab;
        [SerializeField] Vector3 spawnPosition;
        [SerializeField] GameObject player;

        void Start()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                print(">>>>>> instantiated Player <<<<<<<<" + PhotonNetwork.IsConnectedAndReady);
                player =
                    PhotonNetwork.Instantiate(genericVRPlayerPrefab.name, spawnPosition, Quaternion.identity);
                NetworkPlayer np = player.transform.GetChild(0).GetComponent<NetworkPlayer>();
                np.parentTablet = GameObject.Find("ParentTablet");
                if (np)
                {
                    np.transform.name = "MyRemotePlayer";
                    np.AssignPlayerObjects();
                }
            }
        }
    }
}