using Unity.Netcode;
using UnityEngine;

public class PauseManager : NetworkBehaviour
{
    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }
}