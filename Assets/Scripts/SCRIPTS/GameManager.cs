using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkSingleton<GameManager>
{
    public static event Action OnGameEnd;
    public static event Action OnGameStart;
    public static event Action OnGameRestart;

    [SerializeField] private int winningBits = 150;
    [SerializeField] private GameObject playerWinPanel;
    [SerializeField] private TMP_Text playerWinText;
    [SerializeField] private PauseManager pauseManager;

    [SerializeField] private Button hostStartGameBtn;

    private Dictionary<ulong, int> bitsPerPlayer = new();
    private int playerCount;

    private void Start()
    {
        hostStartGameBtn.onClick.AddListener(() => HostStartGameServerRpc());
        CheckIfCanStartGame();
    }

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
        playerWinText.text = $"Player {id + 1} wins!";
        playerWinPanel.SetActive(true);
        OnGameEnd?.Invoke();
        StartCoroutine(RestartGame());
    }

    private void CheckIfCanStartGame()
    {
        if (!IsHost)
            hostStartGameBtn.gameObject.SetActive(false);
        else if (IsHost && playerCount < 2)
        {
            hostStartGameBtn.gameObject.SetActive(true);
            hostStartGameBtn.interactable = false;
        }
        else if (IsHost && playerCount >= 2)
        {
            hostStartGameBtn.gameObject.SetActive(true);
            hostStartGameBtn.interactable = true;
        }
        else
            hostStartGameBtn.gameObject.SetActive(false);
    }

    [ServerRpc]
    private void HostStartGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ClientStartGameClientRpc();
        hostStartGameBtn.gameObject.SetActive(false);
        Debug.Log($"Calling from server RPC");
    }

    [ClientRpc]
    private void ClientStartGameClientRpc(ClientRpcParams clientRpcParams = default)
    {
        OnGameStart?.Invoke();
        Debug.Log($"Calling from Client RPC");
    }
    
    private IEnumerator RestartGame()
    {
        Debug.Log("CALLING COROUTINE");
        yield return new WaitForSeconds(2f);
        ResetGamePlayServerRpc();
    }

    [ServerRpc]
    private void ResetGamePlayServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("CALLING RESTART SERVER RPC");
        
        foreach (var client in NetworkManager.ConnectedClients)
        {
            var spawnPoint = client.Value.PlayerObject.GetComponent<NetcodePlayer>().SpawnRoom.GetComponent<SpawnRoom>().GetSpawnPoint();
            client.Value.PlayerObject.GetComponent<NetcodePlayer>().SetTransform(spawnPoint);
        }
        
        CheckIfCanStartGame();
        FullResetClientRpc();
    }

    [ClientRpc]
    private void FullResetClientRpc(ClientRpcParams clientRpcParams = default)
    {
        Debug.Log("CALLING RESTART CLIENT RPC");
        HackerComputer.Instance.Reset();
        pauseManager.Resume();
        OnGameRestart?.Invoke();
        playerWinPanel.SetActive(false);
    }
    
    public void UpdatePlayerCount()
    {
        playerCount++;
        CheckIfCanStartGame();
    }
}