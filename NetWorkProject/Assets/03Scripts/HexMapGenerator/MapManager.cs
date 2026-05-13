using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class MapManager : NetworkBehaviour
{
    // 어디서든 쉽게 접근할 수 있게 싱글톤 패턴 적용
    public static MapManager Instance;

    // 모든 발판을 번호(ID)로 관리하기 위한 리스트
    private List<HexTiles> allTiles = new List<HexTiles>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public override void OnNetworkSpawn()
    {
        SubscribeManager.instance.Publish(SubscribeType.DeSpawnObjects, this.NetworkObject);
    }
    // 발판이 맵에 생성될 때, 자기를 매니저에 등록하고 고유 번호(ID)를 발급받는 함수
    public int RegisterTile(HexTiles tile)
    {
        allTiles.Add(tile);
        return allTiles.Count - 1; // 0번, 1번, 2번... 순으로 ID 발급
    }

    
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void RequestHideTileRpc(int tileID)
    {
        // 서버가 요청을 받으면, 모든 사람에게 지우라고 다시 명령함
        HideTileRpc(tileID);
    }

    
    [Rpc(SendTo.Everyone)]
    private void HideTileRpc(int tileID)
    {
        // 모든 클라이언트는 자기 화면의 allTiles 리스트에서 해당 번호를 찾아 끔
        if (tileID >= 0 && tileID < allTiles.Count)
        {
            allTiles[tileID].HideTileLocally();
        }
    }
}