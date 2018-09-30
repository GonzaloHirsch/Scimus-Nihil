using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

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
    public float energy = 30f;
    public float maxEnergy = 60f;
    public float energyDecrementConstant = 0.5f;
    public float energyIncrementConstant = 1f;
    public float dashEnergyLoss = 15f;
    [HideInInspector]
    public bool isDashing = false;
    [HideInInspector]
    public Rigidbody2D playerRB;
    [HideInInspector]
    public Animator playerAnimator;
    public GameObject clouds;
    public GameObject whiteScreen;
    public GameObject blackScreen;
    public ParticleSystem[] splashShot;
    public GameObject barritaAzul;
    public TextMeshProUGUI scoreText;

    private float energydecrement = 0.1f;
    private Vector2 moveDirection;
    private Vector2 bulletDirection;
    private Queue<GameObject> gunQueue;
    //private int gunAmmo;
    private readonly float EPSILON = 0.000000000001f;
    private float currentSpeed;


    void Start() {
        bulletDirection = Vector2.up;
        playerRB = GetComponent<Rigidbody2D>();
        //gunAmmo = initialGunAmmo;
        currentSpeed = playerSpeed;
        FillGun();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isAlive)
        {
            Dash();
            
        }
    }

    void FixedUpdate() {
        if (isAlive) {
            Walk();
            Shoot();
            if (nearCount >= plantCountDeath)
            {
                blackScreen.SetActive(false);
                Lightning();
                isAlive = false;
                GetComponent<Collider2D>().enabled = false;
                clouds.SetActive(false);
            }
            UpdateUI();
            getTile();
        }
    }

    void Walk() {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        moveDirection = new Vector2(horizInput, vertInput);

        playerRB.position += moveDirection * Time.deltaTime * currentSpeed;

        if (System.Math.Abs(horizInput) > EPSILON)
            bulletDirection = GetDirection(horizInput);
    }

    void Shoot() {
        if (Input.GetKeyDown(KeyCode.Space) /*&& initialGunAmmo > 0*/) {
            ShootBullet();
        }
    }

    //Si queremos agregar para arriba agregamos el float del axis
    Vector2 GetDirection(float horizAxis) {
        Vector2 direction = new Vector2();

        if (horizAxis > 0) {
            direction = Vector2.right;
            transform.localScale = new Vector3(4, 4, 4);
        }

        else if (horizAxis < 0) {
            direction = Vector2.left;
            transform.localScale = new Vector3(-4, 4, 4);
        }

        /*
        else if (vertAxis > 0)
            direction = Vector2.up;
        else if (vertAxis < 0)
            direction = Vector2.down;
            */

        return direction;
    }

    void FillGun() {
        gunQueue = new Queue<GameObject>();
        GameObject bulletInstance;
        for (int i = 0; i < queueGunAmmo; i++) {
            bulletInstance = Instantiate(bullet);
            bulletInstance.SetActive(false);
            gunQueue.Enqueue(bulletInstance);
        }
    }

    public void LowerEnergy() {
        energy -= energydecrement;
    }

    public void IncrementEnergyDecrement() {
        energydecrement += energyDecrementConstant;
    }

    public void IncrementEnergy() {
        if ((energy + energyIncrementConstant) >= maxEnergy)
            energy = maxEnergy;
        else
            energy += energyIncrementConstant;
    }

    void ShootBullet() {
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

    public void StoreBullet(GameObject bulletInstance) {
        bulletInstance.SetActive(false);
        gunQueue.Enqueue(bulletInstance);
    }

    void Dash() {
        if (dashEnergyLoss <= energy && Input.GetKeyDown(KeyCode.RightShift)) {
            energy -= dashEnergyLoss;
            isDashing = true;
        } else if (dashEnergyLoss > energy && Input.GetKeyDown(KeyCode.RightShift)) {
            isDashing = false;
            barritaAzul.GetComponent<RectTransform>().DOShakePosition(0.5f, 15f, 25,90,false,false);
            print("ASDASD");
        } else
        {
            isDashing = false;
        }
    }

    public Tilemap tilemap;

    void getTile()
    {
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        var tile = tilemap.GetTile(cellPosition);
        
        if (tile != null && tile.name == "agua")
        {
            currentSpeed = playerSpeed / 3;
        }
        else
            currentSpeed = playerSpeed;
    }

    void Lightning() {
        SpriteRenderer sr = whiteScreen.GetComponent<SpriteRenderer>();
        sr.DOFade(1, 0.2f).OnComplete(
            () =>
            {
                sr.DOFade(0, 0.2f).OnComplete(
            () =>
            {
                sr.DOFade(1, 0.2f).OnComplete(
                    () => sr.DOFade(0, 0.2f));
            });
                });
    }

    void UpdateUI()
    {
        Rect rect = barritaAzul.GetComponent<RawImage>().uvRect;
        rect.x = Mathf.Lerp(1, 0, energy / maxEnergy);
        barritaAzul.GetComponent<RawImage>().uvRect = rect;

        scoreText.SetText("Score: " + score);
    }
}