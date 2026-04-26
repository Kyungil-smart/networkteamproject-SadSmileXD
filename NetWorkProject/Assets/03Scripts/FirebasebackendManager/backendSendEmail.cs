using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

public class backendSendEmail
{
    
    public void SendEmail()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user == null)
            return;

        user.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendEmailVerificationAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("Email sent successfully.");
        });
    }
}
