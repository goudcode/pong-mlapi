using MLAPI;
using MLAPI.NetworkVariable;


public class Player : NetworkBehaviour
{
    public NetworkVariableString playerName = new NetworkVariableString(new NetworkVariableSettings()
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.OwnerOnly
    });

    public NetworkVariableInt score = new NetworkVariableInt(new NetworkVariableSettings()
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.ServerOnly
    });

    public override void NetworkStart()
    {
        if (!IsOwner)
            return;

        playerName.Value = "Player";
    }
}
