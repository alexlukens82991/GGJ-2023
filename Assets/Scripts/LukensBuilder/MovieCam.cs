using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MovieCam : NetworkBehaviour
{
    [SerializeField] private Camera m_MovieCam;
    [SerializeField] private Camera m_PlayerCam;
    private bool movieCamOn;
    public override void OnNetworkSpawn()
    {
        enabled = IsOwner;

        m_MovieCam = GameObject.FindGameObjectWithTag("MovieCam").GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (movieCamOn)
            {
                movieCamOn = false;
                m_MovieCam.enabled = false;
                m_PlayerCam.enabled = true;
            }
            else
            {
                movieCamOn = true;
                m_MovieCam.enabled = true;
                m_PlayerCam.enabled = false;
            }
        }
    }
}
