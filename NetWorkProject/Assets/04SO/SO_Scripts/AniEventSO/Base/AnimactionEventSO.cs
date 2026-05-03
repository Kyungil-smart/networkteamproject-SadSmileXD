using UnityEngine;

public abstract class AnimactionEventSO :ScriptableObject
{
    public string EventName;
    public abstract void Init(MonoBehaviour Owner, Object[] args);

    public abstract void OnAnimationEvent();
 
}
