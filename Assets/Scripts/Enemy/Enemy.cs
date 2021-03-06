using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 5;
    public int damage = 5;
    private bool Killed = false;
    public bool killed { get { return Killed; } }
    void OnTriggerEnter(Collider otherCollider)
    {
        
                if (health <= 0)
                {
                    if (Killed == false)
                    {
                        Killed = true;
                        OnKill();
                    }


                }
        
    }
    protected virtual void OnKill()
    {

    }
}