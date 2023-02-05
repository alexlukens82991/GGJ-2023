using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;

public class NetworkManagerUI : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private Button m_CreateLobby;
    [SerializeField] private Button m_JoinLobby;
    [SerializeField] private TMP_InputField m_JoinCodeInput;
    [SerializeField] private Button m_JoinBtn;
    [SerializeField] private CanvasGroup m_Cg;
    [SerializeField] private CanvasGroup m_JoinCodeCg;
    [SerializeField] private GameObject m_JoinCodePanel;
    [SerializeField] private TextMeshProUGUI m_JoinCode;

    bool serverJoined = false;
    
    private void Awake()
    {
        m_CreateLobby.onClick.AddListener(() => { CreateRelay(); ClosePanel(); });
        m_JoinLobby.onClick.AddListener(() => { OpenJoinCode(); });
        m_JoinBtn.onClick.AddListener(() => { JoinRelay(m_JoinCodeInput.text); });
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            print("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        NetworkManager.Singleton.OnClientConnectedCallback += CloseJoinCodePanel;
    }

    private void OpenJoinCodePanel(bool open)
    {
        m_JoinCodeCg.alpha = open ? 1 : 0;
        m_JoinCodeCg.blocksRaycasts = open;
        m_JoinCodeCg.interactable = open;
    }

    private void CloseJoinCodePanel(ulong id)
    {
        if (serverJoined)
            OpenJoinCodePanel(false);
        else
            serverJoined = true;
    }

    private void ClosePanel()
    {
        m_Cg.alpha = 0;
        m_Cg.interactable = false;
        m_Cg.blocksRaycasts = false;
    }

    private void OpenJoinCode()
    {
        m_JoinCodePanel.SetActive(true);
    }

    private async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(10);

            string JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            print(JoinCode);
            m_JoinCode.text = JoinCode;
            OpenJoinCodePanel(true);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }

    private async void JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();

            ClosePanel();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
        }
        await RelayService.Instance.JoinAllocationAsync(joinCode);
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= CloseJoinCodePanel;
        }
    }
}
