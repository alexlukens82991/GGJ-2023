using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class NetworkSpawner : NetworkSingleton<NetworkSpawner>
{
    [SerializeField] private Transform[] m_SpawnableItems;

    [ServerRpc(RequireOwnership = false)]
    public void SpawnItemServerRpc(int itemInt, SpawnItemData spawnItemData, ulong id = 12345)
    {
        if (itemInt >= m_SpawnableItems.Length)
        {
            Debug.LogError("ERROR SPAWNING ITEM: " + itemInt);
        }

        Transform newItem = Instantiate(m_SpawnableItems[itemInt]);
        newItem.position = spawnItemData.GetPosition();
        newItem.rotation = spawnItemData.GetRotation();

        NetworkObject foundObj = newItem.GetComponent<NetworkObject>();
        foundObj.Spawn();

        if (spawnItemData._hasVelocity)
        {
            Rigidbody foundRb = newItem.GetComponent<Rigidbody>();
            foundRb.AddForce(spawnItemData.GetVelocity(), ForceMode.Impulse);
        }

        if (itemInt == 0)
        {
            print("SETTING BULLET TAG " + "PlayerBullet_" + id);
            newItem.tag = "PlayerBullet_" + id;
            UpdateBulletTagClientRpc(foundObj.NetworkObjectId, id);
        }

        //if (id != 12345)
        //{
        //    // bullet, player
        //    NetworkClient requestingClient = NetworkManager.ConnectedClients[id];

        //    Physics.IgnoreCollision(requestingClient.PlayerObject.gameObject.GetComponent<Collider>(), newItem.GetComponent<Collider>());
        //}
    }

    [ClientRpc]
    private void UpdateBulletTagClientRpc(ulong bulletId, ulong clientId)
    {
        NetworkObject netObj = NetworkManager.SpawnManager.SpawnedObjects[bulletId];

        netObj.tag = "PlayerBullet_" + clientId; ;
    }


    public void SpawnItem(int itemInt, SpawnItemData spawnItemData)
    {
        if (itemInt >= m_SpawnableItems.Length)
        {
            Debug.LogError("ERROR SPAWNING ITEM: " + itemInt);
        }

        Transform newItem = Instantiate(m_SpawnableItems[itemInt]);
        newItem.position = spawnItemData.GetPosition();
        newItem.rotation = spawnItemData.GetRotation();

        NetworkObject foundObj = newItem.GetComponent<NetworkObject>();
        foundObj.Spawn();

        if (spawnItemData._hasVelocity)
        {
            Rigidbody foundRb = newItem.GetComponent<Rigidbody>();

            foundRb.AddForce(spawnItemData.GetVelocity(), ForceMode.Impulse);
        }
    }
}

public struct SpawnItemData : INetworkSerializable
{
    public float _xPos;
    public float _yPos;
    public float _zPos;

    public float _xRot;
    public float _yRot;
    public float _zRot;
    public float _wRot;

    public float _veloX;
    public float _veloY;
    public float _veloZ;

    public bool _hasVelocity;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref _xPos);
        serializer.SerializeValue(ref _yPos);
        serializer.SerializeValue(ref _zPos);

        serializer.SerializeValue(ref _xRot);
        serializer.SerializeValue(ref _yRot);
        serializer.SerializeValue(ref _zRot);
        serializer.SerializeValue(ref _wRot);

        serializer.SerializeValue(ref _veloX);
        serializer.SerializeValue(ref _veloY);
        serializer.SerializeValue(ref _veloZ);

        serializer.SerializeValue(ref _hasVelocity);
    }

    public void SetPosition(Vector3 pos)
    {
        _xPos = pos.x;
        _yPos = pos.y;
        _zPos = pos.z;
    }

    public void SetRotation(Quaternion rot)
    {
        _xRot = rot.x;
        _yRot = rot.y;
        _zRot = rot.z;
        _wRot = rot.w;
    }

    public void SetVelocity(Vector3 dir)
    {
        _veloX = dir.x;
        _veloY = dir.y;
        _veloZ = dir.z;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(_xPos, _yPos, _zPos);
    }

    public Quaternion GetRotation()
    {
        return new Quaternion(_xRot, _yRot, _zRot, _wRot);
    }

    public Vector3 GetVelocity()
    {
        return new Vector3(_veloX, _veloY, _veloZ);
    }
}
