﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour {

    public static gameController instance = null;
    [HideInInspector]
    public float plantSpawnTime;
    public GameObject[] spawners;
    public GameObject clouds;

    private float totalTime = 0f;
    private player1Controller player1;
    private player2Controller player2;
    private GameObject screenRenderer;
    private bool playingGame = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        player1 = GameObject.FindWithTag("Player").GetComponent<player1Controller>();
        player2 = GameObject.FindWithTag("Player").GetComponent<player2Controller>();
        screenRenderer = GameObject.FindWithTag("ProximityText");
    }

    void Start () {
        ActivateMainMenu();
    }
	
	void Update () {
        DeactivateMainMenu();
        if (playingGame){
            PlayGame();
            if (!player1.isAlive){
                playingGame = false;
                PlayerDeath();
            }
        } else if (!playingGame){
            Credits();
        }
	}

    void ActivateMainMenu(){
        screenRenderer.SetActive(false);
        player2.enabled = false;
        clouds.SetActive(false);
        //TODO activar la imagen del menu
    }

    void DeactivateMainMenu(){
        if(Input.GetKeyDown(KeyCode.Return) && !playingGame){
            clouds.SetActive(false);
            screenRenderer.SetActive(true);
            player2.enabled = true;
            player1.isAlive = true;
            playingGame = true;
            //TODO desactivar la imagen del menu
            player1.isAlive = true;
            player1.playerAnimator.SetTrigger("PlayerWakeUp");
            player1.playerAnimator.SetTrigger("PlayerRun");
            ActivateSpawners();
        }
    }

    void Credits(){
        player1.playerAnimator.SetTrigger("PlayerRemainDead");
        //TODO mostrar creditos
    }

    void PlayerDeath(){
        player1.playerAnimator.SetTrigger("PlayerDie");
        player2.enabled = false;
    }

    void PlayGame(){
        totalTime += Time.deltaTime;
        for (int i = 0; i < spawners.Length; i++)
            spawners[i].GetComponent<spawnerController>().waitTimeTotal = SpawnFunction(totalTime);
    }

    int SpawnFunction(float time){
        return (int)(40 / (time + 5));
    }

    void ActivateSpawners(){
        for (int i = 0; i < spawners.Length; i++)
            spawners[i].GetComponent<spawnerController>().isActive = true;;
    }
}
