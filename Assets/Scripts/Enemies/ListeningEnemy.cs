using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ListeningEnemy : MonoBehaviour
{
    
    public static Action EnemyInLight;
    public static Action KillPlayer;
    [Header("Detection & Pathfinding")]
    [SerializeField] private float detectionRadius = 5f; // Radius to checkl for light sources
    [SerializeField] public bool isInPlayerSight;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private float followSpeed = 5f;
    [Header("Light Detection")]
    [SerializeField] private LayerMask lightLayer; // Layer for light emitting objects
    [SerializeField] public float lightIntensity = 1f; // Minimum intensity of light source to detect
    [SerializeField] public float currentIntensity = 0f; // Current intensity of light source
    [Header("Game Objects")]
    [SerializeField] private GameObject _enemy;
    
    private float aggressivness;
    private Transform _player;
    private Rigidbody _rb;
    private bool _followPlayer;
    private bool _makingNoise;
    private SphereCollider _listeningRadius;
    
    


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
        aggressivness = 0;
        _rb = GetComponent<Rigidbody>();
        _listeningRadius = GetComponent<SphereCollider>();
        _listeningRadius.radius = detectionRadius;
        StartCoroutine(DetectLightReactToLight());
    }
    

    // Update is called once per frame
    void Update()
    {
        if (_player)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
            if (distanceToPlayer <= stoppingDistance + 2 && !isInPlayerSight)
            {
                print("Check 1");
                _agent.speed = followSpeed;
                // Set the player's position as the NavMeshAgent's destination
                _agent.SetDestination(_player.position);
            }
            // Checking if the player is in range, making noise
            else if (_followPlayer && _makingNoise && _player)
            {
                print("check 4");
                if (isInPlayerSight)
                {
                    print("check 2");
                    _agent.ResetPath();
                    return;
                }
                FollowPlayer();
            }
            else
            {
                print("check 3");
                _agent.ResetPath();
            }
            
            
        }
    }
    
    // Checks if the enemy is in the player sound range
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _followPlayer = true;
            Debug.Log("In radius");
            _player = other.gameObject.transform;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        print("collided");
        if (other.gameObject.CompareTag("Player"))
        {
            KillPlayer?.Invoke();
        }
    }

    private void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        
        if (distanceToPlayer > stoppingDistance)
        {
            float targetSpeed = Mathf.Lerp(0, followSpeed, distanceToPlayer / detectionRadius);
            _agent.speed = targetSpeed;
            // Set the player's position as the NavMeshAgent's destination
            _agent.SetDestination(_player.position);
        }
        else
        {
            // Stop moving if within stopping distance
            _agent.ResetPath();
            _agent.speed = 0;
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
