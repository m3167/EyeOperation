using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetModelWithoutTween : MonoBehaviour
{
    public Vector3 originalPos;
    public Quaternion originalRot;


    private void Start()
    {
        originalPos = gameObject.transform.localPosition;
        originalRot = gameObject.transform.localRotation;
    }

    public void OnResetPositionRotationClicked()
    {
        gameObject.transform.localPosition = originalPos;
        gameObject.transform.localRotation = originalRot;
    }
}