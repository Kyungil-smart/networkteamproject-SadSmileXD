using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "GameObjectActivator", menuName = "Scriptable Objects/Button/GameObjectActivator")]
public class GameObjectActivator : ButtonBase
{
    private List<GameObject> m_object=new();
    
    public bool flag;
    public override void ButtonInit(MonoBehaviour OWner, Object[] Objs)
    {
         foreach (Object obj in Objs)
         {
            if (obj is GameObject _object && _object.TryGetComponent(out Button btn))
            {
                btn.onClick.AddListener(AddButtonListener);
            }
            else if (obj is GameObject gameObj)
            {
                m_object.Add(gameObj);
            }
            
         }
    }

    public override void AddButtonListener()
    {
        foreach(GameObject obj in m_object)
        {
            obj.SetActive(flag);
        }
    }
}
