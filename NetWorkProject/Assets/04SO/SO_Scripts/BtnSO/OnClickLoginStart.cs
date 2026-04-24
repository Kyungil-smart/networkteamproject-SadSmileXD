using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "btn_", menuName = "Scriptable Objects/Button/LoginStart")]
public class OnClickLoginStart : ButtonBase
{
    private TMP_InputField email;
    private TMP_InputField password;
    public override void ButtonInit(MonoBehaviour OWner, UnityEngine.Object[] objects)
    {
        m_Owner = OWner;
        foreach (var obj in objects)
        {
          
           
            if (obj is GameObject btn && btn.TryGetComponent(out Button goBtn))
            {
                 
                 if(goBtn !=null)
                    goBtn.onClick.AddListener(AddButtonListener);
            }
            else if (obj is GameObject inputfield && inputfield.TryGetComponent(out TMP_InputField goInput))
            {
              if(goInput.gameObject.name.Contains("Email_InputField"))
              {
                  email = goInput;
              }
              else if(goInput.gameObject.name.Contains("Password_InputField"))
              {
                  password = goInput;
              }
            }
        }
    }

    public override void AddButtonListener()
    {
        Debug.Log($"이메일 : {email.text}");
        Debug.Log($"비밀번호 :{password.text}");
        BackendManager.Instance.SignIn(email.text, password.text);
    }
   
}
