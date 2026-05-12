using Unity.Netcode;
using UnityEngine;

public class DeadZone : NetworkBehaviour
{
     private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!IsOwner) return;
            other.gameObject.TryGetComponent<NetworkObject>(out var networkObject);
            networkObject?.Despawn();
            // 플레이어가 사망 구역에 들어왔을 때 처리할 로직
            Debug.Log("Player entered the Dead Zone!");
                
            // 예: 플레이어의 체력을 0으로 만들거나, 리스폰 위치로 이동시키는 등의 처리를 할 수 있습니다.
        }
    }
}
