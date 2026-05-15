using NUnit.Framework;
using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : PlayerBase
{
    [SerializeField] private float JumpForce;
    private Rigidbody m_rigid;
    public bool isJumping;
    public NetworkAnimator m_animator;
    public override void init(NetworkBehaviour Owner, params object[] objects)
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
            if (isJumping) return;
            m_rigid.AddForce(Vector3.up* JumpForce, ForceMode.Impulse);
            isJumping = true;
            m_animator.SetTrigger("OnJump");
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
