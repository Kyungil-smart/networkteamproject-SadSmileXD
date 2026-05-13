using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody m_rigid;
    [SerializeField] private GameObject m_Camera;
    [SerializeField] private List<PlayerBase> m_PlayerData = new List<PlayerBase>();
    [SerializeField] private PlayerRun m_Run;
    [SerializeField] private PlayerJump m_Jump;
    [SerializeField] private AudioListener m_AudioListener;

    [Header("Settings")]
    private float moveSpeed => m_Run.moveSpeed;
    private Vector2 currentInput => m_Run.currentInput;
    private bool isJumping => m_Jump.isJumping;
    [SerializeField] private float maxFallSpeed = -9.81f;
    [SerializeField] private float airControlMultiplier = 0.5f;

    public NetworkVariable<ulong> customPlayerId = new NetworkVariable<ulong>(0);

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // 1. 서버가 고유 ID 부여
            customPlayerId.Value = (ulong)Random.Range(1000, 9999) + OwnerClientId;
            // 2. 서버 매니저에게 즉시 등록
            SubscribeManager.instance.Publish(SubscribeType.PlayerSpawnCountUp, customPlayerId.Value);
        }

        if (IsOwner)
        {
            m_rigid.sleepThreshold = 0f;
            m_rigid.WakeUp();
            Debug.Log($"나의 아이디 : {customPlayerId.Value}");
            // 3. 승리/패배 공보 구독
            SubscribeManager.instance.Subscribe<ulong>(SubscribeType.PlayerWinLose, OnGameFinished);

            InputManager.instance.input.Player.Enable();
            foreach (PlayerBase init_Data in m_PlayerData) init_Data.init(null, m_rigid);

            foreach (PlayerBase Logic in m_PlayerData)
            {
                if (Logic is PlayerRun Run_Logic)
                {
                    InputManager.instance.input.Player.Move.performed += Run_Logic.Execute;
                    InputManager.instance.input.Player.Move.canceled += Run_Logic.Execute;
                }
                else if (Logic is PlayerJump Jump_Logic)
                {
                    InputManager.instance.input.Player.Jump.started += Jump_Logic.Execute;
                }
            }
        }
        else
        {
            // 남의 캐릭터 설정
            if (m_Camera != null) m_Camera.SetActive(false);
            if (m_AudioListener != null) m_AudioListener.enabled = false;
        }
    }

    // [중요] 데드존 등에서 호출할 탈락 함수
    public void Die()
    {
        if (IsOwner)
        {
            if (customPlayerId.Value == 0)
            {
                Debug.LogWarning("ID가 아직 할당되지 않아 탈락 처리를 보류합니다.");
                return;
            }
            Debug.Log("내가 탈락함 - 서버에 보고합니다.");
            // 서버에 내 ID를 보내서 리스트에서 지워달라고 함
            ReportDeathServerRpc(customPlayerId.Value);
        }
    }

    [Rpc(SendTo.Server,InvokePermission = RpcInvokePermission.Everyone)]
    private void ReportDeathServerRpc(ulong id)
    {
        Debug.Log("ReportDeathServerRpc 실행");
        // 서버의 survivalCount가 이 이벤트를 듣고 리스트에서 지움
        SubscribeManager.instance.Publish(SubscribeType.PlayerSpawnCountDown, id);
    }

    private void OnGameFinished(ulong winnerId)
    {
        Time.timeScale = 0;
        // 서버에서 온 우승자 ID와 내 ID 비교
        if (winnerId == customPlayerId.Value)
        {
            Debug.Log("<color=cyan>★ 우승: 당신이 최후의 생존자입니다! ★</color>");
            // 승리 UI 처리
        }
        else
        {
            Debug.Log("<color=red>☆ 패배: 당신은 우승하지 못했습니다. ☆</color>");
            // 패배 UI 처리
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            SubscribeManager.instance.Unsubscribe<ulong>(SubscribeType.PlayerWinLose, OnGameFinished);
            InputManager.instance.input.Player.Disable();
            // 이벤트 해제 로직 생략...
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        Vector3 moveDir = (transform.forward * currentInput.y + transform.right * currentInput.x).normalized;
        Vector3 targetVelocity = moveDir * moveSpeed;
        float currentYVelocity = m_rigid.linearVelocity.y;
        targetVelocity.y = Mathf.Max(currentYVelocity, maxFallSpeed);
        float control = isJumping ? airControlMultiplier : 1f;
        m_rigid.linearVelocity = new Vector3(targetVelocity.x * control, targetVelocity.y, targetVelocity.z * control);
    }
}