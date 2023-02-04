using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private NetworkObject netObj;
    private void OnCollisionEnter(Collision collision)
    {
        DespawnServerRpc();
        //Destroy(gameObject);
    }


    [ServerRpc]
    private void DespawnServerRpc()
    {
        netObj.Despawn();
    }
}
