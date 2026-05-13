using Unity.Netcode;
using UnityEngine;

public class DeadZone : NetworkBehaviour
{
    private bool flag = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. 부딪힌 오브젝트에서 PlayerController를 가져옴
            var playerController = other.GetComponentInChildren<PlayerController>();

            if (playerController != null)
            {
                // 2. 중요: 부딪힌 '그 플레이어'가 로컬 플레이어(나)인지 확인
                if (playerController.IsOwner&& flag==false)
                {
                    flag = true;
                    Debug.Log("내가 데드존에 들어감!");
                    playerController.Die();
                }
            }
        }
    }
}
