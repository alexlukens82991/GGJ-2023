using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerItemSpawner : NetworkBehaviour
{
    [SerializeField] private Transform m_Player;
    [SerializeField] private Transform m_SpawnPoint;
    [SerializeField] private Transform m_Sights;
    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0))
        {
            SpawnItemData spawnItemData = new();

            spawnItemData._hasVelocity = true;

            spawnItemData.SetPosition(m_SpawnPoint.position);
            spawnItemData.SetRotation(m_SpawnPoint.rotation);

            Vector3 veloDir = (m_Sights.position - m_SpawnPoint.position).normalized;

            veloDir *= 10;

            spawnItemData.SetVelocity(veloDir);

            if (!IsServer)
                NetworkSpawner.Instance.SpawnItemServerRpc(0, spawnItemData);
            else
                NetworkSpawner.Instance.SpawnItem(0, spawnItemData);
        }


    }
}
