using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] private UnityEngine.Object[] initObj;
    [SerializeField] private ButtonBase m_data;
    public ButtonBase data => m_data;
    [TextArea][SerializeField]private string Destription;
    private void Awake()
    {
        if(m_data !=null)
        {
            m_data = Instantiate(m_data);
            m_data.ButtonInit(this, initObj);
        }
    }
   

}
