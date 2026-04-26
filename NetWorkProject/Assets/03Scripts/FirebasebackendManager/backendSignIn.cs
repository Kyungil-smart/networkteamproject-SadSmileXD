using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
public class backendSignIn  
{
    public backendSendEmail m_sendEmail;
    public void SignIn(string email, string password)
    {
        BackendManager.Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            if (!BackendManager.Auth.CurrentUser.IsEmailVerified)
            {
                Debug.Log("이메일 인증 안됨");
                SubscribeManager.instance.Publish(SubscribeType.EmailVerificationRequired);
                m_sendEmail.SendEmail();
                BackendManager.Auth.SignOut();

            }
            else
            {
                Debug.Log("이메일 인증됨");
            }
            AuthResult result = task.Result;
            Debug.Log($"User signed in successfully: {result.User.DisplayName} ({result.User.UserId})");

           

        });
    }
}
