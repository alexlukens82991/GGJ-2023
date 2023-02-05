using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkSingleton<GameManager>
{
    public static event Action OnGameEnd;
    [SerializeField] private int winningBits = 150;
    [SerializeField] private GameObject playerWinPanel;
    [SerializeField] private TMP_Text  playerWinText;
    [SerializeField] private PauseManager pauseManager;
    
    private Dictionary<ulong, int> bitsPerPlayer = new();
    
    public void RegisterBitCount(ulong playerID)
    {
        bitsPerPlayer.Add(playerID, 0);
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
        playerWinText.text = $"Player {id} wins!";
        playerWinPanel.SetActive(true);
        pauseManager.Pause();
        OnGameEnd?.Invoke();
        //StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(5f);
        
    }
}