using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class initBreak : MonoBehaviour
{
    public List<GameObject> objs=new();
   public void OnEnable()
    {
        SubscribeManager.instance.Subscribe(SubscribeType.dontDestroyBreak, breakScene);
    }
    public void OnDisable()
    {
        SubscribeManager.instance.Unsubscribe(SubscribeType.dontDestroyBreak, breakScene);
    }

     private void breakScene()
    {

#if UNITY_EDITOR
        // 유니티 에디터에서 실행 중일 때
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 실제 빌드된 게임에서 실행 중일 때
        Application.Quit();
#endif
    }
}
