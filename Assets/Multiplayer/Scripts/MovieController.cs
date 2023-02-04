using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MovieController : NetworkBehaviour
{
    private void Update()
    {
        if (!IsHost) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            MovieSingleton.Instance.PlayMoveServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            MovieSingleton.Instance.PauseServerRpc();
        }
    }
}
