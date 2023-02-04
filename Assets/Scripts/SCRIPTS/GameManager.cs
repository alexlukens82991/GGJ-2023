using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkSingleton<GameManager>
{
    private Dictionary<NetworkClient, int> bitsPerPlayer = new();
    [SerializeField] private int winningBits = 150;
    private List<ulong> longList = new();

    public void RegisterBitCount(ulong playerID)
    {
        bitsPerPlayer.Add(NetworkManager.ConnectedClients[playerID], 0);
        longList.Add(playerID); 
    }

    [ServerRpc]
    public void SetPlayerBitsServerRpc(ulong playerID, int numBits)
    {
        bitsPerPlayer[NetworkManager.ConnectedClients[playerID]] = numBits;

        if (numBits >= winningBits)
        {
            Debug.Log($"{NetworkManager.ConnectedClients[playerID]} wins!");
        }
    }
}