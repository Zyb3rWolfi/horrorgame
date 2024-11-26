using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ListeningEnemy : MonoBehaviour
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
    [SerializeField] private GameObject _enemy;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float stoppingDistance = 1f;
    private Transform _player;
    private Rigidbody _rb;
    private bool _followPlayer;
    private bool _makingNoise;
    

    [SerializeField] private float followSpeed = 5f;

    private void OnEnable()
    {
        LoudnessManager.MakeNoiseAction += IsPlayerMakingNoise;
    }

    private void OnDisable()
    {
        LoudnessManager.MakeNoiseAction -= IsPlayerMakingNoise;
    }

    private void IsPlayerMakingNoise(bool value)
    {
        _makingNoise = value;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(DetectLightReactToLight());
    }

    // Update is called once per frame
    void Update()
    {
        if (_followPlayer && _makingNoise && _player)
        {
            if (isInPlayerSight)
            {
                _agent.ResetPath();
                return;
            }
            FollowPlayer();
        }
        else
        {
            _agent.ResetPath();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _followPlayer = true;
            Debug.Log("In radius");
            _player = other.gameObject.transform;
        }
    }

    private void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            // Set the player's position as the NavMeshAgent's destination
            _agent.SetDestination(_player.position);
        }
        else
        {
            // Stop moving if within stopping distance
            _agent.ResetPath();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _followPlayer = false;
            _player = null;
        }
    }
    
    private IEnumerator DetectLightReactToLight()
    {
        while (true)
        {
            DetectLight();
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
            if (light.gameObject.CompareTag("Player") && angle <= light.spotAngle / 2)
            {
                isInPlayerSight = true;
            }
            else
            {
                isInPlayerSight = false;
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
    
    // Method to draw the detection radius in the scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
