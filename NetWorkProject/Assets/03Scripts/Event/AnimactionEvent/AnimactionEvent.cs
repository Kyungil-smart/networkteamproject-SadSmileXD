using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AnimactionEvent : MonoBehaviour
{
    [SerializeField]private Object[] m_initObject;
    [SerializeField]private List<AnimactionEventSO> animactionEventSOs;
    private Dictionary<string, AnimactionEventSO> animactionEventSODic = new Dictionary<string, AnimactionEventSO>();
    private void Awake()
    {
        foreach (var animactionEventSO in animactionEventSOs)
        {
            animactionEventSO.Init(this, m_initObject);
        }
        animactionEventSODic= animactionEventSOs.ToDictionary(x=> x.EventName, x => x);
    }

   public void OnAnimationEvent(string eventName)
    {
        if (animactionEventSODic.TryGetValue(eventName, out AnimactionEventSO animactionEventSO))
        {
            animactionEventSO.OnAnimationEvent();
        }
    }
}
