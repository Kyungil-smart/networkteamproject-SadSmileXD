using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class GetJoinCode : NetworkBehaviour
{
    /*
     호스트 또는 
     */
    [SerializeField]private TextMeshProUGUI _textMeshPro;
    [SerializeField]private Button button;

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            TryInit();

        }
        else
        {
            _textMeshPro.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
        }
    }
    void TryInit()
    {
        if (!string.IsNullOrEmpty(LobbyManager.Instance.JoinCode))
        {
            Settext();
            btn();
        }
        else
        {
            // 아직 안 들어왔으면 기다렸다가 다시 시도
            Invoke(nameof(TryInit), 0.2f);
        }
    }
    private void Settext()
    {
        _textMeshPro.text = LobbyManager.Instance.JoinCode;
    }
    private  void btn()
    {
        button.onClick.AddListener(()=>
        {
            GUIUtility.systemCopyBuffer = _textMeshPro.text;
        });
    }
}
