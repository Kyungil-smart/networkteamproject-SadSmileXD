using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MyProject.CustomTools
{
    public class SOListEditorWindow : EditorWindow
    {
        private string folderPath = "Assets/04SO/Data/";
        private List<ScriptableObject> soList = new List<ScriptableObject>();

        private ScriptableObject selectedSO;
        private SerializedObject serializedObject;

        private Vector2 leftScrollPos;
        private Vector2 rightScrollPos;

        [MenuItem("Tools/SO/SO List Manager")]
        public static void ShowWindow()
        {
            SOListEditorWindow window = GetWindow<SOListEditorWindow>("SO Manager");
            window.position = new Rect(100, 100, 900, 700);
            window.minSize = new Vector2(750, 500); // 최소 크기 지정
        }

        private void OnGUI()
        {
            DrawTopBar();
            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            DrawLeftList();
            DrawRightDetail();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawTopBar()
        {
            EditorGUILayout.BeginVertical("helpbox");
            GUILayout.Label("📂 데이터 관리 컨트롤러", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            folderPath = EditorGUILayout.TextField("대상 폴더", folderPath);
            if (GUILayout.Button("선택", GUILayout.Width(50)))
            {
                string path = EditorUtility.OpenFolderPanel("SO 폴더 선택", "Assets", "");
                if (!string.IsNullOrEmpty(path) && path.Contains(Application.dataPath))
                {
                    folderPath = "Assets" + path.Replace(Application.dataPath, "");
                }
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("데이터 새로고침", GUILayout.Height(25)))
            {
                RefreshSOList();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawLeftList()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.Width(250), GUILayout.ExpandHeight(true));
            GUILayout.Label("SO 리스트", EditorStyles.centeredGreyMiniLabel);
            leftScrollPos = EditorGUILayout.BeginScrollView(leftScrollPos);

            foreach (var so in soList)
            {
                if (so == null) continue;

                // GUI.skin.button 사용하여 스타일 에러 방지
                GUIStyle style = new GUIStyle(GUI.skin.button);
                style.alignment = TextAnchor.MiddleLeft;

                if (selectedSO == so)
                {
                    style.normal.textColor = Color.cyan;
                    style.fontStyle = FontStyle.Bold;
                }

                if (GUILayout.Button($"{so.name}", style))
                {
                    selectedSO = so;
                    serializedObject = new SerializedObject(selectedSO);
                    GUI.FocusControl(null);
                }
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawRightDetail()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            if (selectedSO == null || serializedObject == null)
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("왼쪽 리스트에서 SO를 선택하세요.", EditorStyles.largeLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }
            else
            {
                // 실시간 데이터 동기화
                serializedObject.Update();

                GUILayout.Label($"📝 수정 중: {selectedSO.name}", EditorStyles.boldLabel);
                EditorGUILayout.Space(10);

                rightScrollPos = EditorGUILayout.BeginScrollView(rightScrollPos);

                // --- 포커스 이슈 해결을 위한 변경 감지 로직 ---
                EditorGUI.BeginChangeCheck();

                SerializedProperty iterator = serializedObject.GetIterator();
                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren))
                {
                    if (iterator.name == "m_Script") continue;
                    EditorGUILayout.PropertyField(iterator, true); // 중첩 클래스(dbdata) 자동 지원
                    enterChildren = false;
                }

                if (EditorGUI.EndChangeCheck())
                {
                    // 값이 바뀌는 즉시 메모리 데이터에 반영 (포커스 이동 시 증발 방지)
                    serializedObject.ApplyModifiedProperties();
                }
                // ------------------------------------------

                EditorGUILayout.EndScrollView();

                GUILayout.FlexibleSpace();

                // 최종 파일 저장 버튼 및 알림창
                GUI.color = Color.green;
                if (GUILayout.Button(" 파일로 최종 저장 (Save Assets)", GUILayout.Height(40)))
                {
                    AssetDatabase.SaveAssets();
                    EditorUtility.DisplayDialog("저장 완료",
                        $"{selectedSO.name}의  변경사항이 저장되었습니다.", "확인");
                }
                GUI.color = Color.white;
            }

            EditorGUILayout.EndVertical();
        }

        private void RefreshSOList()
        {
            soList.Clear();
            selectedSO = null;
            serializedObject = null;

            if (!AssetDatabase.IsValidFolder(folderPath)) return;

            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { folderPath });
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (so != null) soList.Add(so);
            }
        }
    }
}