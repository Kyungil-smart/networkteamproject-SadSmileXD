using Unity.Netcode;
using UnityEngine;

public class Stopgenerator : NetworkBehaviour
{
    private void Start()
    {
        // 접속 승인 기능을 활성화합니다.
        NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;

        if (IsServer)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // 일단 접속은 승인하되, 자동으로 플레이어를 생성하지 않게 설정합니다.
        response.Approved = true;
        response.CreatePlayerObject = false; // <--- 이게 핵심! (자동 생성 방지)
        response.Pending = false;
    }
}
