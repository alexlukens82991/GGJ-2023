using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerBullet : NetworkBehaviour
{
    [SerializeField] private NetworkObject netObj;
    private void OnTriggerEnter(Collider collision)
    {
        //if (!IsServer) return;

        print("COLLIDED WITH: " + collision.tag);

        if (collision.tag == "Player")
        {
            print("PLAYER COLLISION DETECTED");
            NetcodePlayer foundPlayer = collision.gameObject.GetComponentInParent<NetcodePlayer>();

            if (foundPlayer != null)
            {
                string[] expandedThisTag = transform.tag.Split('_');
                string[] expandedPlayerHitTag = foundPlayer.transform.tag.Split('_');
                if (expandedThisTag[1] != expandedPlayerHitTag[1])
                {
                    foundPlayer.DamagePlayer();
                }
                else
                {
                    print("OWNER ID MATCHED OWNER WHO SPAWNED BULLET. IGNORING.");
                }
            }
            else
            {
                print("FOUND PLAYER NULL");
            }

        }
        else if (collision.tag == "DumbassEnemy")
        {
            collision.GetComponentInParent<DumbassEnemy>().Damage();
        }

        if (IsOwner)
            DestroyServerRpc();

    }

    [ServerRpc]
    private void DestroyServerRpc()
    {
        Destroy(gameObject);
    }
}
