using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DumbassEnemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navAgent;

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
}
