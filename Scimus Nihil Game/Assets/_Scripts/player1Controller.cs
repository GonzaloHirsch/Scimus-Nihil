using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1Controller : MonoBehaviour {

    public float playerSpeed = 10f;
    public float dashSpeed = 10;
    public GameObject bullet;
    //public int initialGunAmmo = 10;
    public int queueGunAmmo = 4;
    public float bulletLife = 2.0f;
    public float bulletSpeed = 2.0f;
    [HideInInspector]
    public bool isAlive = false;
    public int plantCountDeath = 20;
    [HideInInspector]
    public int nearCount = 0;
    public int score = 0;
    public float energy = 30;
    public float energyDecrementConstant = 0.5f;
    public float energyIncrementConstant = 1f;
    public int dashAmount = 5;

    private float energydecrement = ;
    private Vector2 moveDirection;
    private Vector2 bulletDirection;
    private Rigidbody2D playerRB;
    private Queue<GameObject> gunQueue;
    //private int gunAmmo;
    private readonly float EPSILON = 0.000000000001f;
    private float currentSpeed;

    void Start () {
        bulletDirection = Vector2.up;
        playerRB = GetComponent<Rigidbody2D>();
        //gunAmmo = initialGunAmmo;
        currentSpeed = playerSpeed;
        FillGun();
	}
	
	void FixedUpdate () {
        if (isAlive){
            Walk();
            Shoot();
            Dash();
            if (nearCount >= plantCountDeath)
                isAlive = false;
        }
	}

    void Walk(){
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        moveDirection = new Vector2(horizInput, vertInput);

        playerRB.position += moveDirection * Time.deltaTime * currentSpeed;

        if (System.Math.Abs(horizInput) > EPSILON)
            bulletDirection = GetDirection(horizInput);
    }

    void Shoot(){
        if (Input.GetKeyDown(KeyCode.Space) /*&& initialGunAmmo > 0*/){
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

    public void LowerEnergy(){
        energy -= energydecrement;
    }

    public void IncrementEnergyDecrement(){
        energydecrement += energyDecrementConstant;
    }

    public void IncrementEnergy(){
        energy += energyIncrementConstant;
    }

    void ShootBullet(){
        //initialGunAmmo--;
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
        if (Input.GetKeyDown(KeyCode.RightShift)){
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
            currentSpeed *= dashSpeed;
        else
            currentSpeed = playerSpeed;
    }
}