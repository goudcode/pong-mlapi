using System.Collections;
using System.Linq;
using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine;

public class LobbyNameManager : NetworkBehaviour
{
    [SerializeField] private GameObject lobbyContainer;
    [SerializeField] private GameObject lobbyNamePrefab;
    
    public NetworkVariable<string[]> names = new NetworkVariable<string[]>(new NetworkVariableSettings()
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.OwnerOnly
    });
    
    // Start is called before the first frame update
    public override void NetworkStart()
    {
        if (IsHost)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += UpdateNames;
            NetworkManager.Singleton.OnClientDisconnectCallback += UpdateNames;
            UpdateNames();
        }

        StartCoroutine(nameof(UpdateLobbyContainer));
    }

    // Executed on all clients, gets the names form the synced names network variable that's set by the server
    // Only the updating of names is executed by the server
    private IEnumerator UpdateLobbyContainer()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(1);
            
            if (IsHost)
                UpdateNames();
                
            for(var i = lobbyContainer.transform.childCount-1; i >= 0; --i)
                Destroy(lobbyContainer.transform.GetChild(i).gameObject);

            foreach (var playerName in names.Value)
            {
                var entry = Instantiate(lobbyNamePrefab, lobbyContainer.transform); 
                entry.GetComponent<LobbyEntry>().ChangeName(playerName);
            }
        }
    }
    
    private void UpdateNames(ulong id)
        => UpdateNames();

    private void UpdateNames()
    {
        var players = NetworkManager.Singleton.ConnectedClientsList.Select(client =>
            client.PlayerObject.GetComponent<Player>());

        names.Value = players.Select(p => $"{p.playerName.Value}{(p.IsOwnedByServer ? " (host)" : "")}").ToArray();
    }
}
