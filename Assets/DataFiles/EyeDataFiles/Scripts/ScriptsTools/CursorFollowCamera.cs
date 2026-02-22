using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorFollowCamera : MonoBehaviour
{
    // public RectTransform movingObject;
    // public RectTransform basisObject;
    // public Vector3 offset;
    // public Camera cam;
    //
    // public void Start()
    // {
    //     //Cursor.visible = false; 
    // }
    //
    // void Update()
    // {
    //     MoveObject();
    // }
    //
    // public void MoveObject()
    // {
    //     Vector3 pos = Input.mousePosition + offset;
    //     pos.z = basisObject.position.z;
    //     movingObject.position = cam.ScreenToWorldPoint(pos);
    // }
    
    public Canvas parentCanvas;
    public RawImage mouseCursor;

    public void Start()
    {
        Cursor.visible = false;
    }


    public void Update()
    {
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);

        Vector3 mousePos = parentCanvas.transform.TransformPoint(movePos);

        //Set fake mouse Cursor
        mouseCursor.transform.position = mousePos;

        //Move the Object/Panel
        transform.position = mousePos;
    }
}