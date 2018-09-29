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
        MainMenu();
	}
	
	void Update () {
        PlayGame();
	}

    void MainMenu(){
        player1.isAlive = true;
    }

    void Credits(){

    }

    void PlayGame(){
        totalTime += Time.deltaTime;
        for (int i = 0; i < spawners.Length; i++)
            spawners[i].GetComponent<spawnerController>().waitTimeTotal = SpawnFunction(totalTime);
    }

    int SpawnFunction(float time){
        return (int)(40 / ((time/7) + 5));
    }
}
