using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerBullet : NetworkBehaviour
{
    [SerializeField] private NetworkObject netObj;
    private void OnTriggerEnter(Collider collision)
    {
        if (!IsServer) return;

        if (collision.tag == "Player")
        {
            NetcodePlayer foundPlayer = collision.gameObject.GetComponent<NetcodePlayer>();

            if (foundPlayer != null)
            {
                if (OwnerClientId != foundPlayer.OwnerClientId)
                {
                    foundPlayer.DamagePlayer();
                }
            }
            else
            {
            }
        }
        else if (collision.tag == "DumbassEnemy")
        {
            collision.GetComponentInParent<DumbassEnemy>().Damage();
        }

        Destroy(gameObject);

    }
}
