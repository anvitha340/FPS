using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using EventManagerActivity;

public class LoginManagerScript : MonoBehaviour
{
    [SerializeField] Text result;
    [SerializeField] Text email, password;
    [SerializeField] GameObject Login, LoginBtn, SignUpBtn, Lobby;
    FirebaseAuth auth;
    string login_failure_text;
    public delegate void LoginComplete();
    public LoginComplete LoginCompleteCallbck;
    enum LoginStatus
    {
        PROCESSING,
        COMPLETE,
        ERROR
    };
    LoginStatus loginStatus;
    public static bool smartfoxLoginComplete = false;
    
    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }
    public void DisableScreen()
    {

        Lobby.SetActive(false);
        Login.SetActive(false);
    }
    private void OnEnable()
    {
        EventManager.OnLoginSuccess += SignInComplete;
    }
    private void OnDisable()
    {
        EventManager.OnLoginSuccess -= SignInComplete;
    }
    public void SignUp()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            SignUpTask(task);
        });
    }

    private void SignUpTask(Task<FirebaseUser> task)
    {
        try
        {
            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.Email, newUser.UserId);
        }
        catch (System.AggregateException e)
        {
            Debug.Log("Failure"+e);
        }

    }
    public void SignInBtnCall()
    {
        loginStatus = LoginStatus.PROCESSING;
        SignIn();
        StartCoroutine(CheckLogin());
    }

    IEnumerator CheckLogin()
    {
        while(loginStatus == LoginStatus.PROCESSING)
        {
            yield return null;
        }
        if(loginStatus == LoginStatus.COMPLETE)
        {
            //SignInComplete();
            EventManager.instance.TriggerLoginEventSuccess();
        }
        if(loginStatus == LoginStatus.ERROR)
        {
            result.text = login_failure_text;
        }
    }
    
    public void SignIn()
    {
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            try
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                        newUser.DisplayName, newUser.UserId);
                Debug.Log("Success");
                loginStatus = LoginStatus.COMPLETE;
            }
            catch(AggregateException e)
            {
                LoginFailed(e);
            }
            
        });

    }
    public void LoginFailed(AggregateException e)
    {
        e.Handle((x) =>
        {
            if (x is AggregateException)
            {
                (x as AggregateException).Handle((y) =>
                {
                    Debug.Log("failed a");

                    if (y is Firebase.FirebaseException)
                    {
                        return HandleFirebaseException(y as Firebase.FirebaseException);
                    }
                    return false;
                });
            }
            return false;
        });
    }
    private bool HandleFirebaseException(Firebase.FirebaseException e)
    {
        Debug.Log("failed");
        Debug.Log("here"+e);
        login_failure_text = e.Message;
        loginStatus = LoginStatus.ERROR;
        //result.text = e.Message.ToString();
        return true;
    }

    public void SignInComplete()
    {
        if(loginStatus == LoginStatus.COMPLETE && smartfoxLoginComplete == true)
        {
            Lobby.SetActive(true);
            Login.SetActive(false);
            EventManager.OnLoginSuccess -= SignInComplete;
        }
       
    }

    public void SignOut()
    {
        auth.SignOut();
        OnLogOut();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnLogOut()
    {
        Debug.Log("Logged Out");
        Login.SetActive(true);
        Lobby.SetActive(false);
    }

    public void EnableSignUpScreen()
    {
        LoginBtn.SetActive(false);
        SignUpBtn.SetActive(true);
    }

}
