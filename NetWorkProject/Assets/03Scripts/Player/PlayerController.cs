using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody m_rigid;
    [SerializeField] private GameObject m_Camera;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    // 서버에서 물리 연산을 하기 위해 클라이언트의 입력값을 기억할 변수
    private Vector2 currentInput;
    private bool isJumping = false;
    [SerializeField] private float maxFallSpeed = -9.81f;
    public override void OnNetworkSpawn()
    {
        // 1. 내가 조종하는 내 캐릭터일 때 (권한 확인 완료된 시점)
        if (IsOwner)
        {
            InputManager.instance.input.Player.Enable();

            // 이동 입력 이벤트 연결
            InputManager.instance.input.Player.Move.performed += OnMoveInput;
            InputManager.instance.input.Player.Move.canceled += OnMoveInput;

            // 점프 입력 이벤트 연결
            InputManager.instance.input.Player.Jump.started += OnJumpInput;
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
            InputManager.instance.input.Player.Move.performed -= OnMoveInput;
            InputManager.instance.input.Player.Move.canceled -= OnMoveInput;
            InputManager.instance.input.Player.Jump.started -= OnJumpInput;

            InputManager.instance.input.Player.Disable();
        }
    }

    // ==========================================
    // 1. 클라이언트 입력 처리 (Client Side)
    // ==========================================
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
         
        Vector2 inputDir = context.ReadValue<Vector2>();
        // 내 입력값을 서버로 전송!
        MoveServerRpc(inputDir);
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        // 점프 버튼을 누르면 서버에 점프하라고 전송!
        JumpServerRpc();
    }

    // ==========================================
    // 2. 서버 RPC 통신 (Client -> Server)
    // ==========================================
    [ServerRpc]
    private void MoveServerRpc(Vector2 inputDir)
    {
     
        // 서버가 클라이언트의 입력 방향을 저장해둠
        currentInput = inputDir;
    }

    [ServerRpc]
    private void JumpServerRpc()
    {
        // 서버 측에서 점프 조건 확인 후 물리 힘 가하기
        if (!isJumping)
        {
            m_rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    // ==========================================
    // 3. 서버 물리 연산 (Server Side)
    // ==========================================
    private void FixedUpdate()
    {
        if (!IsServer) return;
         
        if (isJumping) return;

        Vector3 moveDir = new Vector3(currentInput.x, 0, currentInput.y).normalized;
        Vector3 targetVelocity = moveDir * moveSpeed;

        // ⬇️ 수정된 부분: 중력 유지 및 최대 낙하 속도 제한
        float currentYVelocity = m_rigid.linearVelocity.y;

        // 떨어지는 속도(음수)가 maxFallSpeed(-15)보다 더 낮아지지 않도록(빨라지지 않도록) 방어
        targetVelocity.y = Mathf.Max(currentYVelocity, maxFallSpeed);

        m_rigid.linearVelocity = targetVelocity;
    }

    // ==========================================
    // 4. 기타 서버 물리 충돌 처리 (Server Side)
    // ==========================================
    private void OnTriggerEnter(Collider other)
    {
        // 바닥 충돌 판정 역시 서버에서만 처리하는 것이 안전합니다.
        if (!IsServer) return;

        if (other.CompareTag("floor"))
        {
            isJumping = false;
        }
    }
}