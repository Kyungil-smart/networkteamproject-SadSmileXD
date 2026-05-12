using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody m_rigid;
    [SerializeField] private GameObject m_Camera;
    [SerializeField] private List<PlayerBase> m_PlayerData = new List<PlayerBase>();
    [SerializeField] private PlayerRun m_Run;
    [SerializeField] private PlayerJump m_Jump;
    [Header("Settings")]
    private float moveSpeed => m_Run.moveSpeed;
    
    private Vector2 currentInput=> m_Run.currentInput;
    private bool isJumping => m_Jump.isJumping;
    [SerializeField] private float maxFallSpeed = -9.81f;
    [SerializeField] private float airControlMultiplier = 0.5f;
    public override void OnNetworkSpawn()
    {
        Debug.Log("OnNetworkSpawn");
        // 1. 내가 조종하는 내 캐릭터일 때 (권한 확인 완료된 시점)
        if (IsOwner)
        {
            Debug.Log("IsOwner");
            InputManager.instance.input.Player.Enable();

            foreach (PlayerBase init_Data in m_PlayerData)
            {
                init_Data.init(null, m_rigid);
            }
            Debug.Log("init_Data");
            foreach (PlayerBase Logic in m_PlayerData)
            {
                Debug.Log("foreach");
                if (Logic is PlayerRun Run_Logic)
                {
                    Debug.Log("(Logic is PlayerRun Run_Logic");
                    InputManager.instance.input.Player.Move.performed += Run_Logic.Execute;
                    InputManager.instance.input.Player.Move.canceled += Run_Logic.Execute;
                }
                else if(Logic is PlayerJump Jump_Logic)
                {
                    InputManager.instance.input.Player.Jump.started += Jump_Logic.Execute;
                }
            }

          
        }
        // 2. 남의 캐릭터일 때
        else
        {
            if (m_Camera == null) return;
            var id = this.gameObject.GetComponentInParent<NetworkObject>().NetworkObjectId;
            spectatormode.Instance.CarmeraPush(id, m_Camera);
            m_Camera.SetActive(false);
        }
    }

    public override void OnNetworkDespawn()
    {
        
        // 오브젝트가 파괴되거나 연결이 끊길 때 이벤트 해제
        if (IsOwner)
        {
            
            foreach (PlayerBase Logic in m_PlayerData)
            {
               
                if (Logic is PlayerRun Run_Logic)
                {
                    
                    InputManager.instance.input.Player.Move.performed -= Run_Logic.Execute;
                    InputManager.instance.input.Player.Move.canceled -= Run_Logic.Execute;
                }
                else if (Logic is PlayerJump Jump_Logic)
                {
                    InputManager.instance.input.Player.Jump.started -= Jump_Logic.Execute;
                }
            }
            

            InputManager.instance.input.Player.Disable();
        }
    }


   
    //   서버 물리 연산 (Server Side)
    private void FixedUpdate()
    {
        const float defaultValue = 1f;
        if (!IsOwner) return;

        // 변경된 부분: 단순히 new Vector3가 아니라, 캐릭터의 앞(forward)과 옆(right)을 기준으로 계산
        // transform.forward: 캐릭터가 바라보는 앞방향
        // transform.right: 캐릭터의 오른쪽 방향
        Vector3 moveDir = (transform.forward * currentInput.y + transform.right * currentInput.x).normalized;

        Vector3 targetVelocity = moveDir * moveSpeed;

        float currentYVelocity = m_rigid.linearVelocity.y;

        // Y축 속도(중력 등) 유지
         targetVelocity.y = Mathf.Max(currentYVelocity, maxFallSpeed);
       
        float control = isJumping ? airControlMultiplier : defaultValue;

        // 최종 속도 적용
        Vector3 MovePos = new Vector3(targetVelocity.x * control, targetVelocity.y, targetVelocity.z * control);
        m_rigid.linearVelocity = MovePos;
    }


}