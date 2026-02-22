using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeyControlsManager : MonoBehaviour
{
    public GameObject hotkeyWindowsControls;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor
            ||Application.platform == RuntimePlatform.WindowsPlayer)
        {
            hotkeyWindowsControls.SetActive(true);
        }
    }

    public void HideHotkeyWindowsControl()
    {
        hotkeyWindowsControls.SetActive(false);
    }
}
