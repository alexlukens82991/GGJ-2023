using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkSingleton<GameManager>
{
    private NetworkManager networkManager;

    private Dictionary<NetworkClient, int> bitsPerPlayer = new();
    
    private void Start()
    {
        InitializeBitDictionary();
    }

    private void InitializeBitDictionary()
    {
        foreach (var player in networkManager.ConnectedClients)
        {
            bitsPerPlayer.Add(player.Value, 0);
        }
    }

    public void SetPlayerBits(NetworkClient player, int numBits)
    {
        bitsPerPlayer[player] = numBits;
    }
}
