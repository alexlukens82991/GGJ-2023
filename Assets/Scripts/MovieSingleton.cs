using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Video;

public class MovieSingleton : NetworkBehaviour
{
    [SerializeField] private VideoPlayer m_VideoPlayer;

    public static MovieSingleton Instance;

    private void Awake()
    {
        Instance = this;
    }

    [ServerRpc]
    public void PlayMoveServerRpc()
    {
        m_VideoPlayer.Play();
        PlayMovieClientRpc();
    }

    [ClientRpc]
    private void PlayMovieClientRpc()
    {
        m_VideoPlayer.Play();
    }

    [ServerRpc]
    public void PauseServerRpc()
    {
        m_VideoPlayer.Pause();
        PauseClientRpc();
    }

    [ClientRpc]
    public void PauseClientRpc()
    {
        m_VideoPlayer.Pause();
    }
}
