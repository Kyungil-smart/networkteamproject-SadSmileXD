using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }
    private InputSystem_Actions m_input;
    public InputSystem_Actions input
    {
        get => m_input;
       
    }
    private void Awake()
    {
        m_input= new InputSystem_Actions();
        if(instance ==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
}
