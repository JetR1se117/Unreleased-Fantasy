using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Spawner : MonoBehaviour
{
    public float projectileSpeed;

    public float projectileLife = 3f;

    private Rigidbody projectileRB;
    
    public float speed = 5f;
    public float lifetime = 2f;
    private float lifetimer;

    void Awake()
    {
      
        projectileRB = GetComponent<Rigidbody>();
    }

    void Start()
    {
        //projectileRB.velocity = Vector3.forward * projectileSpeed;
        Destroy(gameObject, projectileLife);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("EGG");
        }
    }
    // Use this for initialization
    void OnEnable()
    {
        lifetimer = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        lifetimer -= Time.deltaTime;
        if (lifetimer <= 0f)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }


    }
}

    /*
 :::::OLD SCRIPT:::::
        GameObject fireblast = Instantiate(projectile, transform) as GameObject;
            Rigidbody rb = fireblast.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileSpeed);
 */
