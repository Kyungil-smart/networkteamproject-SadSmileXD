using Unity.Netcode;
using UnityEngine;

public class IsServer : NetworkBehaviour
{

    public override void OnNetworkSpawn()
    {
        if (IsServer) return;
        this.gameObject.SetActive(false);
    }
}
