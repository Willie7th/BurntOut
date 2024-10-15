using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Flicker : MonoBehaviour
{
    public Light2D myLight;
    // Start is called before the first frame update
    public float maxInterval = 1;
    public float maxFlicker = 0.2f;
    public float minRadius = 0.7f;

    float defaultRadius;
    bool isOn;
    float timer;
    float delay;

    private void Start()
    {
        defaultRadius = myLight.pointLightOuterRadius;
    }

    void Update()
    {
        // The radius may have changed since the last flicker
        if (isOn) defaultRadius = myLight.pointLightOuterRadius;
        timer += Time.deltaTime;
        if (timer > delay)
        {
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        isOn = !isOn;

        if (isOn)
        {
            myLight.pointLightOuterRadius = defaultRadius;
            delay = Random.Range(0, maxInterval);
        }
        else
        {
            myLight.pointLightOuterRadius = Random.Range(defaultRadius*minRadius, defaultRadius);
            delay = Random.Range(0, maxFlicker);
        }

        timer = 0;
    }
}
