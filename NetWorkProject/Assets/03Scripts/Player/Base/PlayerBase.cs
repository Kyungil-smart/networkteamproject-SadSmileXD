using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBase :NetworkBehaviour 
{
    public abstract void init(NetworkBehaviour Owner,params object[] objects );

     public abstract void Execute(InputAction.CallbackContext context);
     
}
