using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;

public class TestDBSave : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Test();
    }

    public void Test()
    {
        DatabaseReference root = BackendManager.Database.RootReference;

        // 기본 자료형을 통한 저장
        string user = BackendManager.Auth.CurrentUser.Email;
        var userId = user.Split("@")[0];
        var userRoot = root.Child("users").Child(userId);

        //userRoot.Child("string").SetValueAsync("텍스트");
        //userRoot.Child("long").SetValueAsync(10);
        //userRoot.Child("double").SetValueAsync(3.14);
        //userRoot.Child("bool").SetValueAsync(true);
        //// List 자료구조를 통한 순차 저장
        //List<string> list = new List<string>() { "첫번째", "두번째", "세번째" };
        //root.Child("List").SetValueAsync(list);

        //// Dictionary 자료구조를 통한 키&값 저장
        //Dictionary<string, object> dictionary = new Dictionary<string, object>()
        // {
        //     { "stringKey", "텍스트" },
        //     { "longKey", 10 },
        //     { "doubleKey", 3.14 },
        //     { "boolKey", true },
        // };
        //root.Child("Dictionary").SetValueAsync(dictionary);

    }
}
