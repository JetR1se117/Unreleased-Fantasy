using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;
    public float waitingTime;

    //Idle
    public Transform[] navPoints;
    public int destPoint = 0;
    public Vector3 distanceToWalkPoint;
    public bool waiting = false;

    //Attacking 
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, playerInSphere, playerInAngel;
    public Vector3 playerDir;
    public float playerAngel;
    public float maxAngel = 95.0f;

    private void Awake()
    {
        
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        playerDir = player.position - transform.position;
        playerAngel = Vector3.Angle(playerDir, transform.forward);
        playerInAngel = playerAngel < maxAngel;
        playerInSphere = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (playerInAngel && playerInSphere)
        {
            playerInSightRange = true;
        }
        else
        {
            playerInSightRange = false;
        }
           

        if (!playerInSightRange && !playerInAttackRange) Patrol();
        if (playerInSightRange && !playerInAttackRange) Chase();
        if (playerInSightRange && playerInAttackRange) Attack();
    }
    
    private void Patrol()
    {
        if (waiting == false)
        {
            if (navPoints.Length == 0)
            {
                return;
            }
            else
            {
                agent.SetDestination(navPoints[destPoint].position);
                distanceToWalkPoint = transform.position - navPoints[destPoint].position;
            }
            if (distanceToWalkPoint.magnitude < 0.5f)
            {
                waiting = true;
                searchWalkPoint();
                
            }
        }
    }

    private void searchWalkPoint()
    {
        if (waiting == true)
        {
            destPoint = (destPoint + 1) % navPoints.Length;
            StartCoroutine(Wait());
        }  
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }
    private void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack
            Debug.Log("hit");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    IEnumerator Wait()
    {
        
        yield return new WaitForSeconds(waitingTime);
        waiting = false;
        
        
    }
}
