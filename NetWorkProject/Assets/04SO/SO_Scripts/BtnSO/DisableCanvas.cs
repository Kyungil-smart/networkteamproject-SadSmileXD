using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "btn_DisableCanvas", menuName = "Scriptable Objects/Button/DisableCanvas")]
public class DisableCanvas : ButtonBase
{
    private Button m_btn;
    private GameObject obj1;
  
    public override void ButtonInit(MonoBehaviour OWner, Object[] Obj)
    {
        foreach (Object obj in Obj)
        {
            if (obj is GameObject btn && btn.TryGetComponent(out Button goBtn))
            {
                goBtn.onClick.AddListener(AddButtonListener);

            }
            else if(obj is GameObject gameobj1)
            {
                obj1 = gameobj1;
            }
             
        }

    }
    public override void AddButtonListener()
    {
        obj1.SetActive(false);
        
    }
}
