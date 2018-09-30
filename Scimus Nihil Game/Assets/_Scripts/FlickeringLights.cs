using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlickeringLights : MonoBehaviour {

    public Light[] lights;
    [Range(0, 100)]
    public int chanceOfFlicker = 70;

    private int lightIndex;
	
	void Update () {
        if (Random.Range(0, 101) <= chanceOfFlicker)
            Flicker();
	}

    void Flicker(){
        lightIndex = Random.Range(0, lights.Length);
        Light myLight = lights[lightIndex];
        DOTween.To(
            getter: () => { return myLight.intensity; },
            setter: (float value) => { myLight.intensity = value; },
            endValue: 0f,
            duration: 0.5f).OnComplete(() => DOTween.To(
            getter: () => { return myLight.intensity; },
            setter: (float value) => { myLight.intensity = value; },
                endValue: Random.Range(4.5f, 6.5f),
            duration: 0.5f));
    }
}
