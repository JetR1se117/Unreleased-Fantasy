using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageAndEffectHandler : MonoBehaviour
{
   
        public float shootDamage;
        public int effect;
        public GameObject hittedEnemy;
        public void OnCollisionEnter(Collision other)
        {
        if (other.collider.tag == "Enemy")
            {
               
                hittedEnemy = other.gameObject;
                EnemyAI target = hittedEnemy.GetComponent<EnemyAI>();
                target.TakeDamage(shootDamage);
                target.CurrentEnemyGO(hittedEnemy);
                target.EffectApply(effect);
            }
        }
    
}
