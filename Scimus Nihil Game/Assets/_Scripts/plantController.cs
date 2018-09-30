﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantController : MonoBehaviour {

    public Sprite[] walkingSprites;
    public Sprite[] deadSprites;
    public Sprite[] deadCivil;
    public Sprite[] walkingCivil;
    [HideInInspector]
    public int spriteIndex;
    [Range(0, 100)]
    public int probabilityOfFollowing = 70;
    public float plantFollowSpeed = 4f;
    public float plantRoamSpeed = 2f;
    public float directionChangeTime = 3f;
    public float minimunAngleDifference = 45f;
    [HideInInspector]
    public bool isAlive = true;
    public float minimumPlayerDistance = 3f;
    public float explosionForce = 10f;

    private SpriteRenderer plantRenderer;
    private int spritesSize;
    private bool follows;
    private GameObject player;
    private float directionRealTime = 0f;
    private Vector2 versorDirection;
    private float previousAngle = 0;
    private float distance;
    private bool isInRange = false;



	void Start () {
        spritesSize = walkingSprites.Length;
        plantRenderer = GetComponent<SpriteRenderer>();
        spriteIndex = (int)Random.Range(0, (float)spritesSize);
        SetSprites(spriteIndex);

        setState();

        player = GameObject.FindGameObjectWithTag("Player");

        gameController.OnPlayerDeath += onPlayerDeath;
	}
	
	void FixedUpdate () {
        if (isAlive)
        {
            if (follows)
            {
                Follow();
            }
            else
            {
                Roam();
            }
            UpdateDistanceStatus();

            if (distance <= minimumPlayerDistance && !isInRange)
            {
                player.GetComponent<player1Controller>().nearCount++;
                isInRange = true;
            }
            else if (distance > minimumPlayerDistance && isInRange)
            {
                player.GetComponent<player1Controller>().nearCount--;
                isInRange = false;
            }
            /*
            if (Input.GetKeyDown(KeyCode.R)){
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                Vector2 direction = player.transform.position - transform.position;
                rb.AddForce(new Vector2(-direction.x, -direction.y) * explosionForce, ForceMode2D.Force);
            }*/
                
            if (isInRange && player.GetComponent<player1Controller>().isDashing){
                Explode();
            }
        }
        else if (!isAlive)
            DeactivateColliderAndRb();

        if (isInRange && !isAlive){
            player.GetComponent<player1Controller>().nearCount--;
            isInRange = false;
        }
	}

    public void onPlayerDeath()
    {
        if (isAlive)
            GetComponent<SpriteRenderer>().sprite = walkingCivil[Random.Range(0, walkingCivil.Length - 1)];
        else
            GetComponent<SpriteRenderer>().sprite = deadCivil[Random.Range(0, deadCivil.Length - 1)];
    }

    void DeactivateColliderAndRb(){
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Rigidbody2D>());
    }

    void Explode(){
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 direction = player.transform.position - transform.position;
        rb.AddForce(new Vector2(-direction.x, -direction.y).normalized * explosionForce, ForceMode2D.Force);
    }

    void UpdateDistanceStatus(){
        distance = Vector2.Distance(player.transform.position, transform.position);
    }

    void SetSprites(int spriteNumber){
        plantRenderer.sprite = walkingSprites[spriteNumber];
    }

    void setState(){
        int state = (int)Random.Range(0f, 100f);
        if (probabilityOfFollowing >= state)
            follows = true;
        else
            ChangeDirection();
    }

    void Follow(){
        Vector3 vectorDifference = player.transform.position - transform.position;
        transform.position += (vectorDifference / vectorDifference.magnitude) * Time.deltaTime * plantFollowSpeed;
    }

    void Roam(){
        transform.position += (Vector3)versorDirection * Time.deltaTime * plantRoamSpeed;
        directionRealTime += Time.deltaTime;
        if (directionRealTime >= directionChangeTime){
            directionRealTime = 0f;
            ChangeDirection();
        }
    }

    void ChangeDirection(){
        float angle = GetNewAngle();
        previousAngle = angle;
        versorDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        versorDirection = versorDirection / versorDirection.magnitude;
        SetSprites(spriteIndex);
    }

    float GetNewAngle(){
        float angle;
        do {
            angle = Random.Range(0f, 360f);
        } while (minimunAngleDifference > Mathf.Abs(angle - previousAngle));
        return angle;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Bullet")){
            isAlive = false;
            player.GetComponent<player1Controller>().score++;
            player.GetComponent<player1Controller>().IncrementEnergy();
            plantRenderer.sprite = deadSprites[spriteIndex];
        }
    }
}
