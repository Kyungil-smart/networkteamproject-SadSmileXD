using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;
using System.Collections.Generic;

public class PlayerSpawnManager : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab;    // NetworkObject 가 붙은 플레이어 프리팹
    [SerializeField] private Transform[] _spawnPoints;
    List<Vector3> spawnPositions = new List<Vector3>();
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        for(int i=0; i<10; i++)
        {
            for(int j=0; j<10; j++)
            {
                spawnPositions.Add(new Vector3(i*2, 5002, j*2f));
            }
        }
        SpawnAllPlayers();
    }

    private void SpawnAllPlayers()
    {
        int index = 0;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
           // Transform sp = _spawnPoints[index % _spawnPoints.Length];
            var pos= spawnPositions[index % spawnPositions.Count];
            GameObject instance = Instantiate(_playerPrefab, pos, Quaternion.identity);
            instance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

            //Debug.Log($"[Spawn] Player {clientId} → {sp.position}");
            index++;
        }
        this.gameObject.GetComponent<NetworkObject>().Despawn();
    }
}