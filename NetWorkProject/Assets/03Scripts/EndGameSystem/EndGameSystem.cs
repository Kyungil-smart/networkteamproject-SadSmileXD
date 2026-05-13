using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EndGameSystem : NetworkBehaviour
{
    public List<NetworkObject> networkObjects = new List<NetworkObject>();
    private void Awake()
    {
      
    }
    public override void OnNetworkSpawn()
    {
        // 서버에서만 작동해야 하는 시스템이라면 여기서 체크
        if (!IsServer) return;
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("EndGameSystem: 서버 구독 완료");
        SubscribeManager.instance.Subscribe<NetworkObject>(SubscribeType.DeSpawnObjects, despawnPush);
        SubscribeManager.instance.Subscribe(SubscribeType.DeSpawnObjectsComplete, free);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) return;

        SubscribeManager.instance.Unsubscribe<NetworkObject>(SubscribeType.DeSpawnObjects, despawnPush);
        SubscribeManager.instance.Unsubscribe(SubscribeType.DeSpawnObjectsComplete, free);
    }
    public void despawnPush(NetworkObject obj)
    {
        networkObjects.Add(obj);
    }
    public void free()
    {
        foreach (var obj in networkObjects)
        {
            if (obj != null)
            {
                obj.Despawn(true);
            }
        }
        networkObjects.Clear();
        this.NetworkObject.Despawn(true);
    }
}
