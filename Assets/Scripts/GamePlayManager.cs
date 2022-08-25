using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerActivity;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GamePlayManager : MonoBehaviour
{
    public static string player1 = "not updated";
    [SerializeField] GameObject player_prefab, player, GameView, EndScreen;
    [SerializeField] Text winMessage; 
    public ResourceBar myHealth,enemyHealth;

    public void StartGamePlay()
    {
        EventManager.instance.TriggerStartGameRequest();
    }
    private void OnEnable()
    {
        EventManager.OnGameOver += OnGameOver;
    }
    private void OnDisable()
    {
        EventManager.OnGameOver -= OnGameOver;
        Destroy(player);
    }
    private void OnGameOver(bool win)
    {
        GameView.SetActive(false);
        EndScreen.SetActive(true);
        Destroy(player);
        if (win)
        {
            winMessage.text = "YOU WoN";
        }
        else
        {
            winMessage.text = "ENEMY WoN";
        }
    }

    private void Start()
    {
        if(GameView != null)
        {
            var position = new Vector3(Random.Range(0, 30), 1, Random.Range(-70, -60));
            player = Instantiate(player_prefab);

            //player = Instantiate(player_prefab);
            Bullet.player = player;
            player.transform.SetParent(GameView.transform);
            player.transform.localPosition = position;
            
            myHealth.GetComponent<Slider>().value = 100;
            enemyHealth.GetComponent<Slider>().value = 100;
        }
    }

}
