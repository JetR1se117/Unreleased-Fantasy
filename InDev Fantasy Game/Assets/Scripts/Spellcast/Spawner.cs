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

    /*
 :::::OLD SCRIPT:::::
        GameObject fireblast = Instantiate(projectile, transform) as GameObject;
            Rigidbody rb = fireblast.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileSpeed);
 */
}