using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ButtonAnimationActionSO", menuName = "Scriptable Objects/Button/ButtonAnimationActionSO")]
public class ButtonAnimationActionSO : ButtonBase
{
    private Animator m_animator;
    private Button m_button;
    private TextMeshProUGUI m_text;
    bool m_flag;
    public override void ButtonInit(MonoBehaviour OWner, Object[] Objs)
    {
         foreach (var obj in Objs)
        {
            if (obj is GameObject gameOBJ && gameOBJ.TryGetComponent<Animator>(out Animator getAnimator))
            {
               if(getAnimator != null)
               {
                    m_animator=getAnimator;
               }
            }
            else if(obj is GameObject gameOBJ2 && gameOBJ2.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI getText))
            {
                if(getText !=null)
                {
                    m_text=getText;
                }
            }
            else if(obj is Button btn)
            {
                m_button = btn;
                m_button.onClick.AddListener(AddButtonListener);
            }
        }
    }
    public override void AddButtonListener()
    {
       m_flag = !m_flag;
       m_animator.SetBool("UIFlag", m_flag);
       m_text.text = m_flag ? "펼치기" : "접기";
    }
}
