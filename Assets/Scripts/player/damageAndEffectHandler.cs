using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageAndEffectHandler : MonoBehaviour
{
    public float shootDamage;
    public int effect;
    public GameObject hittedEnemy;
    public GameObject player;
    private void Start()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        player = playerController.gameObject;
    }
    public void OnTriggerEnter(Collider other)
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
        }
    }

}
