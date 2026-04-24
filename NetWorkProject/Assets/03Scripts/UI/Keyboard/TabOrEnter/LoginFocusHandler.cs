using UnityEngine;
using System;
using Unity.VisualScripting;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class LoginFocusHandler : MonoBehaviour
{
    [SerializeField] private List<TMP_InputField> MP_InputFields;
    
    private int CurrentIndex=0;
    private void Awake()
    {
       
        InitializeInputFields();
    }
    private void OnEnable()
    {
        InputManager.instance.input.LoginScene.Enable();
        InputManager.instance.input.LoginScene.Tab.started += ChanageFocus;
      

    }
    private void OnDisable()
    {
        InputManager.instance.input.LoginScene.Tab.started -= ChanageFocus;
        InputManager.instance.input.LoginScene.Disable();
    }

    public void ChanageFocus(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if (MP_InputFields.Count == 0) return;
            CurrentIndex = (CurrentIndex + 1) % MP_InputFields.Count;
            MP_InputFields[CurrentIndex].Select();
        }
    }
    public void InitializeInputFields()
    {
        for(int i=0; i<MP_InputFields.Count;i++)
        {
            int index = i;
            MP_InputFields[i].onSelect.AddListener((string _)=>
            {
                CurrentIndex = index;
            });
            

        }
          
    }
    
}
