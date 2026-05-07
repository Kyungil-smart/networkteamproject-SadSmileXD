using Unity.Netcode;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "OnNextScene",menuName = "Scriptable Objects/Button/OnNextScene")]
public class OnNextScene:ButtonBase
{
    private Button m_btn;
    public string SceneName;
    public override void ButtonInit(MonoBehaviour OWner, Object[] Objs)
    {
        foreach (Object obj in Objs)
        {
            if(obj is GameObject gameobject && gameobject.TryGetComponent<Button>(out Button btn))
            {
                m_btn = btn;
                m_btn.onClick.AddListener(AddButtonListener);
            }
        }
    }

    public override void AddButtonListener()
    {
        Debug.Log($"진입함: {SceneName}");
        NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }
}
