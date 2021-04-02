using System;
using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.SceneManagement;
using TMPro;
using UnityEngine;

public class GameOverScreen : NetworkBehaviour
{
    [SerializeField] private GameObject restartButton;
    [SerializeField] private TextMeshProUGUI messageText;

    private NetworkVariableString message = new NetworkVariableString(new NetworkVariableSettings()
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.ServerOnly
    });
    
    public override void NetworkStart()
    {
        message.OnValueChanged += (_, value) => messageText.text = value;
        
        if (!IsHost)
            return;

        restartButton.SetActive(true);
        var player1 = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.ServerClientId].PlayerObject
            .GetComponent<Player>();
        var player2 = NetworkManager.Singleton.ConnectedClients.Last().Value.PlayerObject
            .GetComponent<Player>();

        message.Value =
            $"{(player1.score.Value > player2.score.Value ? player1.playerName.Value : player2.playerName.Value)} won";
    }

    public void OnRestart()
    {
        NetworkSceneManager.SwitchScene("Game");
    }
}
