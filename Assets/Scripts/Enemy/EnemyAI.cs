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
    public GameObject currentEnemyGO;

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
            partical.transform.parent = currentEnemyGO.transform;
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
                if (distanceToWalkPoint.magnitude < 0.7f)
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

    public bool canFire;        //jc
    public bool canMelee;       //jc
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
            //jc - I changed around the order for the if statements
            //Attack

            if (canMelee)
            {
            }
            if (canFire)
            {
                shootfire();
            }
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            alreadyAttacked = true;
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

    private void OnCollisionEnter(Collision other)//jc
    {
        if (other.transform.tag == "")
        {
            TakeDamage(10);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void TakeDamage(float damage)
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
        Debug.Log(effect);
        if (effectParticle.Length == 0)
        {
            return;
        }
        else
        {
            switch (effect)
            {
                case 1:
                    
                    partical = Instantiate(effectParticle[effect - 1], currentEnemyGO.transform.position, Quaternion.identity);
                    Destroy(partical, 3);
                    break;
            }
        }
    }
    public void CurrentEnemyGO(GameObject GO)
    {
        currentEnemyGO = GO;
    }
}


/*  ::::: Legacy Enemy AI Health Script :::::
     public int health = 5;
    public int damage = 5;
    private bool Killed = false;
    public bool killed { get { return Killed; } }
    void OnTriggerEnter(Collider otherCollider)
    {
        
                if (health <= 0)
                {
                    if (Killed == false)
                    {
                        Killed = true;
                        OnKill();
                    }


                }
        
    }
    protected virtual void OnKill()
    {

    }
 */