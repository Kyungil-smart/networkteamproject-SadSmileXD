using System;
using System.Collections.Generic;
using UnityEngine;

public enum SubscribeType
{
    None,
    LoginEnter,    // 데이터 없는 로그인 엔터
    OnLoading,//로딩 시작
    OnLoadingComplete,//로딩 끝

    //==================
    EmailVerificationRequired,//활성화 이메일 인증 하라는 창
    DeleteBtnActive, //계정 삭제
    //==================
    SendMessage,
    //=============
    AddUserName,

}

public class SubscribeManager : MonoBehaviour
{
    public static SubscribeManager instance { get; private set; }

    // Action(인자 없음)과 Action<T>(인자 있음)을 모두 담기 위해 Delegate 사용
    private Dictionary<SubscribeType, Delegate> subscriptions = new Dictionary<SubscribeType, Delegate>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region 인자가 없는 구독 (Action)
    public void Subscribe(SubscribeType type, Action action)
    {
        if (subscriptions.ContainsKey(type))
            subscriptions[type] = (Action)subscriptions[type] + action;
        else
            subscriptions[type] = action;
    }

    public void Unsubscribe(SubscribeType type, Action action)
    {
        if (subscriptions.ContainsKey(type))
        {
            subscriptions[type] = (Action)subscriptions[type] - action;
            if (subscriptions[type] == null) subscriptions.Remove(type);
        }
    }

    public void Publish(SubscribeType type)
    {
        if (subscriptions.ContainsKey(type) && subscriptions[type] is Action action)
        {
            action.Invoke();
        }
    }
    #endregion

    #region 인자가 있는 구독 (Action<T>)
    public void Subscribe<T>(SubscribeType type, Action<T> action)
    {
        if (subscriptions.ContainsKey(type))
            subscriptions[type] = (Action<T>)subscriptions[type] + action;
        else
            subscriptions[type] = action;
    }

    public void Unsubscribe<T>(SubscribeType type, Action<T> action)
    {
        if (subscriptions.ContainsKey(type))
        {
            subscriptions[type] = (Action<T>)subscriptions[type] - action;
            if (subscriptions[type] == null) subscriptions.Remove(type);
        }
    }

    public void Publish<T>(SubscribeType type, T data)
    {
        if (subscriptions.ContainsKey(type) && subscriptions[type] is Action<T> action)
        {
            action.Invoke(data);
        }
    }
    #endregion
}