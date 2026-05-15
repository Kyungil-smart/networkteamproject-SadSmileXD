using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ChatSendServer : NetworkBehaviour
{
    [SerializeField] private GameObject m_gameObject;
    [SerializeField] private GameObject m_chatObject;
    private Queue<GameObject> m_MessageQueue;
    private void Awake()
    {
        m_MessageQueue = new Queue<GameObject>(20);



    }
    private void OnEnable()
    {
        SubscribeManager.instance.Subscribe<string>(SubscribeType.SendMessage,SendMessageRpc);
    }
    private void OnDisable()
    {
        SubscribeManager.instance.Unsubscribe<string>(SubscribeType.SendMessage, SendMessageRpc);
    }
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone )]
    public void SendMessageRpc(string text)
    {
        Debug.Log("SendMessageRpc들어옴");
        FixedString128Bytes msg= new FixedString128Bytes(text);

        ReceiveMessageRpc(msg);
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void ReceiveMessageRpc(FixedString128Bytes message)
    {
        Debug.Log("ReceiveMessageRpc");
        var chatobject=Instantiate(m_chatObject);
        chatobject.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI chatText);
        chatText.text= message.ToString();
        chatText.transform.SetParent(m_gameObject.transform);
        Push(chatobject);

    }

    private void Push(GameObject obj)
    {
        int MAX_MSG = 20;
        if (m_MessageQueue.Count>= MAX_MSG)
        {
            var MSG =    m_MessageQueue.Dequeue();
            Destroy(MSG);
        }
        m_MessageQueue.Enqueue(obj);
    }
}
