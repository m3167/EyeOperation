using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (transform.position.y < 1.37019f)
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
                transform.GetChild(0).GetComponent<PlayerGravity>().GravityEnabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (transform.position.y > 0.3494981f)
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
                transform.GetChild(0).GetComponent<PlayerGravity>().GravityEnabled = false;
            }
        }
    }
}