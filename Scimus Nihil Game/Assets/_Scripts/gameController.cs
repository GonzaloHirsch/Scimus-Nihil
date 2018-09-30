using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class gameController : MonoBehaviour {

    public static gameController instance = null;
    [HideInInspector]
    public float plantSpawnTime;
    public GameObject[] spawners;
    public GameObject clouds;
    public player1Controller player1;
    public player2Controller player2;
    public Image creditsBG;
    public Image titleScreen;
    public Image credits;

    public AudioSource endAudio;
    public AudioSource menuAudio;
    public AudioSource startAudio;
    public AudioSource gameAudio;

    public delegate void PlayerDies();
    public static event PlayerDies OnPlayerDeath;

    private float totalTime = 0f;
    private bool playingGame = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        credits.enabled = false;
        creditsBG.enabled = false;
    }

    void Start () {
        ActivateMainMenu();
    }

    public float t = 0;
    bool isEnding = false;

	void Update () {
        DeactivateMainMenu();
        if (playingGame){
            PlayGame();
            if (!player1.isAlive){
                playingGame = false;
                PlayerDeath();
                isEnding = true;
            }
        } else if (!playingGame){
            
            if (isEnding)
            {
                t += Time.deltaTime;
                if (t >= 1f)
                {
                    OnPlayerDeath();

                    isEnding = false;

                    Credits();
                }
            }
        }
	}

    void ActivateMainMenu(){
        player2.enabled = false;
        clouds.SetActive(false);
        menuAudio.Play();
        titleScreen.enabled = true;
    }

    void DeactivateMainMenu(){
        if(Input.GetKeyDown(KeyCode.Return) && !playingGame){
            playingGame = true;
            player1.isAlive = true;
            player2.enabled = true;
            clouds.SetActive(true);

            menuAudio.DOFade(0, 2);

            titleScreen.DOFade(0.0f, 1.5f);

            startAudio.Play();
            startAudio.DOFade(1, 2);

            player1.playerAnimator.SetTrigger("PlayerWakeUp");
            player1.playerAnimator.SetTrigger("PlayerRun");
            ActivateSpawners();
        }
    }

    void Credits(){
        creditsBG.DOFade(1f, 10f);
    }

    void PlayerDeath(){
        player1.playerAnimator.SetTrigger("PlayerDie");
        DeactivateSpawners();
        player2.enabled = false;
        startAudio.DOFade(0, 0.5f);
        endAudio.Play();
        endAudio.DOFade(1, 0.5f);
    }

    void PlayGame(){
        totalTime += Time.deltaTime;
        for (int i = 0; i < spawners.Length; i++)
        {
            if (SpawnFunction(totalTime) == 0)
                spawners[i].GetComponent<spawnerController>().waitTimeTotal = 0.75f;
            else
                spawners[i].GetComponent<spawnerController>().waitTimeTotal = SpawnFunction(totalTime);
        }
    }

    int SpawnFunction(float time){
        return (int)(40 / (time + 5));
    }

    void ActivateSpawners(){
        for (int i = 0; i < spawners.Length; i++)
            spawners[i].GetComponent<spawnerController>().isActive = true;;
    }

    void DeactivateSpawners()
    {
        for (int i = 0; i < spawners.Length; i++)
            spawners[i].GetComponent<spawnerController>().isActive = false; ;
    }
}

