using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    public Transform spawnPointsParent;
    private List<Transform> spawnPoints = new();


    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        foreach (Transform transform in spawnPointsParent)
        {
            spawnPoints.Add(transform);
        }

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        print("SPAWNING ENEMIES");
        do
        {
            Transform m_SpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            SpawnItemData spawnItemData = new();

            spawnItemData.SetPosition(m_SpawnPoint.position);
            spawnItemData.SetRotation(m_SpawnPoint.rotation);

            NetworkSpawner.Instance.SpawnItemServerRpc(1, spawnItemData);

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        } while (true);
    }
}
