using Unity.VisualScripting;
using UnityEngine;

public class EnterSubscribe : MonoBehaviour
{
    [SerializeField] private ButtonClick btnClick;

    private void OnEnable()
    {
        SubscribeManager.instance.Subscribe(SubscribeType.LoginEnter, btnClick.data.AddButtonListener);
    }
    private void OnDisable()
    {
        SubscribeManager.instance.Unsubscribe(SubscribeType.LoginEnter, btnClick.data.AddButtonListener);
    }

}
