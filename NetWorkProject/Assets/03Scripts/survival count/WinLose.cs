using Unity.Netcode;
using UnityEngine;

public class WinLose : MonoBehaviour
{
    public static WinLose instance { get; private set; }
    public GameObject WinObject;
    public GameObject LoseObject;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void OnEnable()
    {
        SubscribeManager.instance.Subscribe(SubscribeType.OnWinCanvas, OnWinObject);
        SubscribeManager.instance.Subscribe(SubscribeType.OnLoseCanvas, OnLoseObject);
        SubscribeManager.instance.Subscribe(SubscribeType.OFFWinCanvas, OFFWinObject);
        SubscribeManager.instance.Subscribe(SubscribeType.OFFLoseCanvas, OFFLoseObject);    
    }
    private void OnDisable()
    {
        SubscribeManager.instance.Unsubscribe(SubscribeType.OnWinCanvas, OnWinObject);
        SubscribeManager.instance.Unsubscribe(SubscribeType.OnLoseCanvas, OnLoseObject);
        SubscribeManager.instance.Unsubscribe(SubscribeType.OFFWinCanvas, OFFWinObject);
        SubscribeManager.instance.Unsubscribe(SubscribeType.OFFLoseCanvas, OFFLoseObject);
    }

    public void OnWinObject()
    {
        Debug.Log("OnWinObject");
        WinObject.SetActive(true);
    }
    public void OFFWinObject()
    {
        WinObject.SetActive(false);
    }
    public void OnLoseObject()
    {
        Debug.Log("OnLoseObject");
        LoseObject.SetActive(true);
    }
    public void OFFLoseObject()
    {
        LoseObject.SetActive(false);
    }

}
