using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongSnapManager : MonoBehaviour
{
    public GameObject centerEyeAnchor;
    public bool[] boolItems;
    public string[] names;

    private void Start()
    {
        centerEyeAnchor = GameObject.Find("CenterEyeAnchor");
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < names.Length; i++)
        {
            if (other.gameObject.name == names[i] && boolItems[i] == false)
            {
                centerEyeAnchor.transform.GetChild(0).gameObject.SetActive(true);
                StartCoroutine("HideWrongObject");
            }
        }
    }

    public IEnumerator HideWrongObject()
    {
        yield return new WaitForSeconds(0.5f);
        centerEyeAnchor.transform.GetChild(0).gameObject.SetActive(false);
    }
}