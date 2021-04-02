using Gameplay;
using MLAPI;
using MLAPI.SceneManagement;
using MLAPI.Transports.UNET;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Network
{
    public class GameSetup : MonoBehaviour
    {
        [SerializeField] private GameObject networkSetup;
        [SerializeField] private GameObject lobby;
        [SerializeField] private GameObject connectedPlayerPrefab;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private Button startButton;
        [SerializeField] private TMP_InputField ipInput;
        [SerializeField] private TMP_InputField playerNameInput;

        private static int port = 27017;
    
        public void OnHost()
        {
            NetworkManager.Singleton.OnServerStarted += ServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
            NetworkManager.Singleton.GetComponent<UNetTransport>().ServerListenPort = port;
            NetworkManager.Singleton.StartHost();
            ToggleAllInputsInNetworkSetup(false);
        }

        public void OnJoin()
        {
            UNetTransport transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
            transport.ConnectAddress = ipInput.text;
            transport.ConnectPort = port;

            NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisconnected;
            NetworkManager.Singleton.StartClient();
        }
    
        public void OnStart()
        {
            NetworkSceneManager.SwitchScene("Game");
        }

        private void ServerStarted()
        {
            ClientConnected(NetworkManager.Singleton.ServerClientId);
            SwitchToLobby();
            startButton.gameObject.SetActive(true);
        }

        private void ClientConnected(ulong id)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                startButton.interactable = NetworkManager.Singleton.ConnectedClientsList.Count == 2;
                return;
            }
            SwitchToLobby();
        }
    
        private void ClientDisconnected(ulong obj)
        {
            startButton.interactable = NetworkManager.Singleton.ConnectedClientsList.Count == 2;
        }
    
        private void SwitchToLobby()
        {
            networkSetup.SetActive(false);
            lobby.SetActive(true);
        }
    
        public void OnIpInputChanged()
        {
            joinButton.interactable = ipInput.text != "";
        }

        public void OnPlayerNameChanged()
        {
            var player = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject
                .GetComponent<Player>().playerName.Value = playerNameInput.text;
        }
    
        private void ToggleAllInputsInNetworkSetup(bool value)
        {
            hostButton.interactable = value;
            joinButton.interactable = value;
            ipInput.interactable = value;
        }
    }
}
