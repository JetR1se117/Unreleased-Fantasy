using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillStreak : MonoBehaviour
{
    public int  killStreak = 0;
    public float killStreakTimer = 0;
    public float time;
    public void TagetHitted(GameObject GO, bool killStreakCounted)
    {
        if (GO != null)
        {
            EnemyAI target = GO.GetComponent<EnemyAI>();
            if (target.health <= 0)
            {
                killStreak++;
                killStreakTimer = time;
                Debug.Log("kill steak: " + killStreak);
            }
        }
    }
    private void Update()
    {
        if (killStreakTimer > 0) // check if timer is not zero 
        {
            killStreakTimer -= Time.deltaTime; // starts timer after kill
            Debug.Log("time: " + killStreakTimer);
            if (killStreakTimer <= 0)
            {
                killStreak = 0;// if timer is 0 kill streaks is 0
            }
        }
    }
}
