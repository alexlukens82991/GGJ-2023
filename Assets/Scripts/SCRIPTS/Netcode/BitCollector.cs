using Unity.Netcode;
using UnityEngine;

public class BitCollector : NetworkBehaviour
{
    public int CurrentBits;

    private void OnCollisionEnter(Collision other)
    {
        Bit bit = other.gameObject.GetComponent<Bit>();
        if (bit == null) return;
        CurrentBits++;
        bit.gameObject.SetActive(false);
    }
}