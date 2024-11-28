using System.Collections;
using UnityEngine;

public class FlickeringLightEffect : MonoBehaviour
{
    [SerializeField] private float interval;
    private Light light;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(flicker(interval));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator flicker(float interval)
    {
        while (true)
        {
            light.enabled = true;
            yield return new WaitForSeconds(0.2f);
            light.enabled = false;
            yield return new WaitForSeconds(0.6f);
            light.enabled = true;
            yield return new WaitForSeconds(interval);
        }
    }
}
