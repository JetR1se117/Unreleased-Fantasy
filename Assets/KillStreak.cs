using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillStreak : MonoBehaviour
{
    public int  killStreak = 0;
    
    public void TagetHitted(GameObject GO, bool killStreakCounted)
    {
        if (GO != null)
        {
            EnemyAI target = GO.GetComponent<EnemyAI>();
            if (target.health <= 0)
            {
                killStreak++;
                Debug.Log("kill steak: " + killStreak);
            }
        }
    }
}
