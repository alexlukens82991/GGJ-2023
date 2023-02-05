using Unity.Netcode;
using UnityEngine;

public class BitCollector : NetworkBehaviour
{
    public int CurrentBits;

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag.Equals("Bit"))
        {
            CurrentBits++;

            NetworkObject bit = other.gameObject.GetComponent<NetworkObject>();
            DespawnBitServerRpc(bit.NetworkObjectId);
        }
        
    }

    [ServerRpc]
    private void DespawnBitServerRpc(ulong id)
    {
        NetworkObject found = NetworkManager.SpawnManager.SpawnedObjects[id];
        if (found != null)
            Destroy(found.gameObject);

        DespawnBitClientRpc(id);
    }

    [ClientRpc]
    private void DespawnBitClientRpc(ulong id)
    {
        NetworkObject found = NetworkManager.SpawnManager.SpawnedObjects[id];
        if (found != null)
            Destroy(found.gameObject);
    }
}