using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerController : MonoBehaviour {

    public GameObject plant;
    [HideInInspector]
    public float waitTimeTotal = 8f;

    private float waitTime = 0f;

	void Update () {
        waitTime += Time.deltaTime;
        SpawnPlants();
	}

    public void SpawnPlants(){
        if (waitTime >= waitTimeTotal){
            waitTime = 0f;
            GameObject instance = Instantiate(plant);
            instance.transform.position = transform.position;
        }
            
    }
}
