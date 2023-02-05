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

        if (collision.collider.tag == "Player")
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
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
