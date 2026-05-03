using System;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "Register", menuName = "Scriptable Objects/Register/disable")]
public class Register_Active : Register
{
    private GameObject transparencyObj;
    [SerializeField]private bool flag;
    public override void Init(MonoBehaviour owner, UnityEngine.Object[] objs)
    {
        transparencyObj = null;
        foreach(var obj in objs)
        {
            if(obj is GameObject go) 
            {
                transparencyObj = go;
            }
        }
    }
    public override void subscribe()
    {
        if(transparencyObj!=null)
            transparencyObj.SetActive(flag);
    }
}
