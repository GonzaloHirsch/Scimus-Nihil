using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour {

    [HideInInspector]
    public float bulletLife = 2.0f;
    public bool canMove = false;
    public float bulletSpeed = 2.0f;
    [HideInInspector]
    public float existanceTime;
    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public Rigidbody2D bulletBody;
    [HideInInspector]
    public Transform bulletTransform;

    private player1Controller playerController;


    void Start (){
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<player1Controller>();
        bulletBody = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate (){
        if (canMove){
            existanceTime += Time.deltaTime;
            BulletMovement();
            if (existanceTime >= bulletLife){
                canMove = false;
                playerController.StoreBullet(this.gameObject);
            }
        }
	}

    void BulletMovement(){
        bulletBody.position += direction * Time.deltaTime * bulletSpeed; 
    }
}
