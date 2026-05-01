using UnityEngine;

/// <summary>
/// [CreateAssetMenu(fileName = "Register", menuName = "Scriptable Objects/Register")]
/// </summary>
public abstract class Register : ScriptableObject
{
  
    [SerializeField] public SubscribeType enumType;
    [TextArea]
    [SerializeField] private string description;
    public abstract void Init(MonoBehaviour owner, Object[] objs);

    public abstract void subscribe();
   
  
}
