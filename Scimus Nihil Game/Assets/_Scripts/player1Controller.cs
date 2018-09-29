using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1Controller : MonoBehaviour {

    public float playerSpeed = 10f;
    public float dashSpeed = 10;
    public GameObject bullet;
    public int initialGunAmmo = 10;
    public int queueGunAmmo = 4;
    public float bulletLife = 2.0f;
    public float bulletSpeed = 2.0f;


    [HideInInspector]
    public bool isAlive = false;

    private Vector2 moveDirection;
    private Vector2 bulletDirection;
    private Rigidbody2D playerRB;
    private Queue<GameObject> gunQueue;
    private int gunAmmo;
    private readonly float EPSILON = 0.000000000001f;

    void Start () {
        bulletDirection = Vector2.up;
        playerRB = GetComponent<Rigidbody2D>();
        gunAmmo = initialGunAmmo;

        FillGun();
	}
	
	void FixedUpdate () {
        Walk();
        Shoot();
        Dash();
	}

    void Walk(){
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        moveDirection = new Vector2(horizInput, vertInput);

        playerRB.position += moveDirection * Time.deltaTime * playerSpeed;

        if (System.Math.Abs(horizInput) > EPSILON || System.Math.Abs(vertInput) > EPSILON)
            bulletDirection = GetDirection(horizInput);
    }

    void Shoot(){
        if (Input.GetKeyDown(KeyCode.Space) && initialGunAmmo > 0){
            ShootBullet();
        }
    }

    //Si queremos agregar para arriba agregamos el float del axis
    Vector2 GetDirection(float horizAxis){
        Vector2 direction = new Vector2();

        if (horizAxis > 0)
            direction = Vector2.right;
        else if (horizAxis < 0)
            direction = Vector2.left;
        /*
        else if (vertAxis > 0)
            direction = Vector2.up;
        else if (vertAxis < 0)
            direction = Vector2.down;
            */

        return direction;
    }

    void FillGun(){
        gunQueue = new Queue<GameObject>();
        GameObject bulletInstance;
        for (int i = 0; i < queueGunAmmo; i++){
            bulletInstance = Instantiate(bullet);
            bulletInstance.SetActive(false);
            gunQueue.Enqueue(bulletInstance);
        }
    }

    void ShootBullet(){
        initialGunAmmo--;
        GameObject bulletInstance = gunQueue.Dequeue();
        bulletController controller = bulletInstance.GetComponent<bulletController>();

        bulletInstance.SetActive(true);
        controller.canMove = true;
        controller.direction = bulletDirection;
        bulletInstance.transform.position = playerRB.position;
        controller.existanceTime = 0f;
        controller.bulletLife = bulletLife;
    }

    public void StoreBullet(GameObject bulletInstance){
        bulletInstance.SetActive(false);
        gunQueue.Enqueue(bulletInstance);
    }

    void Dash(){
        if (Input.GetKeyDown(KeyCode.RightShift))
            playerRB.velocity *= dashSpeed;
    }
}