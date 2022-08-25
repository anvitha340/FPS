using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventManagerActivity;
using UnityEngine.UI;
using Firebase.Auth;

public class LobbyScript : MonoBehaviour
{
    [SerializeField] GameObject user_content, requests_content, user_prefab;
    int user_count = 0;
    FirebaseAuth auth;
    List<string> list_of_users = new List<string>();

    #region MonoMethods

    private void OnEnable()
    {
        EventManager.OnLoginSuccess += PopulateUserList;
    }

    private void OnDisable()
    {
        EventManager.OnLoginSuccess -= PopulateUserList;
    }

    #endregion

    #region Public methods

    public void PopulateUserList()
    {
        if(user_count < SmartfoxManager.GetUserNames().Count )
        {
            Debug.Log("Populating user list");
            foreach (var user in SmartfoxManager.GetUserNames())
            {
                if (!list_of_users.Contains(user) )
                {
                    GameObject user_item = Instantiate(user_prefab);
                    user_item.GetComponent<Text>().text = user;
                    user_item.transform.parent = user_content.transform;
                    list_of_users.Add(user);
                }
            }
            user_count = SmartfoxManager.GetUserNames().Count;
        }

    }

    public void OnSend(Text message)
    {
        if(!string.IsNullOrEmpty( SmartfoxManager.sender))
        {
            Debug.Log("send" + message.text + "to" +SmartfoxManager.sender);
        }
    }

    #endregion

}
