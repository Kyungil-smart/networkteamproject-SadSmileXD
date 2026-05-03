using Unity.VisualScripting;
using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    private void OnEnable()
    {
        SubscribeManager.instance.Subscribe(SubscribeType.OnLoading, OnLoading);
        SubscribeManager.instance.Subscribe(SubscribeType.OnLoadingComplete, OnLoadingComplete);
    }

    private void OnDisable()
    {
        SubscribeManager.instance.Unsubscribe(SubscribeType.OnLoading, OnLoading);
        SubscribeManager.instance.Unsubscribe(SubscribeType.OnLoadingComplete, OnLoadingComplete);

    }

    private void OnLoading()
    {
        loadingPanel.SetActive(true);
    }
    private void OnLoadingComplete()
    {
        loadingPanel.SetActive(false);
    }

}
