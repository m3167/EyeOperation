using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager instance;

    public void Start()
    {
        instance = this;
    }
    public void QuitApplication()
    {
        Application.Quit();
    }
}
