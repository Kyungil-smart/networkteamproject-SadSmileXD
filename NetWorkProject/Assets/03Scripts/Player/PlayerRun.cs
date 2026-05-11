using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerRun : PlayerBase
{
    [Header("Components")]
    private Rigidbody m_rigid;
    [SerializeField] private PlayerJump m_jump;
    
   
    [Header("Settings")]
    [SerializeField] public float moveSpeed = 5f; // 이동 속도
    public Vector2 currentInput;

//////////////////////////////////////

    public override void init(NetworkBehaviour Owner, params object[] objects)
    {
        foreach (object obj in objects)
        {
            if(obj is Rigidbody rigid)
            {
                m_rigid = rigid;
            }
        }
    }
 
    public override void Execute(InputAction.CallbackContext context)
    {

        Vector2 inputDir = context.ReadValue<Vector2>();
        // 내 입력값을 서버로 전송!
        MoveServerRpc(inputDir);

    }

    [ServerRpc]
    private void MoveServerRpc(Vector2 inputDir)
    {

        // 서버가 클라이언트의 입력 방향을 저장해둠
        currentInput = inputDir;
    }

}
