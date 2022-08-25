using System.Collections;
using System.Collections.Generic;
using EventManagerActivity;
using Firebase.Auth;
using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;
using UnityEngine.UI;

public class UserRequestController : MonoBehaviour
{
    [SerializeField] GameObject popupWindow, opponentUserName, chatBtn;
   
    [SerializeField] Text opponent_username;

    public void EnablePopup()
    {
        popupWindow.SetActive(true);
    }
    public void EnableChat(Text user_item)
    {
        GamePlayManager.player1 = user_item.text;
        Debug.Log("chat with" + user_item.text);
        EventManager.instance.TriggerChatRequest(user_item);
    }
    public void EnableGameRequest(Text user_item)
    {
        GamePlayManager.player1 = user_item.text;
        Debug.Log("play with" + user_item.text);
        EventManager.instance.TriggerPlayRequest(user_item);

    }
}
