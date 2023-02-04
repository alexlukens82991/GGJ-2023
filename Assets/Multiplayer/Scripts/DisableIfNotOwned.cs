using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DisableIfNotOwned : NetworkBehaviour
{
    public MonoBehaviour[] ScriptsToDisable;
    public NetworkBehaviour[] NetworkScriptsToDisable;
    public AudioListener AudioListener;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) return;

        foreach (MonoBehaviour item in ScriptsToDisable)
        {
            item.enabled = false;
        }

        foreach (NetworkBehaviour item in NetworkScriptsToDisable)
        {
            item.enabled = false;
        }

        if (AudioListener)
            AudioListener.enabled = false;
    }
}
