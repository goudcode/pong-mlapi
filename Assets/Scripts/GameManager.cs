using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.SceneManagement;
using TMPro;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private GameObject paddlePrefab;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private TextMeshProUGUI scoreText;

    private Player player1;
    private Player player2;

    public override void NetworkStart()
    {
        // Game initialisation should only be executed by host
        if (!IsHost)
            return;
        
        player1 = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.ServerClientId].PlayerObject
            .GetComponent<Player>();
        player2 = NetworkManager.Singleton.ConnectedClients.Last().Value.PlayerObject
            .GetComponent<Player>();
            
        player1.score.Value = 0;
        player2.score.Value = 0;
            
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

    public void AddPoint(bool isPlayer1)
    {
        if (isPlayer1)
            player1.score.Value++;
        else
            player2.score.Value++;

        UpdateScoreClientRpc(player1.score.Value, player2.score.Value);
        
        if (player1.score.Value >= 5 || player2.score.Value >= 5)
            NetworkSceneManager.SwitchScene("GameOver");
    }

    [ClientRpc]
    private void UpdateScoreClientRpc(int player1Score, int player2Score)
        => scoreText.text = $"{player1Score} : {player2Score}";
}
