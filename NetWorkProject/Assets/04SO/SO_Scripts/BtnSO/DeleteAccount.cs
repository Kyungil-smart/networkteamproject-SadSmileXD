using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DeleteAccount", menuName = "Scriptable Objects/Button/DeleteAccount")]
public class DeleteAccount : ButtonBase
{
    private Button btn;

    public override void ButtonInit(MonoBehaviour OWner, Object[] Objs)
    {
         foreach (Object obj in Objs)
        {
            if(obj is GameObject go && go.TryGetComponent<Button>(out btn))
            {
                btn.onClick.AddListener(AddButtonListener);
            }
        }
    }
    public override void AddButtonListener()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user == null)
            return;

        user.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("DeleteAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("User deleted successfully.");
        });
    }
}
