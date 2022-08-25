using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private ResourceBar resourceBar;
    private Slider healthSlider;
    private PlayerScript playerScript;
    private GamePlayManager gamePlayManager;
    public float damagePlayer, damageEnemy;
    bool win = false;

    void Start()
    {
        gamePlayManager = gameObject.GetComponentInParent<GamePlayManager>();
        playerScript = gameObject.GetComponent<PlayerScript>();
        
    }
    private void OnEnable()
    {
        EventManagerActivity.EventManager.setEnemyHealth += DamagePlayer;
    }

    private void OnDisable()
    {
        EventManagerActivity.EventManager.setEnemyHealth -= DamagePlayer;
    }
    public void DamagePlayer( bool isMe)
    {
        if(isMe == false)
        {
            resourceBar = gamePlayManager.myHealth;
            Debug.Log("if",resourceBar);
            win = false;
        }
        else
        {
            resourceBar = gamePlayManager.enemyHealth;
            win = true;
        }
        healthSlider = resourceBar.GetComponent<Slider>();
        float healthBarVal = healthSlider.value;

        if (healthBarVal > 0f)
        {
            resourceBar.SetBarValue(healthSlider,healthBarVal - 10f);
        }
        else
        {
            EventManagerActivity.EventManager.instance.TriggerOnGameOver(win);
        }

    }
}
