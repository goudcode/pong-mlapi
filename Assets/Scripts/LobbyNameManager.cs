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
        if (IsOwner)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += UpdateNames;
            NetworkManager.Singleton.OnClientDisconnectCallback += UpdateNames;
            UpdateNames();
        }

        StartCoroutine(nameof(UpdateLobbyContainer));
    }

    private IEnumerator UpdateLobbyContainer()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(1);
            
            for(var i = lobbyContainer.transform.childCount-1; i >= 0; --i)
                Destroy(lobbyContainer.transform.GetChild(i).gameObject);

            foreach (var name in names.Value)
            {
                var entry = Instantiate(lobbyNamePrefab, lobbyContainer.transform);
            }
        }
    }
    
    private void UpdateNames(ulong id)
        => UpdateNames();

    private void UpdateNames()
    {
        var playerNames = NetworkManager.Singleton.ConnectedClientsList.Select(client =>
            client.PlayerObject.GetComponent<Player>()
                .playerName.Value);
        names.Value = playerNames.ToArray();
    }
}
