using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Firebase.Auth;
public class AuthService : MonoBehaviour
{
    public static AuthService instance;
    private const string FirebaseProviderName = "oidc-firebase";
    private async void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
        await InitializeAsync();
    }
    public async Task InitializeAsync()
    {
        try
        {
            await UnityServices.InitializeAsync();
            ///---기존 내용---------------------------------------------
            //if (!AuthenticationService.Instance.IsSignedIn)
            //{
            //    await AuthenticationService.Instance.SignInAnonymouslyAsync();
            //    Debug.Log($"[Auth] 로그인 완료: {AuthenticationService.Instance.PlayerId}");
            //}

            ///------------------------------------------------
            FirebaseUser user = BackendManager.Auth.CurrentUser;
            string idToken = await user.TokenAsync(false);
            await AuthenticationService.Instance.SignInWithOpenIdConnectAsync(
                FirebaseProviderName, idToken);
            Debug.Log($"[Auth] 로그인 완료:{user.UserId} / USG Player ID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Auth] 초기화 실패: {e.Message}");
            throw;
        }
    }
}