using NUnit.Framework;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
 

public class UserList : NetworkBehaviour
{
    public NetworkList<FixedString64Bytes> UserName;
    [SerializeField] private GameObject m_parent;
    [SerializeField] private GameObject m_nameText;
    private void Awake()
    {
        UserName = new NetworkList<FixedString64Bytes>(
            readPerm: NetworkVariableReadPermission.Everyone,
            writePerm: NetworkVariableWritePermission.Server
        );
    }
    public override void OnNetworkSpawn()
    { 
         
        UserName.OnListChanged += HandleUserNameChanged;
        PushAllUserList();
      
            string myName = BackendManager.Auth.CurrentUser.Email.Split('@')[0];
            SetUserNameRpc(myName);
      
         
        
    }
    public override void OnNetworkDespawn()
    {
        // 네트워크 오브젝트가 파괴될 때 이벤트 구독 해제 (메모리 누수 방지)
        UserName.OnListChanged -= HandleUserNameChanged;
    }
    public void SetUserName(string name)
    {
        UserName.Add(name);
    }
    private void HandleUserNameChanged(NetworkListEvent<FixedString64Bytes> changeEvent)
    {
        switch (changeEvent.Type)
        {
            case NetworkListEvent<FixedString64Bytes>.EventType.Add:
                PushUser(changeEvent.Value.ToString());

                // TODO: UI에 유저 이름 추가 로직 작성
                break;

            case NetworkListEvent<FixedString64Bytes>.EventType.Remove:
                // 리스트에서 값이 삭제되었을 때
                Debug.Log($"유저 접속 종료! 삭제된 이름: {changeEvent.Value}");
                // TODO: UI에서 유저 이름 제거 로직 작성
                break;

            case NetworkListEvent<FixedString64Bytes>.EventType.Clear:
                // 리스트가 초기화되었을 때
                Debug.Log("모든 유저 목록이 초기화되었습니다.");
                // TODO: UI 목록 초기화 로직 작성
                break;

                // 필요에 따라 Insert, RemoveAt, Value(값 변경) 등도 처리 가능합니다.
        }
    }
    private void PushAllUserList()
    {
        foreach (var item in UserName)
        {
            PushUser(item.ToString());
        }
    }
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void SetUserNameRpc(string name)
    {
        // 서버 측에서 리스트에 추가합니다. 
        // 이때 OnListChanged 이벤트가 모든 클라이언트에게 쫙 뿌려집니다.
        UserName.Add(new FixedString64Bytes(name));
    }
    private void PushUser(string name)
    {
        var data = Instantiate(m_nameText);
        var Namedata = data.GetComponent<TextMeshProUGUI>();
        Namedata.text = name;
        data.transform.SetParent(m_parent.transform);
    }
}
