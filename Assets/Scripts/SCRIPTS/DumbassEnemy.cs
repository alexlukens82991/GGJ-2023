using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class DumbassEnemy : NetworkBehaviour
{
    public int Health = 100;

    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform bit;

    private void Start()
    {
        StartCoroutine(RoamRoutine());
    }

    private IEnumerator RoamRoutine()
    {
        do
        {
            float walkDist = 20f;
            Vector3 randomDirection = Random.insideUnitSphere * walkDist;

            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkDist, 1);
            Vector3 finalPosition = hit.position;


            navAgent.SetDestination(finalPosition);

            yield return new WaitForSeconds(Random.Range(3f, 9f));
        } while (true);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag.Equals("PlayerBullet"))
        {
            Health -= 25;
        }

        if (Health <= 0)
        {
            // spawn bits
            // kill
            print("KILLING ENEMY: " + gameObject.name);
            SpawnBitBundleServerRpc();
        }
    }

    [ServerRpc]
    public void SpawnBitBundleServerRpc()
    {
        print("SERVER SPAWN BITS FIRED!");
        for (int i = 0; i < 24; i++)
        {
            Transform newItem = Instantiate(bit);
            newItem.position = transform.position;

            NetworkObject foundObj = newItem.GetComponent<NetworkObject>();
            foundObj.Spawn();
        }
        
    }
}
