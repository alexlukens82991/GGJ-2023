using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraNetwork : NetworkBehaviour
{
    [SerializeField] private Camera cam;

    public override void OnNetworkSpawn()
    {
        cam.enabled = IsOwner;
    }
}
