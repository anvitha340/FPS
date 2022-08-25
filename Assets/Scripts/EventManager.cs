using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EventManagerActivity
{
    public class EventManager : MonoBehaviour
    {

        public delegate void Events();
        public delegate void RequestHandlerEvent(string s);
        public static event Events OnLoginSuccess , sendHit ;
        public static event RequestHandlerEvent OnRequestToChat, SendMessageToPlay, OnRequestToPlay;
        public static EventManager instance;

        public delegate void PlayRequestHandler(Text opponent_name);
        public static event PlayRequestHandler OnPlayRequest, OnChatRequest;
        public delegate void StartGameRequest();
        public static event StartGameRequest SendMessageToStartGame;
        public delegate void SendChangeInPos(Vector3 newPos, Quaternion newRotation);
        public static event SendChangeInPos sendChangeinPos;

        public delegate void HealthChangeHandler(bool isMe);
        public static event HealthChangeHandler setEnemyHealth;

        public delegate void GameOver(bool win);
        public static event GameOver OnGameOver;
        private void Awake()
        {
            instance = this;
        }

        public void TriggerLoginEventSuccess()
        {
            OnLoginSuccess?.Invoke();
        }
        public void RequestToChat(string s)
        {
            OnRequestToChat?.Invoke(s);
        }
        public void TriggerPlayRequest(Text opponent_name)
        {
            OnPlayRequest?.Invoke(opponent_name);
        }
        public void TriggerChatRequest(Text opponent_name)
        {
            OnChatRequest?.Invoke(opponent_name);
        }
        public void TriggerStartGameRequest()
        {
            SendMessageToStartGame?.Invoke();
        }
        public void TriggerChangeInPos(Vector3 newPos,Quaternion newRotation)
        {
            sendChangeinPos?.Invoke(newPos, newRotation);
        }
        public void TriggerHit()
        {
            sendHit?.Invoke();
        }
        public void TriggerSetEnemyHealth(bool isMe)
        {
            setEnemyHealth?.Invoke(isMe);
        }
        public void TriggerOnGameOver(bool win)
        {
            OnGameOver?.Invoke(win);
        }

    }
}

