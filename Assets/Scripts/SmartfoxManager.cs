using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using UnityEngine.UI;
using Sfs2X.Requests;
using Sfs2X.Entities.Data;
using EventManagerActivity;
using System;

public class SmartfoxManager : MonoBehaviour
{
	[SerializeField] Text email;
	[SerializeField] GameObject LobbyScene, playRequestPopup, GameView, enemyPrefab, ChatRequestPopup;

	GameObject enemygameObject;
	private string opponent_name;

	public SmartFox sfs;
	public string Host = "smartfox.juegogames.com";
	public int TcpPort = 9933;
	public string Zone = "FPS";
	public static List<string> user_names = new List<string>();
	public static SmartfoxManager instance;
    public static string sender = "";
    private Vector3 enemyPosition;
	private Room curr_room = null;
	[SerializeField] string room_name = "Lobby";
	[SerializeField] ResourceBar enemyHealth;

	#region MonoMethods

	private void Awake()
    {
		instance = this;
    }

	private void Start()
	{
		enemyPosition = enemyPrefab.transform.localPosition;
		user_names.Clear();
	}

	private void OnEnable()
    {
		EventManager.OnPlayRequest += SendExtension;
		EventManager.SendMessageToStartGame += StartGameRequest;
		EventManager.sendChangeinPos += SendPositionChangeExt;
		EventManager.OnRequestToChat += EnableChatPopup;
		EventManager.OnChatRequest += SendChatRequestExt;
		EventManager.sendHit += SendHitDetectExt;
	}

    private void SendHitDetectExt()
    {
		ISFSObject isFSObject = new SFSObject();
        if (curr_room != null)
        {
			sfs.Send(new ExtensionRequest("reduceHealth", isFSObject, curr_room));
        }
    }

    private void OnDisable()
	{
		EventManager.OnPlayRequest -= SendExtension;
		EventManager.SendMessageToStartGame -= StartGameRequest;
		EventManager.sendChangeinPos -= SendPositionChangeExt;
		EventManager.OnRequestToChat -= EnableChatPopup;
		EventManager.OnChatRequest -= SendChatRequestExt;
		EventManager.sendHit -= SendHitDetectExt;
	}

    private void Update()
    {
		if (sfs != null)
        {
			sfs.ProcessEvents();

			EventManager.instance.TriggerLoginEventSuccess();

		}
	}

    #endregion

    #region Private Methods

	private void OnConnection(BaseEvent evt)
	{
		if ((bool)evt.Params["success"])
		{
			Debug.Log("Connected");
			sfs.Send(new LoginRequest(email.text));
		}
		else
		{
			Reset();
			Debug.Log("Connection Error Occured");
			Debug.Log("Reason:" + (string)evt.Params["reason"]);
		}
	}

	private void OnConnectionLost(BaseEvent evt)
	{
		Reset();
		Debug.Log("Not Connected");
	}

	private void OnLogin(BaseEvent evt)
	{
		User user = (User)evt.Params["user"];
		LoginManagerScript.smartfoxLoginComplete = true;
		EventManager.instance.TriggerLoginEventSuccess();
		Debug.Log("User " + user.Name + " Logged in");

		ISFSObject isfsObject = new SFSObject();
		sfs.Send(new ExtensionRequest("getUserList", isfsObject));
	}

	private void OnLoginError(BaseEvent evt)
	{
		Reset();
		Debug.Log("Login Error Occured");
		Debug.Log("Error Message" + (string)evt.Params["errorMessage"]);
	}

	private void OnRoomCreate(BaseEvent evt)
	{
		Debug.Log("Room created");
		sfs.Send(new JoinRoomRequest(sfs.GetRoomByName("Lobby")));

		Debug.Log("Staring game with" + opponent_name);

		ISFSObject isfsObject = new SFSObject();
		isfsObject.PutUtfString("player1", opponent_name);
		sfs.Send(new ExtensionRequest("startGame", isfsObject));
	}

	private void OnRoomCreateError(BaseEvent evt)
	{
		Debug.Log("couldnt create room");
		Debug.Log("error" + (string)evt.Params["errorMessage"]);
	}

	private void OnRoomJoin(BaseEvent evt)
	{
		curr_room = (Room)evt.Params["room"];
		Debug.Log("Room Joined" + curr_room);
		SwitchToGame();
	}

	private void OnRoomJoinError(BaseEvent evt)
	{
		Debug.Log("error" + (string)evt.Params["errorMessage"]);
	}

	private void OnUserExitRoom(BaseEvent evt)
	{
		Destroy(enemygameObject);
	}

	private void OnUserEnterRoom(BaseEvent evt)
	{
		Debug.Log("user enetered room");
		//spawn user
		enemygameObject = Instantiate(enemyPrefab);
		enemygameObject.transform.SetParent(GameView.transform);
		enemygameObject.transform.localPosition = new Vector3(37, 1, -80);
	}

    private void OnExtensionResponse(BaseEvent evt)
    {
		string cmd = (string)evt.Params["cmd"];
		ISFSObject obj = (SFSObject)evt.Params["params"];

		if (cmd == "getUserList")
        {
			user_names.Clear();
			for(int i = 1; i < 5; i++)
            {
				string key = "name" + i.ToString();
				string val = obj.GetUtfString(key);

                if (obj.ContainsKey(key) && !user_names.Contains(val) )
					user_names.Add(val);
            }
        }
		else if( cmd == "chatRequest")
        {
			string display_message;
			opponent_name = obj.GetUtfString("player2");
			display_message = "Chat with " + opponent_name + "?";

			Debug.Log(display_message);

			GameObject popup = Instantiate(playRequestPopup);
			popup.GetComponentInChildren<Text>().text = display_message;

			popup.transform.parent = LobbyScene.transform;
			popup.transform.localPosition = Vector3.zero;
		}
		else if (cmd == "playRequest")
		{
			string display_message;
			opponent_name = obj.GetUtfString("player2");
			display_message = "Play with " + opponent_name + "?";

			Debug.Log(display_message);

			GameObject popup = Instantiate(playRequestPopup);
			popup.GetComponentInChildren<Text>().text = display_message;

			popup.transform.parent = LobbyScene.transform;
			popup.transform.localPosition = Vector3.zero;

		}
		else if(cmd == "startGame")
        {
			if(sfs.LastJoinedRoom != curr_room)
            {
				Debug.Log("enter room and  switch to game scene, play with" + obj.GetUtfString("player2"));
				sfs.Send(new JoinRoomRequest(room_name));
            }
        }
		else if (cmd == "changeInPos")
		{
			Vector3 new_pos = new Vector3(37,1,-80);
			new_pos.x = obj.GetFloat("x");
			new_pos.y = obj.GetFloat("y");
			new_pos.z = obj.GetFloat("z");

			Vector3 new_rot = new Vector3(0, 0, 0);
			new_rot.y = obj.GetFloat("y_rot");

			Quaternion rotation = Quaternion.Euler(new_rot);
            
            if (enemygameObject)
				enemygameObject.GetComponent<EnemyScript>().SetTransform(new_pos,rotation);
            
		}
		else if(cmd == "reduceHealth")
        {
			if(obj.GetUtfString("sender") != sfs.MySelf.Name)
            {
				EventManager.instance.TriggerSetEnemyHealth(false);

            }
            else
            {
				EventManager.instance.TriggerSetEnemyHealth(true);
			}
		}
	}
    

	private void EnableChatPopup(string sender)
    {
		//Instantiate popup to accept chat req with accpt button => sender = obj.GetUtfString("sender") ;
		GameObject chatReqPopup = Instantiate(ChatRequestPopup);
	}

	private void SendPositionChangeExt(Vector3 newPos, Quaternion newRotation)
    {
        if (enemygameObject)
        {
			ISFSObject isfsObject = new SFSObject();
			isfsObject.PutFloat("x", newPos.x);
			isfsObject.PutFloat("y", newPos.y);
			isfsObject.PutFloat("z", newPos.z);

			isfsObject.PutFloat("y_rot", newRotation.eulerAngles.y);
			isfsObject.PutUtfString("recipient", opponent_name);

			sfs.Send(new ExtensionRequest("changeInPos", isfsObject,curr_room));
        }
	}

    private void SwitchToGame()
    {
		GameView.SetActive(true);
		LobbyScene.SetActive(false);

		if (curr_room == null)
        {
			return;
        }
		foreach (var user in curr_room.UserList)
		{
			Debug.Log("name" + user.Name);
            if (user.Name != sfs.MySelf.Name)
            {
				enemygameObject = Instantiate(enemyPrefab);
				enemygameObject.transform.SetParent(GameView.transform);
				enemygameObject.transform.localPosition = new Vector3(37, 1, -80);
			}
		}
	}

	private void SendExtension(Text opponent_text)
	{
		Debug.Log("sending extension");
		opponent_name = opponent_text.text;

		ISFSObject isfsObject = new SFSObject();
		isfsObject.PutUtfString("recipient", opponent_name);
		sfs.Send(new ExtensionRequest("playRequest", isfsObject));
	}

	private void StartGameRequest()
	{
		RoomSettings roomSettings = new RoomSettings(room_name);
		roomSettings.Extension = new RoomExtension("FPSGame", "GamePlay.FPSGameExtension");
		Debug.Log("creating room");
		sfs.Send(new CreateRoomRequest(roomSettings));
		SwitchToGame();
	}

	private void SendChatRequestExt(Text opponent_text)
	{
		Debug.Log("sending chat request extension");
		opponent_name = opponent_text.text;

		ISFSObject isfsObject = new SFSObject();
		isfsObject.PutUtfString("recipient", opponent_name);
		sfs.Send(new ExtensionRequest("chatRequest", isfsObject));
	}
	#endregion

	#region Public methods

	public void OnLoginButton()
	{
		ConfigData cfg = new ConfigData();
		cfg.Host = Host;
		cfg.Port = TcpPort;
		cfg.Zone = Zone;
		sfs = new SmartFox();

		sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
		sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
		sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
		sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
		sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
		sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
		sfs.AddEventListener(SFSEvent.ROOM_ADD, OnRoomCreate);
		sfs.AddEventListener(SFSEvent.ROOM_CREATION_ERROR, OnRoomCreateError);
		sfs.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
		sfs.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
		sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);

		sfs.Connect(cfg);
	}
	public void Reset()
	{
		if (sfs != null)
		{
			sfs.RemoveEventListener(SFSEvent.CONNECTION, OnConnection);
			sfs.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
			sfs.RemoveEventListener(SFSEvent.LOGIN, OnLogin);
			sfs.RemoveEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
			sfs.RemoveEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
			sfs.RemoveEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
			sfs.RemoveEventListener(SFSEvent.ROOM_ADD, OnRoomCreate);
			sfs.RemoveEventListener(SFSEvent.ROOM_CREATION_ERROR, OnRoomCreateError);

			sfs.RemoveEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
			sfs.RemoveEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
			sfs.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);

			sfs = null;

		}
	}

	public static List<string> GetUserNames()
    {
		return user_names;
    }
	public void Logout()
    {
		sfs.Disconnect();
		Reset();
    }
    #endregion
}
