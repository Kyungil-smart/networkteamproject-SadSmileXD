using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToLobbyManager : MonoBehaviour
{
    void Start()
    {
        // 1. NetworkManager가 살아있는지 먼저 확인
        if (NetworkManager.Singleton != null)
        {
            // 2. 현재 내가 '호스트'로서 네트워크를 유지 중인지 확인
            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log("★ 호스트 접속 상태 확인됨: 방장 전용 UI나 로직을 실행합니다.");
                ExecuteHostLogic();
            }
            else
            {
                Debug.Log("☆ 클라이언트 접속 상태: 방장을 기다립니다.");
            }
        }
    }

    private void ExecuteHostLogic()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("TestLoad", LoadSceneMode.Additive);
    }
}