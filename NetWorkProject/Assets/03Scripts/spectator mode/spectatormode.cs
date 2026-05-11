using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class spectatormode : MonoBehaviour
{
    public static spectatormode Instance { get; private set; }
    Dictionary<int ,ulong> m_Cameradic=new Dictionary<int, ulong>();
    [SerializeField]private int count = 0;
    private Dictionary<ulong,GameObject> m_CarmeraOnject=new Dictionary<ulong, GameObject>();
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
         
         
    }
    private void Update()
    {
        //ToDo 테스트용 코드 이므로 나중에 inputsystem 사용하기 
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("카메라 활성화");
            var id=m_Cameradic[0];
            var obj = m_CarmeraOnject[id];
            var camera=obj.GetComponent<Camera>();
            camera.depth = 3;
            obj.SetActive(true);
        }
    }
    public void CarmeraPush(ulong id, GameObject obj)
    {
        Debug.Log($"카메라 id : {id}");
        m_CarmeraOnject.Add(id, obj);
        m_Cameradic.Add(count++, id);
        Debug.Log($"삽입개수 :{m_CarmeraOnject.Count}");
    }
    public void DeleteCarmera(ulong id)
    {
         
        m_CarmeraOnject.Remove(id);
    }

}
