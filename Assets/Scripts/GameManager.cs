using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using MLAPI.Transports.UNET;
using TMPro;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private TextMeshProUGUI scoreText;
    private bool gameStarted = false;
    private int port = 27017;

    private int scorePlayer1;
    private int scorePlayer2;

    private string ipField;

    public void AddPoint(bool player1)
    {
        if (player1)
            scorePlayer1++;
        else
            scorePlayer2++;
        
        UpdateScoreClientRpc(scorePlayer1, scorePlayer2);
    }

    [ClientRpc]
    private void UpdateScoreClientRpc(int player1, int player2)
    {
        scorePlayer1 = player1;
        scorePlayer2 = player2;
        scoreText.text = $"{scorePlayer1} : {scorePlayer2}";
    }
    
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            StartButtons();
        else
            StatusLabel();
        
        GUILayout.EndArea();
    }

    private void StartButtons()
    {
        ipField = GUILayout.TextField(ipField);
        if (GUILayout.Button("Host"))
        {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ServerListenPort = port;
            NetworkManager.Singleton.StartHost();
        }

        if (GUILayout.Button("Client"))
        {
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectPort = port;
            NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = ipField;
            NetworkManager.Singleton.StartClient();
        }
    }

    private void StatusLabel()
    {
        var mode = NetworkManager.Singleton.IsClient ? "Client" : "Server";
        GUILayout.Label($"Transport: {NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name}");
        GUILayout.Label($"Mode: {mode}");

        if (!gameStarted && NetworkManager.Singleton.IsHost)
        {
            if (GUILayout.Button("Start game"))
            {
                gameStarted = true;
                var ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
                ball.GetComponent<NetworkObject>().Spawn();
                ball.GetComponent<BallController>().Launch();
            }
        }
    }
}
