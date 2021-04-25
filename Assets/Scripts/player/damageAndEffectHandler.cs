using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageAndEffectHandler : MonoBehaviour
{
    public float shootDamage;
    public int effect;
    public GameObject hittedEnemy;
    public GameObject player;
    public bool colided;
    private void Start()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        player = playerController.gameObject;
    }
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            hittedEnemy = other.gameObject;
            EnemyAI target = hittedEnemy.GetComponent<EnemyAI>();
            KillStreak tagetToKill = player.GetComponent<KillStreak>();
            target.TakeDamage(shootDamage);
            target.CurrentEnemyGO(hittedEnemy);
            target.EffectApply(effect);
            tagetToKill.TagetHitted(hittedEnemy.gameObject, true);
            Destroy(gameObject);
        }
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "SpellProj" && !colided)
        {
            colided = true;
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "SpellProj")
        {
            Physics.IgnoreCollision(other.collider, other.collider);
        }

    }

}
