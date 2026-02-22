using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class HandleTablet : MonoBehaviour
{
    public GameObject localPlayer;
    public GameObject[] remotesPlayer;


    private void Update()
    {
        print("Handle Tablet");
        
        localPlayer = GameObject.Find("MyRemotePlayer");
        if (localPlayer != null)
        {
            localPlayer.GetComponent<PlayerTabletManager>().enabled = true;
        }

        remotesPlayer = GameObject.FindObjectsOfType<GameObject>();
        if (remotesPlayer != null)
        {
            // Loop through the found objects and do something
            foreach (GameObject obj in remotesPlayer)
            {
                if (obj.name == "RemotePlayer")
                {
                    // Do something with the object
                    Debug.Log("Found object with name: " + obj.name);
                    obj.GetComponent<PlayerTabletManager>().enabled = false;
                }
            }
        }
    }
}