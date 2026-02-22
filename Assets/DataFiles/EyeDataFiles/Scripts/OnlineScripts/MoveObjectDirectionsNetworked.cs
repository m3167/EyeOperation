using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveObjectDirectionsNetworked : MonoBehaviourPun
{
     public Transform objectToMoveUpDown; // Reference to the object you want to move in up or down directions
    public Transform objectToMoveRightLeft; // Reference to the object you want to move in right or left directions
    public float moveSpeed = 2.0f; // Speed of the movement
    public float upperLimit = 0.0f; // Upper limit for the object's position
    public float lowerLimit = 0.0f; // Lower limit for the object's position
    public float rightLimit = 0.0f; // right limit for the object's position
    public float leftLimit = 0.0f; // left limit for the object's position

    // Function to move the object up
    public void MoveUp()
    {
        photonView.RPC("Rpc_MoveUp",RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Rpc_MoveUp()
    {
        if (objectToMoveUpDown != null && objectToMoveUpDown.localPosition.z < upperLimit)
        {
            // Move the object upwards within the upper limit
            objectToMoveUpDown.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Object to move is not assigned or reached upper limit!");
        }
    }

    // Function to move the object down
    public void MoveDown()
    {
        photonView.RPC("Rpc_MoveDown",RpcTarget.AllBuffered);
    }
    
    [PunRPC]
    public void Rpc_MoveDown()
    {
        if (objectToMoveUpDown != null && objectToMoveUpDown.localPosition.z > lowerLimit)
        {
            // Move the object downwards within the lower limit
            objectToMoveUpDown.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Object to move is not assigned or reached lower limit!");
        }
    }

    // Function to move the object right
    public void MoveRight()
    {
        photonView.RPC("Rpc_MoveRight",RpcTarget.AllBuffered);
    }
    
    [PunRPC]
    public void Rpc_MoveRight()
    {
        if (objectToMoveRightLeft != null && objectToMoveRightLeft.localPosition.x < rightLimit)
        {
            // Move the object right within the right limit
            objectToMoveRightLeft.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Object to move is not assigned or reached right limit!");
        }
    }

    // Function to move the object left
    public void MoveLeft()
    {
        photonView.RPC("Rpc_MoveLeft",RpcTarget.AllBuffered);
    }
    
    [PunRPC]
    public void Rpc_MoveLeft()
    {
        if (objectToMoveRightLeft != null && objectToMoveRightLeft.localPosition.x > leftLimit)
        {
            print("moveLeft");
            // Move the object left within the left limit
            objectToMoveRightLeft.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Object to move is not assigned or reached left limit!");
        } 
    }
}
