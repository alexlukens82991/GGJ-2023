using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class NetcodePlayer : NetworkBehaviour
{
    public int Health;
    public int Bits;
    public int GunHeat;

    [Header("Cache")]
    [SerializeField] private NetworkTransform networkTransform;
    [SerializeField] private Transform spawnRoom;
    [SerializeField] private Transform spawnRoomPrefab;
    [SerializeField] private ulong roomID;
    private BitCollector bitCollector;

    public override void OnNetworkSpawn()
    {
        RegisterPlayerClientRpc();
        if (!IsOwner) return;

        // Initialize stats
        Health = 100;

        // Spawn in new player room
        SpawnRoomServerRpc(OwnerClientId);

        // move new player to room

        // Update gamemanager
    }
    
    [ClientRpc]
    private void RegisterPlayerClientRpc()
    {
        GameManager.Instance.RegisterBitCount(OwnerClientId);
    }

    private void Start()
    {
        bitCollector = GetComponent<BitCollector>();
    }

    [ServerRpc]
    public void SpawnRoomServerRpc(ulong id)
    {
        print("SPAWN ROOM RPC");
        int numPlayersConnected = NetworkManager.ConnectedClients.Count;
        Vector3 roomPos = Vector3.zero + (Vector3.right * numPlayersConnected * 20);

        SpawnItemData roomSpawnData = new();
        roomSpawnData.SetPosition(roomPos);

        Transform newItem = Instantiate(spawnRoomPrefab);
        newItem.position = roomSpawnData.GetPosition();

        NetworkObject foundObj = newItem.GetComponent<NetworkObject>();
        foundObj.Spawn();

        spawnRoom = newItem;

        // return to client that requested it
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { id }
            }
        };

        roomID = foundObj.NetworkObjectId;

        ReturnRoomToClientClientRpc(roomID, clientRpcParams);
    }

    [ClientRpc]
    private void ReturnRoomToClientClientRpc(ulong room, ClientRpcParams clientRpcParams)
    {
        MovePlayerToRoom(room);
    }

    public void MovePlayerToRoom(ulong room)
    {
        spawnRoom = NetworkManager.SpawnManager.SpawnedObjects[room].transform;
        networkTransform.SetState(spawnRoom.GetComponent<SpawnRoom>().GetSpawnPoint());
        HackerComputer.Instance.SetTargetPlayer(networkTransform);
        HackerComputer.Instance.SetComputer(spawnRoom.GetComponent<SpawnRoom>().GetComputer());
    }
    

    public void MoveBackToRoom()
    {
        transform.position = spawnRoom.GetComponent<SpawnRoom>().GetSpawnPoint();
        Debug.Log($"Moved player");
        GameManager.Instance.SetPlayerBitsServerRpc(bitCollector.CurrentBits);
    }
}