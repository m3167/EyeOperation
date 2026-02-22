using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLastSceneManager : MonoBehaviour
{
    public static int lastloadedsceneIndex;

    void Awake()
    {
        SaveLastSceneManager.lastloadedsceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
}