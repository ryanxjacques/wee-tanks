using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Special Thanks to Dave from "Dave / GameDevelopment" on YouTube for the helpful YouTube video.
 * Credit: Dave / GameDevelopment YouTube Channel.
 * Link: https://youtu.be/UjkSFoLxesw?si=BJ_zg2p13_gKFUM5
 */
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(enemy_hit))]
public class ShooterController : MonoBehaviour
{
    private NavMeshAgent agent;
    private PlayerController _playerController;
    private GameObject player;
    public LayerMask whatIsVisible;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsGround;

    public GameObject bulletPrefab;
    public float fireRate;
    private float nextFire = 0.0f;

    public float sightRange;
    public float attackRange;
    public float walkPointRange;

    private bool playerInSightRange;
    private bool playerInAttackRange;

    private Vector3 walkPoint;
    private bool walkPointSet;

    private bool isPatrolling;
    private bool isChasing;
    private bool isAttacking;
    
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        _playerController = player.GetComponent<PlayerController>();
    }

    // Modified version of "Dave / GameDevelopment"'s implementation.
    private void Update()
    {   
        // Stop updating if the player is destroyed.
        if (player == null)
            return;

        // Check if the shooter can see the player.
        if (CheckVision() == false)
        {
            Patrolling();
            return;
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) Chasing();
        if (playerInSightRange && playerInAttackRange) Attacking();
    }

    // Credit "Dave / GameDevelopment"
    private void Patrolling()
    {
       
        if (!walkPointSet) {
            FindWalkPoint();
        }
        if (walkPointSet) {
            agent.SetDestination(walkPoint);
        }
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 8f)
            walkPointSet = false;
    }

    // Modified version of "Dave / GameDevelopment"'s implementation.
    private void FindWalkPoint()
    {
        // Choose a random point in the walk range.
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomY = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(randomZ, transform.position.y, randomY);
        
        // Check if the walkPoint is in the map.
        // If walkPoint was out-of-bounds, FindWalkPoint() will be called again.
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    // Credit "Dave / GameDevelopment"
    private void Chasing()
    {
        agent.SetDestination(player.transform.position);
    }

    // Modified version of "Dave / GameDevelopment"'s implementation.
    private void Attacking()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player.transform);

        // Cooldown implementation comes from the Unity API "manual pages".
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }
    }
    private void Shoot()
    {
        Vector3 bulletPosition = transform.position + transform.forward * 5f;
        GameObject bullet = Instantiate(bulletPrefab, bulletPosition, transform.rotation);
        Destroy(bullet, 5f);
    }

    private bool CheckVision()
    {
        // Calculate the direction from the enemy to the player
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position,
                            directionToPlayer,
                            out hit,    // Out keyword means pass by reference rather than value.
                            Mathf.Infinity,
                            whatIsVisible.value))
        {
            // Check if the hit object is the player
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // Ray hit the player.
                return true;
            }
        }
        // Ray did not hit the player.
        return false;
    }
   

    void FixedUpdate(){}
}
