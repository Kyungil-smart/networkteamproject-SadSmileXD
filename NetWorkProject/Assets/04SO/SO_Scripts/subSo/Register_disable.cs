using UnityEngine;
[CreateAssetMenu(fileName = "Register", menuName = "Scriptable Objects/Register/disable")]
public class Register_disable : Register
{
    private GameObject transparencyObj;
    public override void Init(MonoBehaviour owner, Object[] objs)
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
            transparencyObj.SetActive(true);
    }
}
