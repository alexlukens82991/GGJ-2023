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
    public ulong PlayerNumber;
    public NetworkVariable<ulong> RoomId;

    [SerializeField] private GameObject[] models;

    [Header("Cache")] [SerializeField] private FirstPersonMovement firstPersonMovement;
    [SerializeField] private NetworkAnimator networkAnimator;
    [SerializeField] private NetworkTransform networkTransform;
    [SerializeField] private Transform spawnRoomPrefab;
    [SerializeField] private ulong roomID;

    public Transform SpawnRoom;
    private BitCollector bitCollector;
    private static int playersConnected;

    private void OnEnable()
    {
        if (IsServer || IsHost)
        {
            OnNetworkSpawn();
        }
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log($"Spawned player {OwnerClientId}");
        RegisterPlayerClientRpc(OwnerClientId);
        GameManager.Instance.CreateUI(OwnerClientId);
        GameManager.Instance.RebuiltPlayerUIClientRpc(OwnerClientId);
        
        if (IsHost)
            UpdateModelClientRpc();

        if (!IsOwner) return;
        playersConnected++;
        // Initialize stats
        Health = 100;

        StartCoroutine(UpdateRoomRefForAll());


        // Spawn in new player room
        SpawnRoomServerRpc(OwnerClientId);
    }

    public void SetTransform(Vector3 transform)
    {
        networkTransform.SetState(transform);
    }

    private void OnClientConnect(ulong id)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] {OwnerClientId}
            }
        };
        UpdateModelClientRpc();
        UpdateSpawnedUserTagServerRpc(id);
    }

    IEnumerator FuckingJustWait()
    {
        do
        {
            UpdateTheFuckingRoom();
            yield return new WaitForSeconds(1);
        } while (SpawnRoom == null);
    }

    [ClientRpc]
    private void RegisterPlayerClientRpc(ulong id)
    {
        GameManager.Instance.UpdatePlayerCount();
        GameManager.Instance.RegisterBitCount(OwnerClientId);
        Debug.Log($"Registered player");
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateSpawnedUserTagServerRpc(ulong id)
    {
        print("UpdateSpawnedUserTagServerRpc");

        NetworkClient client = NetworkManager.ConnectedClients[id];

        client.PlayerObject.gameObject.tag = "Player_" + id;

        TellUsersAboutNewTagClientRpc(id);
    }

    [ClientRpc]
    private void TellUsersAboutNewTagClientRpc(ulong id)
    {
        print("TELL NEW USERS ABOUT NEW TAG");

        GameObject[] foundPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in foundPlayers)
        {
            NetcodePlayer netPlayer = player.GetComponent<NetcodePlayer>();

            if (netPlayer == null)
                continue;

            Debug.Log("FOUND PLAYER!", player);
            player.tag = "Player_" + player.GetComponent<NetcodePlayer>().OwnerClientId;
        }
    }

    private void Start()
    {
        bitCollector = GetComponent<BitCollector>();
        NetworkManager.OnClientConnectedCallback += OnClientConnect;
        //StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        var playerlist = FindObjectsOfType<NetcodePlayer>();
        yield return new WaitForSeconds(2f);
        
        foreach (var player in playerlist)
        {
            GameManager.Instance.RegisterStatsUIClientRpc(player.OwnerClientId);
        }
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

        SpawnRoom = newItem;

        // return to client that requested it
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] {id}
            }
        };

        RoomId.Value = foundObj.NetworkObjectId;

        ReturnRoomToClientClientRpc(RoomId.Value, clientRpcParams);
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

    private IEnumerator UpdateRoomRefForAll()
    {
        bool playersAreSet;

        do
        {
            playersAreSet = true;
            GameObject[] foundPlayers = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in foundPlayers)
            {
                NetcodePlayer netPlayer = player.GetComponent<NetcodePlayer>();

                if (netPlayer == null)
                    continue;

                netPlayer.UpdateTheFuckingRoom();
                if (netPlayer.SpawnRoom == null)
                    playersAreSet = false;
            }

            print("UPDATING ROOM REF");
            yield return new WaitForSeconds(1);
        } while (!playersAreSet);
    }

    public void UpdateTheFuckingRoom()
    {
        if (RoomId.Value == 0) return;
        print("ATTEMPTING TO UPDATE ROOM.");
        print("Room status: " + SpawnRoom == null);
        NetworkObject netObj = NetworkManager.SpawnManager.SpawnedObjects[RoomId.Value];
        SpawnRoom = netObj.transform;
    }

    public void MovePlayerToRoom(ulong room)
    {
        SpawnRoom = NetworkManager.SpawnManager.SpawnedObjects[room].transform;
        networkTransform.SetState(SpawnRoom.GetComponent<SpawnRoom>().GetSpawnPoint());
        HackerComputer.Instance.SetTargetPlayer(networkTransform);
        HackerComputer.Instance.SetComputer(SpawnRoom.GetComponent<SpawnRoom>().GetComputer());
    }

    public void MoveBackToRoom()
    {
        Health = 100;
        HackerComputer.Instance.Reset();
        transform.position = SpawnRoom.GetComponent<SpawnRoom>().GetSpawnPoint();
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
            if (i.Equals((int) id))
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
            SoundBank.Instance.PlayOneShot(6);

            // spawn bits
            // kill
            Bits = 0;
            MoveBackToRoom();
        }
        else
            SoundBank.Instance.PlayOneShot(5);
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