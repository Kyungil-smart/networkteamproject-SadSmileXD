using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerRun : PlayerBase
{
    [Header("Components")]
    private Rigidbody m_rigid;
    [SerializeField] private PlayerJump m_jump;
    [SerializeField] private Animator m_animator;
   
    [Header("Settings")]
    [SerializeField] public float moveSpeed = 5f; // 이동 속도
    public Vector2 currentInput;
    private NetworkVariable<bool> m_IsWalking = new NetworkVariable<bool>(
      false,
      NetworkVariableReadPermission.Everyone,
      NetworkVariableWritePermission.Owner
  );

    //////////////////////////////////////

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        m_IsWalking.OnValueChanged += OnWalkingStateChanged;
    }

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
        if (!IsOwner) return;

        Vector2 inputDir = context.ReadValue<Vector2>();
        // 내 입력값을 서버로 전송!
        currentInput = inputDir;

        if (context.performed)
        {
            m_IsWalking.Value = true;
        }
        else
        {
            m_IsWalking.Value = false;

        }
    }

    private void OnWalkingStateChanged(bool previousValue, bool newValue)
    {
        m_animator.SetBool("isRun",newValue);
    }


}
