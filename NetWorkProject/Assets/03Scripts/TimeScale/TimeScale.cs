using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TimeScale : NetworkBehaviour
{
    protected override void OnNetworkPostSpawn()
    {
        Time.timeScale = 0;
        NetworkManager.SceneManager.OnLoadEventCompleted += HandleAllPlayersLoaded;
    }
    private void HandleAllPlayersLoaded(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        Time.timeScale = 1;
    }
}
