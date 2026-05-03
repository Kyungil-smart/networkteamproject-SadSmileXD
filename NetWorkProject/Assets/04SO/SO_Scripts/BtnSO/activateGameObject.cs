using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "btn_", menuName = "Scriptable Objects/Button/ObjectActive")]
public class activateGameObject: ButtonBase
{
    private GameObject canvas;
    private IDisable m_disable;
    public override void ButtonInit(MonoBehaviour OWner, Object[] Obj)
    {
         m_Owner = OWner;
        foreach(var item  in Obj)
        {
            if(item is Button btn)
            {
                btn.onClick.AddListener(AddButtonListener);
            }
            else if(item is GameObject obj)
            {
                if(obj.TryGetComponent<IDisable>(out var disableComponent))
                {
                    m_disable= disableComponent;

                }
                else
                {
                    canvas = obj;
                }
                
            }

        }
    }
    public override void AddButtonListener()
    {
        canvas.SetActive(!canvas.activeSelf);
        m_disable.Disable();
    }
}
