using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform computer;
    
    public Vector3 GetSpawnPoint()
    {
        return spawnPoint.position;
    }

    public Transform GetComputer()
    {
        return computer;
    }
}
