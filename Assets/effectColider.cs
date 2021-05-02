using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectColider : MonoBehaviour
{
    public int[] effectParticle;
    public GameObject partical;
    public GameObject currentEnemyGO;
    

    public void effectTriger(Vector3 vec, GameObject gm)
    {
        Vector3 thisVector = vec;
        GameObject thisGameObject = gm;

        partical = Instantiate(gm,vec, Quaternion.identity);
    }
    public void destroyEffect(bool bulk)
    {
        if(bulk == true)
        {
            Destroy(partical, 2f);
            bulk = false;
        }
    }
}
