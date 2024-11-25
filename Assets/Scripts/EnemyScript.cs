using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    
    public static Action EnemyInLight;
    [SerializeField] private float detectionRadius = 5f; // Radius to checkl for light sources
    [SerializeField] private LayerMask lightLayer; // Layer for light emitting objects
    [SerializeField] public float lightIntensity = 1f; // Minimum intensity of light source to detect
    [SerializeField] private Vector2 pathfindingOffset = new Vector2(5, 5); // Offset for pathfinding
    public float currentIntensity = 0f; // Current intensity of light source
    public bool isInLight;
    public Transform player;
    public NavMeshAgent agent;
    public bool isInPlayerSight;

    // State of the enemy
    // 0 - Idle state
    // 1 - Player is in radius and activates a timer which builds up the intensity
    // 2 - Enemy goes behind the player and follows making noises (Gets cancelled if player shines light on enemy)
    // 3 - Enemy attacks the player
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(DetectLightReactToLight());
    }
    
    
    private IEnumerator DetectLightReactToLight()
    {
        while (true)
        {
            DetectLight();
            ReactToLight();
            yield return new WaitForSeconds(1);
            
        }
    }
    
    void DetectLight()
    {
        // Creating an array of colliders that are within the detection radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, lightLayer);
        currentIntensity = 0f;
        
        // Looping through all the colliders in the array
        foreach (Collider col in hitColliders)
        {
            // Checking if the collider has a light component
            Light light = col.GetComponent<Light>();
            if (light.isActiveAndEnabled && light != null)
            {
                Vector3 directionToEnemy = (transform.position - light.transform.position).normalized;
                // Calculating the distance between the enemy and the light source
                if (IsEnemyInLightCone(light, directionToEnemy))
                {
                    float distance = Vector3.Distance(transform.position, light.transform.position);
                    float intensity = light.intensity / (distance * distance);
                    currentIntensity += intensity;
                }
                
            }
        }
    }

    bool IsEnemyInLightCone(Light light, Vector3 directionToEnemy)
    {
        if (light.type == LightType.Spot)
        {
            float angle = Vector3.Angle(light.transform.forward, directionToEnemy);
            if (light.gameObject.CompareTag("Player"))
            {
                isInPlayerSight = true;
            }
            return angle <= light.spotAngle / 2;
        } else if (light.type == LightType.Directional)
        {
            return Vector3.Dot(-light.transform.forward, directionToEnemy) > 0;
        }
        else
        {
            return true;
        }
    }
    
    // Method to react to the light
    void ReactToLight()
    {
        if (isInPlayerSight)
        {
            Destroy(gameObject);
            return;
        }
        if (currentIntensity > lightIntensity)
        {
            Debug.Log("Enemy in light");
            isInLight = true;
            EnemyInLight?.Invoke();
        } else {
            isInLight = false;
        }
    }
    // Method to draw the detection radius in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

