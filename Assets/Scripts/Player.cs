using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public NetworkVariable<string> playerName = new NetworkVariable<string>(new NetworkVariableSettings()
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.OwnerOnly
    });

    public override void NetworkStart()
    {
        if (!IsOwner)
            return;

        playerName.Value = "Player";
    }
}
