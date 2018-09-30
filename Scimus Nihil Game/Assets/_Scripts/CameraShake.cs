using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private player1Controller playerController;

    private void Start(){
        playerController = GameObject.FindWithTag("Player").GetComponent<player1Controller>();
    }

    private void Update(){
        Shake();
    }

    void Shake(){
        transform.position += (Random.insideUnitSphere * (playerController.nearCount / playerController.plantCountDeath));
    }
}
