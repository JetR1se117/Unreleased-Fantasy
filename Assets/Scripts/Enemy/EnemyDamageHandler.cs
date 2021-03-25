using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour
{
    public float shootDamage;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            PlayerController player = other.GetComponent<PlayerController>();
            DashAndBlock block = other.GetComponent<DashAndBlock>();
            if(!block.blockOn == true)
            {
                player.Shot(shootDamage);
            }
            
        }
    }
}
