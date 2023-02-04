using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerBullet : NetworkBehaviour
{
    [SerializeField] private NetworkObject netObj;
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsServer) return;
        //StartCoroutine(EndOfFrameExecute());
        Destroy(gameObject);
    }

    IEnumerator EndOfFrameExecute()
    {
        yield return new WaitUntil(() => netObj.IsSpawned);
        netObj.Despawn();
    }


    [ServerRpc]
    private void DespawnServerRpc()
    {
        
    }
}
