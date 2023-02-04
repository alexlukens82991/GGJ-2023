using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class NetworkSingleton<T> : NetworkBehaviour where T : Component
{
    public static T Instance;

    private void Awake()
    {
        Instance = this as T;
    }
}
