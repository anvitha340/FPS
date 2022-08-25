using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Vector3 new_position;
    Quaternion new_rotation;
    PlayerScript playerScript;
    private void Start()
    {
        new_position = this.transform.position;
        new_rotation = this.transform.rotation;
    }
    private void Update()
    {
        gameObject.transform.position = Vector3.Lerp(transform.position,new_position,Time.deltaTime*5f);
        transform.rotation = Quaternion.Slerp(transform.rotation, new_rotation, Time.deltaTime * 5f);
    }
    public void SetTransform(Vector3 pos,Quaternion rotation)
    {
        new_position = pos;
        new_rotation = rotation;
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("collided here");
        
        //float damage = 2f;
        //if (playerScript.curHealth > 0f)
        //{
        //    playerScript.curHealth -= damage;
        //    Debug.Log("Player's current health " + playerScript.curHealth);
            //healthSlider.maxValue = playerScript.maxHealth;
            //healthSlider.value = playerScript.maxHealth;
            //resourceBar.SetBarValue(healthSlider, playerScript.curHealth);
        //}

        //if (playerScript.curHealth <= 0)
        //{
        //    damagePlayer += playerScript.curHealth;


        //}
    //}
}
