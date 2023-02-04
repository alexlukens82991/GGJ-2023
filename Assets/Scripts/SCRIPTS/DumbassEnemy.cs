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
    [SerializeField] private Collider enemyCollider;
    [SerializeField] private GameObject rbTransform;

    private void Start()
    {
        StartCoroutine(RoamRoutine());
        Health = 100;
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
            Health -= 100;
        }
        else
            print("HIT, but not bullet?");
        if (Health <= 0)
        {
            // spawn bits
            // kill
            print("KILLING ENEMY: " + gameObject.name);
            enemyCollider.enabled = false;
            SpawnBitBundleServerRpc();
            LocalKillAnimation();
        }
    }

    private void LocalKillAnimation()
    {
        navAgent.enabled = false;
        Rigidbody rb = rbTransform.AddComponent<Rigidbody>();
        rb.AddTorque(new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
        LukensUtils.LukensUtilities.DelayedFire(DespawnEnemyServerRpc, 3);
    }


    [ServerRpc]
    public void SpawnBitBundleServerRpc()
    {
        Vector3 spawnPoint = transform.position + (Vector3.up * 2);
        print("SERVER SPAWN BITS FIRED!");
        for (int i = -6; i < Random.Range(2, 6); i++)
        {
            Transform newItem = Instantiate(bit);
            newItem.position = spawnPoint + ((i * Vector3.right) * 0.2f);

            NetworkObject foundObj = newItem.GetComponent<NetworkObject>();
            foundObj.Spawn();

            Vector3 randomForce = new(Random.Range(-1, 1f), Random.Range(0.7f, 2), Random.Range(-1f, 1));
            foundObj.GetComponent<Rigidbody>().AddForce(randomForce, ForceMode.Impulse);
        }
    }

    [ServerRpc]
    private void DespawnEnemyServerRpc()
    {
        GetComponent<NetworkObject>().Despawn();
    }
}
