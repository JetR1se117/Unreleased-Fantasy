using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterTEST : MonoBehaviour
{
    public Rigidbody bulletPrefab;
    public float shootSpeed = 300;
    public Transform playerTrans;

    void shootfire()
    {
        // Instantiate a new bullet at the players position and rotation
        // later you might want to add an offset here or 
        // use a dedicated spawn transform under the player
        var projectile = Instantiate(bulletPrefab, playerTrans.position, playerTrans.rotation);

        //Shoot the Bullet in the forward direction of the player
        projectile.velocity = transform.forward * shootSpeed;
    }
}
