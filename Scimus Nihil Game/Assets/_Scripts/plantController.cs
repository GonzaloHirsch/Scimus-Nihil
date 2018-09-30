using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantController : MonoBehaviour {

    public Animation[] walkingSprites;
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
    private Animator plantAnimator;
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
        plantAnimator = GetComponent<Animator>();
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
        {
            isAlive = false;
            Destroy(GetComponent<SpriteRenderer>());
            GameObject child = new GameObject();
            child.transform.parent = transform;
            child.transform.localPosition = new Vector3(0, 0, 0);
            child.transform.localScale *= 2.5f;
            child.AddComponent<SpriteRenderer>().sprite = walkingCivil[Random.Range(0, walkingCivil.Length)];
            child.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = deadCivil[Random.Range(0, walkingCivil.Length)];
            transform.localScale *= 0.75f;
            GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
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
        if (spriteNumber == 1){
            plantAnimator.SetBool("Plant2", true);
        }
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
        //SetSprites(spriteIndex);
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
            plantAnimator.enabled = false;
            isAlive = false;
            player.GetComponent<player1Controller>().score++;
            player.GetComponent<player1Controller>().IncrementEnergy();
            plantRenderer.sprite = deadSprites[spriteIndex];
        }
    }
}
