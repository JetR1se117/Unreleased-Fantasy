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

/*
 :::::OLD SCRIPT:::::
        GameObject fireblast = Instantiate(projectile, transform) as GameObject;
            Rigidbody rb = fireblast.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileSpeed);    
 * 
 */
}
