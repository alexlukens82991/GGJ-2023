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

    [SerializeField] private GameObject[] models;

    [Header("Cache")]
    [SerializeField] private FirstPersonMovement firstPersonMovement;
    [SerializeField] private NetworkAnimator networkAnimator;
    [SerializeField] private NetworkTransform networkTransform;
    [SerializeField] private Transform spawnRoom;
    [SerializeField] private Transform spawnRoomPrefab;
    [SerializeField] private ulong roomID;
    private BitCollector bitCollector;
    private static int playersConnected;


    public override void OnNetworkSpawn()
    {
        RegisterPlayerClientRpc();

        if (IsHost)
            UpdateModelClientRpc();

        if (!IsOwner) return;
        playersConnected++;
        // Initialize stats
        Health = 100;

        // Spawn in new player room
        SpawnRoomServerRpc(OwnerClientId);
    }

    private void OnClientConnect(ulong id)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { OwnerClientId }
            }
        };
        UpdateModelClientRpc();
    }

    [ClientRpc]
    private void RegisterPlayerClientRpc()
    {
        GameManager.Instance.RegisterBitCount(OwnerClientId);
        GameManager.Instance.UpdatePlayerCount();
    }

    private void Start()
    {
        bitCollector = GetComponent<BitCollector>();
        NetworkManager.OnClientConnectedCallback += OnClientConnect;
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

    [ClientRpc]
    private void UpdateModelClientRpc()
    {
        SelectModel();
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
        Health = 100;
        HackerComputer.Instance.Reset();
        transform.position = spawnRoom.GetComponent<SpawnRoom>().GetSpawnPoint();
        Debug.Log($"Moved player");
        GameManager.Instance.SetPlayerBitsServerRpc(bitCollector.CurrentBits);
    }


    public void SelectModel()
    {
        ulong id = OwnerClientId;

        if (id >= 4)
        {
            do
            {
                id -= 4;
            } while (OwnerClientId >= 4);
        }

        GameObject activeModel = null;

        print("PLAYERS CONNECTED: " + id);

        for (int i = 0; i < models.Length; i++)
        {
            if (i.Equals((int)id))
            {
                activeModel = models[i];

                activeModel.SetActive(true);
            }
            else
            {
                print("FAILED: " + i + " | " + id);
                models[i].SetActive(false);
            }
        }

        Animator foundAnimator = activeModel.GetComponent<Animator>();
        networkAnimator.Animator = foundAnimator;
        firstPersonMovement.SetAnimator(foundAnimator);
    }

    public void DamagePlayer()
    {
        Health -= 15;
        if (Health <= 0)
        {
            // spawn bits
            // kill
            Bits = 0;
            MoveBackToRoom();
        }
    }

    //private void OnCollisionEnter(Collision col)
    //{
    //    if (col.collider.tag.Equals("PlayerBullet"))
    //    {
    //        Health -= 15;
    //    }
    //    else
    //        print("HIT, but not bullet?");
    //    if (Health <= 0)
    //    {
    //        // spawn bits
    //        // kill
    //        Bits = 0;
    //        MoveBackToRoom();
    //    }
    //}
}