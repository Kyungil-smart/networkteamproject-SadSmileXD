using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
[CreateAssetMenu(fileName = "btn_OnSignUp", menuName = "Scriptable Objects/Button/OnSignUp")]
public class OnSignUp : ButtonBase
{
    /*
     버튼을 눌렀을 때 회원가입 시도하는 기능입니다.
     */
    private TMP_InputField email;
    private TMP_InputField password;
    public override void ButtonInit(MonoBehaviour OWner, Object[] objects)
    {
        m_Owner = OWner;
        foreach (var obj in objects)
        {


            if (obj is GameObject btn && btn.TryGetComponent(out Button goBtn))
            {

                if (goBtn != null)
                    goBtn.onClick.AddListener(AddButtonListener);
            }
            else if (obj is GameObject inputfield && inputfield.TryGetComponent(out TMP_InputField goInput))
            {
                if (goInput.gameObject.name.Contains("Email_InputField"))
                {
                    email = goInput;
                }
                else if (goInput.gameObject.name.Contains("Password_InputField"))
                {
                    password = goInput;
                }
            }
        }
    }

    public override void AddButtonListener()
    {
        //Debug.Log("회원가입 시도");
        //Debug.Log($"{email.text}");
        //Debug.Log($"{password.text}");
        
        BackendManager.Instance.SignUp(email.text, password.text);
    }
}
