using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    
    public Vector3 GetSpawnPoint()
    {
        return spawnPoint.position;
    }
}
