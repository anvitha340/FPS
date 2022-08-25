using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static GameObject player;
    PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collided");
        //playerHealth.DamagePlayer(2f, false);
        EventManagerActivity.EventManager.instance.TriggerHit();
    }
}
