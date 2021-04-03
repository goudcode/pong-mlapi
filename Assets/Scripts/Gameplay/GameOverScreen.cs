using System.Collections;
using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.SceneManagement;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class GameOverScreen : NetworkBehaviour
    {
        [SerializeField] private GameObject restartButton;
        [SerializeField] private TextMeshProUGUI messageText;

        private string gameOverMessage;
    
        public override void NetworkStart()
        {
            if (!IsHost)
                return;

            restartButton.SetActive(true);
            var player1 = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.ServerClientId].PlayerObject
                .GetComponent<Player>();
            var player2 = NetworkManager.Singleton.ConnectedClients.Last().Value.PlayerObject
                .GetComponent<Player>();

            gameOverMessage =
                $"{(player1.score.Value > player2.score.Value ? player1.playerName.Value : player2.playerName.Value)} won";
            
            // This Rpc is not always being called on the connected client.
            // The server will always receive it.
            UpdateGameOverMessageClientRpc(gameOverMessage);
            
            // Delaying the rpc will update it on the connected client without issues.
            StartCoroutine(nameof(DelayedUpdateRpc));
        }

        private IEnumerator DelayedUpdateRpc()
        {
            yield return new WaitForSeconds(10);
            UpdateGameOverMessageClientRpc(gameOverMessage);
        }
        
        [ClientRpc]
        private void UpdateGameOverMessageClientRpc(string message)
        {
            messageText.text = message;
        }

        public void OnRestart()
        {
            NetworkSceneManager.SwitchScene("Game");
        }
    }
}
