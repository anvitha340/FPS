using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    float bulletSpeed = 50f;
    [SerializeField] GameObject bullet;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Fire();
        }
    }
    private void Fire()
    {
        GameObject tempBullet = Instantiate(bullet, gameObject.transform.position, Quaternion.identity) as GameObject;
        tempBullet.transform.SetParent(gameObject.transform);
        Rigidbody tempRigidBodyBullet = tempBullet.GetComponent<Rigidbody>();
        //tempRigidBodyBullet.AddForce(transform.forward * bulletSpeed);
        //tempBullet.transform.TransformDirection(Vector3.forward);
        Destroy(tempBullet, 5f);
        tempRigidBodyBullet.velocity = transform.right * 5;
        //GameObject Bullet = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        //Bullet.GetComponent<Rigidbody>().AddForce(transform.forward * throwDistance);
    }

}
