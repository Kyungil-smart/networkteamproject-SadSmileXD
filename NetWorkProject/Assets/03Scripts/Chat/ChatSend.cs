using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ChatSend : NetworkBehaviour
{
    [SerializeField] private TMP_InputField m_InputField;
    private void Awake()
    {
        AddFunc();
    }

 
    private void AddFunc()
    {
        if(m_InputField ==null)
        {
            Debug.Log($"{this.gameObject} : TMP_InputField NULL");
            return;
        }

        m_InputField.onSubmit.AddListener((string text) =>
        {
            if (text.Length == 0) return;
            var Userid = BackendManager.Auth.CurrentUser.Email.Split("@")[0];
            var ChatText = $"{Userid}:{text}";
            SubscribeManager.instance.Publish<string>(SubscribeType.SendMessage, ChatText);
            m_InputField.text = "";
            StartCoroutine(ReFocus());
        });
    }
    private IEnumerator ReFocus()
    {
        yield return null;
        m_InputField.ActivateInputField();
    }
}
