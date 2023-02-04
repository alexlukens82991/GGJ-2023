using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

public class GhostController : NetworkBehaviour
{
    [SerializeField] private GameObject objectToToggle;

    [ServerRpc]
    public void CmdToggleVisibilityServerRpc()
    {
        print("serverrpc fired");
        objectToToggle.SetActive(!objectToToggle.activeSelf);
        RpcToggleVisibilityClientRpc(objectToToggle.activeSelf);
    }

    [ClientRpc]
    public void RpcToggleVisibilityClientRpc(bool active)
    {
        print("clientRpc telling object");
        objectToToggle.SetActive(active);
    }
}
