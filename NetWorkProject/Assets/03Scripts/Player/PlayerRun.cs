using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerRun : PlayerBase
{
    private Rigidbody m_rigid;
    private Vector3 moveDir; // 입력받은 방향을 저장할 변수
    private float moveSpeed = 5f; // 이동 속도
    Vector3 input=new();
    [SerializeField] private PlayerJump m_jump;
    public override void init(MonoBehaviour Owner, params object[] objects)
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
       
        if (context.performed)
        {
            input = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            input = context.ReadValue<Vector2>();
        }

        // 2. 3D 이동에 맞게 X, Z축으로 변환하여 저장
        moveDir = new Vector3(input.x, 0, input.y).normalized;
    }
    //public void FixedUpdate()
    //{
    //    if (m_jump.isJumping) return;
    //    Vector3 targetVelocity = moveDir * moveSpeed;
    //    targetVelocity.y = m_rigid.linearVelocity.y; // 중력 유지
    //    m_rigid.linearVelocity = targetVelocity;
    //}
    

}
