using Unity.Netcode;
using UnityEngine;

public class dest : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
         this.GetComponent<NetworkObject>().Despawn();
    }
    
}
