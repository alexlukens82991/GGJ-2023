using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RootPortal : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        NetcodePlayer player = other.GetComponent<NetcodePlayer>();

        if (player == null) return;
        Debug.Log($"Player Collided");
        player.MoveBackToRoomClientRpc();
    }
}
