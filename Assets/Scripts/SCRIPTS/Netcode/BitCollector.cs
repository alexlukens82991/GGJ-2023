using Unity.Netcode;
using UnityEngine;

public class BitCollector : NetworkBehaviour
{
    public int CurrentBits;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Bit"))
        {
            CurrentBits++;

            NetworkObject bit = other.gameObject.GetComponent<NetworkObject>();
            GameManager.Instance.SetPlayerBitsServerRpc(CurrentBits);
            DespawnBitServerRpc(bit.NetworkObjectId);
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
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