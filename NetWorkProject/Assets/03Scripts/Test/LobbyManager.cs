using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }
    public string SceneName;
    public string JoinCode;
 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(gameObject);

        }
        InitializeHostLobby();
 
    }

    // ✅ Host 시작 후 호출
    public void InitializeHostLobby()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }
    
    [ContextMenu("dsds")]//   Host 시작 완료 시 실행]
    private void OnServerStarted()
    {
        Debug.Log("[Lobby] 서버 시작 → 로비 씬 로드");
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
       
        NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
       
    }

    //   유저 접속
    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"[Lobby] 유저 접속: {clientId}");

        Debug.Log($"현재 인원: {NetworkManager.Singleton.ConnectedClientsList.Count}");
    }

    //   유저 퇴장
    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"[Lobby] 유저 퇴장: {clientId}");

        Debug.Log($"현재 인원: {NetworkManager.Singleton.ConnectedClientsList.Count}");
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }
}