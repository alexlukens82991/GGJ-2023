using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class BitsBundle : NetworkBehaviour
{
    [SerializeField] private Rigidbody[] bits;

    public override void OnNetworkSpawn()
    {
        foreach (Rigidbody child in bits)
        {
            AddRandomForce(child);
        }
    }

    private void AddRandomForce(Rigidbody rb)
    {
        Vector3 randomForce = new(Random.Range(-5, 5), Random.Range(1, 7), Random.Range(-5, 5));
        rb.AddForce(randomForce, ForceMode.Impulse);
    }
}
