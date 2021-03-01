using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlast : MonoBehaviour
{

    public Rigidbody bulletPrefab;
    public float shootSpeed = 300;
    public Transform playerTrans;
    float CoolDown = 0f;
    void Update()
    {
        CoolDown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
           
            if (CoolDown <= 0f)
            {
                shootfire();
            }
        }
    }

    void shootfire()
    {
        CoolDown = 1f;
        // Instantiate a new bullet at the players position and rotation
        // later you might want to add an offset here or 
        // use a dedicated spawn transform under the player
        var projectile = Instantiate(bulletPrefab, playerTrans.position, playerTrans.rotation);

        //Shoot the Bullet in the forward direction of the player
        projectile.velocity = transform.forward * shootSpeed;
    }
}

/*  ::::: MY OLD CODE :::::
 
 
    public GameObject Projectile;
    public Transform spawnTrans;
    public GameObject spawnGamObj;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //Instantiate(Projectile, spawnTrans.position, spawnTrans.rotation);
            Instantiate(Projectile, spawnGamObj.transform.position, spawnGamObj.transform.rotation);
            
        }
    }
    void HitTarget()
    {
        //Send Message
        Destroy(this.gameObject);
    }
}
*/

//      :::::DANS OLD CODE:::::
/*
if (Target != null)
{
    Vector3 targetPostition = new Vector3(Target.transform.position.x, Target.transform.position.y, Target.transform.position.z);
    
    this.transform.LookAt(targetPostition);

    float distance2 = Vector3.Distance(Target.transform.position, this.transform.position);

    if (distance2 > 2.0f)
    {
        transform.Translate(Vector3.forward * 30.0f * Time.deltaTime);
    }
    else
    {
        HitTarget();
    }
}
*/