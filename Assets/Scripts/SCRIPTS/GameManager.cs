using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkSingleton<GameManager>
{
    private Dictionary<ulong, int> bitsPerPlayer = new();
    [SerializeField] private int winningBits = 150;
    private List<ulong> longList = new();

    public void RegisterBitCount(ulong playerID)
    {
        bitsPerPlayer.Add(playerID, 0);
        longList.Add(playerID); 
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerBitsServerRpc(int numBits, ServerRpcParams serverRpcParams = default)
    {
        bitsPerPlayer[serverRpcParams.Receive.SenderClientId] = numBits;

        if (numBits >= winningBits)
        {
            NotifyClientsOfWinClientRpc(serverRpcParams.Receive.SenderClientId);
        }
    }

    [ClientRpc]
    public void NotifyClientsOfWinClientRpc(ulong id)
    {
        Debug.Log($"{id} wins!");
    }
}