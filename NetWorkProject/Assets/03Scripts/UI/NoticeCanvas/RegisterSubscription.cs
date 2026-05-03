using UnityEngine;

public class RegisterSubscription : MonoBehaviour
{
    /*
     SubscribeManager를 사용해서 결합도는 낮추기 성공 했지만
    얘한테 등록한 메소드를 만들 때 불필요한 클래스 들이 많이 생겨
    행동 및 로직 분리하는 해서 행동에서는 등록된 로직을 그냥 구독 시켜버리는 작업만 하게 만들 예정

     
     */
    [SerializeField]private Register m_register;
    [SerializeField]private Object[] m_objects;
    private SubscribeType m_type => m_register.enumType;
    private void Awake()
    {
        if(m_register!=null)
             m_register.Init(this, m_objects);
    }
    private void OnEnable()
    {
        SubscribeManager.instance.Subscribe(m_type, m_register.subscribe);
    }

    private void OnDisable()
    {
        SubscribeManager.instance.Unsubscribe(m_type, m_register.subscribe);
    }
}
