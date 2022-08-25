using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	private readonly float speed = 8.0f;
	private readonly float jumpSpeed = 8.0f;
	private readonly float gravity = 20.0f;
    internal float maxHealth = 100;
    internal float curHealth;
    private Vector3 moveDirection = Vector3.zero;
	private bool grounded = false;

	private CharacterController controller;
	Vector3 changeInPositon;
    Quaternion changeInRotation;


    void Awake()
	{
		controller = GetComponent<CharacterController>();
	}

    private void Start()
    {
        changeInPositon = transform.position;
        changeInRotation = transform.rotation;
        curHealth = maxHealth;
    }
    private void Update()
    {
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        transform.Rotate(0, Input.GetAxis("Mouse X") * 8f, 0);
        if (Input.GetButton("Jump"))
		{
			moveDirection.y = jumpSpeed;
		}

        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);


        changeInPositon = this.transform.position;
        changeInRotation = this.transform.rotation;
        EventManagerActivity.EventManager.instance.TriggerChangeInPos(changeInPositon,changeInRotation);
        

    }

    
}
