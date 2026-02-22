using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CheckFile : MonoBehaviour
{
    public string urlFile;

    public void CheckFileButton()
    {
        WebFileExists(urlFile);
    }

    static public bool WebFileExists(string uri)
    {
        long fileLength = -1;
        WebRequest request = HttpWebRequest.Create(uri);
        request.Method = "HEAD";
        WebResponse resp = null;
        try
        {
            resp = request.GetResponse();
        }
        catch
        {
            resp = null;
        }

        if (resp != null)
        {
            long.TryParse(resp.Headers.Get("Content-Length"), out fileLength);
            Debug.Log("File is Removed");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }
        else
        {
            Debug.Log("File is Existed");
        }

        return fileLength > 0;
    }
}