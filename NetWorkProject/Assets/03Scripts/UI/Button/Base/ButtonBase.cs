using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonBase : ScriptableObject
{
    [TextArea][SerializeField] private string description;
    protected MonoBehaviour m_Owner;
    public abstract void ButtonInit(MonoBehaviour OWner, UnityEngine.Object[] Obj);
    public abstract void AddButtonListener();
}
