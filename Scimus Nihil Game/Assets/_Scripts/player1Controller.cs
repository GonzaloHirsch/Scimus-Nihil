using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1Controller : MonoBehaviour {

    public float playerSpeed = 10f;
    public GameObject bullet;

    private Vector2 moveDirection;
    private Rigidbody2D playerRB;

	void Start () {
        playerRB = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
        Walk();
	}

    void Walk(){
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        playerRB.position += moveDirection * Time.deltaTime * playerSpeed;
    }
}
