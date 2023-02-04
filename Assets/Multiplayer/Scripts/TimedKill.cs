using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TimedKill : NetworkBehaviour
{
    [SerializeField] private float timeTillDeath = 10;
    private void Start()
    {
        LukensUtils.LukensUtilities.DelayedFire(DestroySelfLocal, timeTillDeath);
    }

    private void DestroySelfLocal()
    {

        if (IsServer)
        {
            Destroy(gameObject);
        }
    }

    [ClientRpc]
    private void DestroySelfClientRpc()
    {
        print("DESTORYED");
        Destroy(gameObject);
    }
}
