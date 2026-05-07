using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*
     플레이어 이동
      forward방향으로 이동
    플레이어 점프
    마우스 좌우 이동 으로 카메라플레이어 forward 조정
     */
    [SerializeField] List<PlayerBase> m_PlayerLogic;
    [SerializeField] private Rigidbody m_rigid;
    private void Awake()
    {
        foreach (PlayerBase data in m_PlayerLogic)
        {
            data.init(this, m_rigid);
        }
    }
    private void OnEnable()
    {
        InputManager.instance.input.Player.Enable();
        foreach (PlayerBase data in m_PlayerLogic)
        {
            if(data is PlayerRun run)
            {
                InputManager.instance.input.Player.Move.performed += run.Execute;
                InputManager.instance.input.Player.Move.canceled += run.Execute;
            }
            else if (data is PlayerJump jump)
            {
                InputManager.instance.input.Player.Jump.started += jump.Execute;
            }
        }
        
    }
    private void OnDisable()
    {
        foreach (PlayerBase data in m_PlayerLogic)
        {
            if (data is PlayerRun run)
            {
                InputManager.instance.input.Player.Move.performed -= run.Execute;
                InputManager.instance.input.Player.Move.canceled -= run.Execute;
            }
            else if (data is PlayerJump jump)
            {
                InputManager.instance.input.Player.Jump.started -= jump.Execute;
            }
        }
        InputManager.instance.input.Player.Disable();
    }
   
    

}
