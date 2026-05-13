using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class survivalCount : NetworkBehaviour
{
    // 명단은 서버만 관리합니다. (NetworkList는 자동 동기화되지만, 수정은 서버만 함)
    public NetworkList<ulong> m_PlayerId = new NetworkList<ulong>();
    public static survivalCount instance { get; private set; }
    public List<int > m_Players = new List<int>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public override void OnNetworkSpawn()
    {
        // 서버만 이벤트를 구독해서 명단을 수정합니다.
        if (IsServer)
        {
            SubscribeManager.instance.Subscribe<ulong>(SubscribeType.PlayerSpawnCountUp, PlayerSpawnCountUp);
            SubscribeManager.instance.Subscribe<ulong>(SubscribeType.PlayerSpawnCountDown, PlayerSpawnCountDown);
        }
    }

    private void PlayerSpawnCountUp(ulong playerId)
    {
        if (!m_PlayerId.Contains(playerId))
        {
            m_Players.Add((int)playerId);
            m_PlayerId.Add(playerId);
            Debug.Log($"[서버] 플레이어 입장: {playerId}. 현재 인원: {m_PlayerId.Count}");
        }
    }

    // 서버 전용 함수: 플레이어가 죽었을 때 호출됨
    private void PlayerSpawnCountDown(ulong playerId)
    {
        Debug.Log($"PlayerSpawnCountDown");
        if (m_PlayerId.Contains(playerId))
        {
            m_PlayerId.Remove(playerId);
            Debug.Log($"[서버] 플레이어 탈락: {playerId}. 남은 인원: {m_PlayerId.Count}");

            // 마지막 1명이 남았을 때
            if (m_PlayerId.Count == 1)
            {
                ulong winnerId = m_PlayerId[0];
                // 모든 클라이언트에게 우승자를 알립니다.
                NotifyWinnerClientRpc(winnerId);
            }
        }
        else
        {
            Debug.LogWarning($"[서버] 탈락 처리 실패: 플레이어 ID {playerId}가 명단에 없습니다.");
        }
    }

    [ClientRpc]
    private void NotifyWinnerClientRpc(ulong winnerId)
    {
        Debug.Log($"[클라이언트] 우승자 발표: {winnerId}");
        
        // 모든 클라이언트의 PlayerController가 이 소식을 듣게 합니다.
        SubscribeManager.instance.Publish(SubscribeType.PlayerWinLose, winnerId);
    }
     
}