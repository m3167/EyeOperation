using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorManager : MonoBehaviour
{
    public GameObject player;
    public Vector3 posDown, posUp;
    public Quaternion rotDown, rotUp;


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ElvatorDown")
        {
            Debug.Log("Elevator down is collide with player");
            StartCoroutine("MoveDown");
        }

        else if (other.tag == "ElvatorUp")
        {
            Debug.Log("Elevator down is collide with player");
            StartCoroutine("MoveUp");
        }
    }

    public IEnumerator MoveDown()
    {
        player.transform.GetComponent<PlayerGravity>().enabled = false;
        FadeScreen.instance.FadeScreenActive();
        yield return new WaitForSeconds(0.5f);
        player.transform.localPosition = posDown;
        player.transform.localRotation = rotDown;
        yield return new WaitForSeconds(1f);
        FadeScreen.instance.FadeScreenDisActive();
        player.transform.GetComponent<PlayerGravity>().enabled = true;
    }

    public IEnumerator MoveUp()
    {
        player.transform.GetComponent<PlayerGravity>().enabled = false;
        FadeScreen.instance.FadeScreenActive();
        yield return new WaitForSeconds(0.5f);
        player.transform.localPosition = posUp;
        player.transform.localRotation = rotUp;
        yield return new WaitForSeconds(1f);
        FadeScreen.instance.FadeScreenDisActive();
        player.transform.GetComponent<PlayerGravity>().enabled = true;
    }
}