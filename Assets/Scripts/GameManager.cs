using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.SceneManagement;
using MLAPI.Spawning;
using MLAPI.Transports.UNET;
using TMPro;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject paddlePrefab;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int scorePlayer1;
    private int scorePlayer2;

    public override void NetworkStart()
    {
        if (IsHost)
            StartGame();
    }

    private void StartGame()
    {
        var firstPlayer = Instantiate(paddlePrefab, new Vector3(-5, 0, 0), Quaternion.identity);
        var firstPlayerNetworkObject = firstPlayer.GetComponent<NetworkObject>();
        firstPlayerNetworkObject.Spawn(destroyWithScene: true);
        
        var secondPlayer = Instantiate(paddlePrefab, new Vector3(5, 0, 0), Quaternion.identity);
        var secondPlayerNetworkObject = secondPlayer.GetComponent<NetworkObject>();
        secondPlayerNetworkObject.SpawnWithOwnership(NetworkManager.Singleton.ConnectedClients.Last().Key, destroyWithScene: true);

        var ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        ball.GetComponent<NetworkObject>().Spawn(destroyWithScene: true);
        ball.GetComponent<BallController>().Launch();
    }

    public void AddPoint(bool player1)
    {
        if (player1)
            scorePlayer1++;
        else
            scorePlayer2++;

        UpdateScoreClientRpc(scorePlayer1, scorePlayer2);
        
        if (scorePlayer1 >= 5 || scorePlayer2 >= 5)
            NetworkSceneManager.SwitchScene("GameOver");
    }

    [ClientRpc]
    private void UpdateScoreClientRpc(int player1, int player2)
    {
        scorePlayer1 = player1;
        scorePlayer2 = player2;
        scoreText.text = $"{scorePlayer1} : {scorePlayer2}";
    }
}
