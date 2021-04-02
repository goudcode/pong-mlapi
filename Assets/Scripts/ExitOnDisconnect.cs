using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class ExitOnDisconnect : MonoBehaviour
{
    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += _ => Application.Quit();
    }
}
