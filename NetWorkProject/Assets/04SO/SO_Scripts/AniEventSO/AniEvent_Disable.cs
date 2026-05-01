using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AniEvent_Disable", menuName = "Scriptable Objects/AnimationEvents/AniEvent_Disable")]
public class AniEvent_Disable : AnimactionEventSO
{
    private List<GameObject> _objectsToDisable = new List<GameObject>();
    public override void Init(MonoBehaviour Owner, Object[] args)
    {
         foreach (var obj in args)
        {
            if (obj is GameObject gameObject)
            {
                _objectsToDisable.Add(gameObject);
            }
            
        }
    }
    public override void OnAnimationEvent()
    {
         foreach(var obj in _objectsToDisable)
         {
            obj.SetActive(false);
         }
    }
}
