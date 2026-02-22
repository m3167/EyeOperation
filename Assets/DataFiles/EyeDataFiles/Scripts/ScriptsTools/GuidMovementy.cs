using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidMovementy : MonoBehaviour
{
    public float distance = 0.5f; // Adjust this to set the distance of the movement
    public float speed = 2f;      // Adjust this to control the movement speed

    private Vector3 startingPosition;

    void Start()
    {
        // Save the starting position of the object
        startingPosition = transform.position;
    }

    void Update()
    {
        // Move the object up and down in a loop
        float yPos = startingPosition.y + Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        transform.position = new Vector3(startingPosition.x, yPos, startingPosition.z);
    }
}
