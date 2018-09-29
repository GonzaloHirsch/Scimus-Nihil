using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour {

    public player1Controller player1;
    public static gameController instance = null;
    public float plantSpawnTime;

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
        MainMenu();
        PlayGame();
	}
	
	void Update () {
		
	}

    void MainMenu(){

    }

    void Credits(){

    }

    void PlayGame(){
        player1.isAlive = true;
    }

    int SpawnFunction(float time){
        return (int)(40 / (time + 5));
    }
}
