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

    //from "FireBlast" script<James>
    public Rigidbody bulletPrefab;
    public float shootSpeed = 100;
    public Transform playerTrans;
    // </James>

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

    //Effects
    public GameObject[] effectParticle;
    public GameObject partical;

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
        if (partical != null)
        {
            partical.transform.parent = GameObject.Find("Enemy").transform;
        }
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

            shootfire();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    void shootfire()    // <James>
    {
        // Instantiate a new bullet at the players position and rotation
        // later you might want to add an offset here or 
        // use a dedicated spawn transform under the player
        var projectile = Instantiate(bulletPrefab, playerTrans.position, playerTrans.rotation);

        //Shoot the Bullet in the forward direction of the player
        projectile.velocity = transform.forward * shootSpeed;
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
    public void EffectApply(int effect)
    {
        if (effectParticle.Length == 0)
        {
            return;
        }
        else
        {
            switch (effect)
            {
                case 1:
                    partical = Instantiate(effectParticle[effect], agent.transform.position, Quaternion.identity) as GameObject;
                    Destroy(partical, 3);
                    break;
                case 2:
                    Instantiate(effectParticle[effect], transform.position, Quaternion.identity);
                    effectParticle[effect].transform.parent = agent.transform;
                    break;
                case 3:
                    Instantiate(effectParticle[effect], transform.position, Quaternion.identity);
                    effectParticle[effect].transform.parent = agent.transform;
                    break;
                case 4:
                    Instantiate(effectParticle[effect], transform.position, Quaternion.identity);
                    effectParticle[effect].transform.parent = agent.transform;
                    break;
            }

        }
    }
}
