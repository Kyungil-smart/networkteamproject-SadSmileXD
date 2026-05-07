using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBase : MonoBehaviour
{
    public abstract void init(MonoBehaviour Owner,params object[] objects );
    public abstract void Execute(InputAction.CallbackContext context);
     
}
