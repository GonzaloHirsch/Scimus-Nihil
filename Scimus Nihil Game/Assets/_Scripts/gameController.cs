using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour {

    public static gameController instance = null;
    [HideInInspector]
    public float plantSpawnTime;
    public GameObject[] spawners;

    private float totalTime = 0f;
    private player1Controller player1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        player1 = GameObject.FindWithTag("Player").GetComponent<player1Controller>();
    }

    void Start () {
        //MainMenu();
        player1.isAlive = true;
    }
	
	void Update () {
        PlayGame();
        if (!player1.isAlive){
            PlayerDeath();
            Credits();
        }
	}

    void MainMenu(){
        //TODO activar la imagen del menu
        while(!Input.GetKeyDown(KeyCode.KeypadEnter)){}
        //TODO desactivar la imagen del menu
        player1.isAlive = true;
        player1.playerAnimator.SetTrigger("PlayerWakeUp");
    }

    void Credits(){
        player1.playerAnimator.SetTrigger("PlayerRemainDead");
        //TODO mostrar creditos
    }

    void PlayerDeath(){
        player1.playerAnimator.SetTrigger("PlayerDie");
    }

    void PlayGame(){
        player1.playerAnimator.SetTrigger("PlayerRun");
        totalTime += Time.deltaTime;
        for (int i = 0; i < spawners.Length; i++)
            spawners[i].GetComponent<spawnerController>().waitTimeTotal = SpawnFunction(totalTime);
    }

    int SpawnFunction(float time){
        return (int)(40 / (time + 5));
    }
}
