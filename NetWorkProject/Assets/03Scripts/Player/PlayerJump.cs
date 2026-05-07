using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : PlayerBase
{
    [SerializeField] private float JumpForce;
    private Rigidbody m_rigid;
    public bool isJumping;
    public override void init(MonoBehaviour Owner, params object[] objects)
    {
        isJumping = false;
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
         if(context.started)
        {
            m_rigid.AddForce(Vector3.up* JumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("floor"))
        {
            isJumping = false;
        }
    }

}
