using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class LoginEnter : MonoBehaviour
{
    [SerializeField] private List<TMP_InputField> m_inputFields;
    
    private void Awake()
    {
        foreach(var item in m_inputFields)
        {
            item.onSubmit.AddListener((string _)=>
            {
                SubscribeManager.instance.Publish(SubscribeType.LoginEnter);
                
            });
           
        }
       
    }

}
