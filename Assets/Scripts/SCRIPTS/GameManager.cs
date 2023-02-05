using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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

    [SerializeField] private RectTransform m_playerStatsPanel;
    [SerializeField] private Transform m_playerStatPrefab;

    private Dictionary<ulong, PlayerStatPrefab> m_statsPerPlayer = new();
    private Dictionary<ulong, NetworkVariable<int>> bitsPerPlayer = new();
    private int playerCount;
    private NetworkObject newObj;

    private void Start()
    {
        hostStartGameBtn.onClick.AddListener(() => HostStartGameServerRpc());
        CheckIfCanStartGame();
    }

    public void RegisterBitCount(ulong playerID)
    {
        bitsPerPlayer.Add(playerID, new NetworkVariable<int>(0));
    }

    [ServerRpc(RequireOwnership = false)]
    public void RegisterStatsUIServerRpc(ulong playerID)
    {
        Debug.Log("FIRING UI INSTANTI");
        Transform statPrefab = Instantiate(m_playerStatPrefab, m_playerStatsPanel);
        m_statsPerPlayer.Add(playerID, statPrefab.GetComponent<PlayerStatPrefab>());
        string name = $"Player {playerID + 1}";
        statPrefab.GetComponent<PlayerStatPrefab>().SetName(name);

        newObj = statPrefab.GetComponent<NetworkObject>();
        newObj.Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerBitsServerRpc(int numBits, ServerRpcParams serverRpcParams = default)
    {
        bitsPerPlayer[serverRpcParams.Receive.SenderClientId] = new NetworkVariable<int>(numBits);

        if (numBits >= winningBits)
        {
            NotifyClientsOfWinClientRpc(serverRpcParams.Receive.SenderClientId);
        }
        else
        {
            m_statsPerPlayer[serverRpcParams.Receive.SenderClientId].SetBits(bitsPerPlayer[serverRpcParams.Receive.SenderClientId]);
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
        Cursor.lockState = CursorLockMode.Locked;
    }

    [ClientRpc]
    private void ClientStartGameClientRpc(ClientRpcParams clientRpcParams = default)
    {
        OnGameStart?.Invoke();
    }
    
    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2f);
        ResetGamePlayServerRpc();
    }

    [ServerRpc]
    private void ResetGamePlayServerRpc(ServerRpcParams serverRpcParams = default)
    {
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

    public void SpawnStatUi()
    {
        
    }
}