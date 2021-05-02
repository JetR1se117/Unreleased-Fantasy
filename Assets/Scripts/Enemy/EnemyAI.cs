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
    public bool ableToMove = true;
    public bool ableToAttack = true;
    public float effectWaitingTime;
    public effectColider cloneContainer;

    //from "FireBlast" script<James>
    public Rigidbody bulletPrefab;
    public float shootSpeed = 100;
    public Transform playerTrans;
    Transform effectTaker;
    // </James>

    //Idle
    public Transform[] navPoints;
    public int destPoint = 0;
    public Vector3 distanceToWalkPoint;
    public bool waiting = false;
    private float y;
    private float x;
    private float z;


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
        cloneContainer = FindObjectOfType<effectColider>();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if(currentEnemyGO != null)
        {
            if (currentEnemyGO.GetComponent<EnemyAI>().ableToMove == false)
            {

                currentEnemyGO.transform.position = new Vector3(x, y, z);
            }
        }
        
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
        if (ableToMove == true)
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

    }

    private void searchWalkPoint()
    {

        if (waiting == true)
        {
            destPoint = (destPoint + 1) % navPoints.Length;
            StartCoroutine(Wait(waitingTime));
        }
    }

    public bool canFire;        //jc
    public bool canMelee;       //jc
    private void Chase()
    {
        if (ableToMove == true)
        {
            agent.SetDestination(player.position);
        }
    }
    private void Attack()
    {
        if (ableToAttack == true)
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
    IEnumerator Wait(float time)
    {
        //Debug.Log(time);
        yield return new WaitForSeconds(time);
        
        //if(time == waitingTime)
            waiting = false;

        //else if(time == effectWaitingTime)
        //    ableToAttack = true;
        //    ableToMove = true;
        //    Debug.Log(time);

    }
    IEnumerator WaitEffectTime(float effectTime)
    {
        yield return new WaitForSeconds(effectTime);
        ableToAttack = true;
        ableToMove = true;
        cloneContainer.destroyEffect(true);





    }
    public void CurrentEnemyGO(GameObject GO)
    {
        currentEnemyGO = GO;
        effectTaker = currentEnemyGO.transform.GetChild(2);

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
                    x = currentEnemyGO.transform.position.x;
                    y = currentEnemyGO.transform.position.y;
                    z = currentEnemyGO.transform.position.z;
                    GameObject currEffect = effectParticle[effect - 1];
                    effectTaker = currentEnemyGO.transform.GetChild(2);
                    Vector3 newEffectTakerPostion = new Vector3(effectTaker.transform.position.x - 0.50f, effectTaker.transform.position.y, effectTaker.transform.position.z);

                    cloneContainer.effectTriger(newEffectTakerPostion, currEffect);
                   

                    currentEnemyGO.GetComponent<EnemyAI>().ableToAttack = false;
                    currentEnemyGO.GetComponent<EnemyAI>().ableToMove = false;
                    
                    StartCoroutine(WaitEffectTime(effectWaitingTime));
                    //currentEnemyGO.GetComponent<Rigidbody>().AddForce(Vector3.zero);
                    // Instantiate(effect, position, new Quaternion()) as GameObject;
                    break;
            }
        }
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