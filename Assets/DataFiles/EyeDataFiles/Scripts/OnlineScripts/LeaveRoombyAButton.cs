using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class LeaveRoombyAButton : MonoBehaviour
{
    void Update()
    {
        if (InputBridge.Instance.AButtonDown || Input.GetKeyDown(KeyCode.Escape))
        {
            VirtualWorldManager.instance.LeaveRoomAndLoadHomeScene();
        }
    }
}