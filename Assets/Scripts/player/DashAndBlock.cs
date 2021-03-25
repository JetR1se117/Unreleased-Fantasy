using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAndBlock : MonoBehaviour
{
    PlayerController playerController;
    public float dashSpeed;
    public float dashTime;
    public bool inDash = false;
    public bool timerActive;
    Vector3 velocityFake;
    public float coolDown;
    public float setCooldown;
    public bool blockOn = false;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            blockOn = true;
            Debug.Log("block is on");

        }else if (Input.GetKeyUp(KeyCode.R))
        {
            blockOn = false;
            Debug.Log("block is of");
        }

            if (Input.GetKey(KeyCode.Q))
        {
            if(inDash == false)
            {
                StartCoroutine(Dash());
            }
        }
        if(timerActive == true)
        {
            DashCooldown();
        }

    }
    IEnumerator Dash()
    {
        coolDown = setCooldown;
        inDash = true;
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            velocityFake = (transform.forward * playerController.currentDir.y + transform.right * playerController.currentDir.x);
            playerController.controller.Move(velocityFake * Time.deltaTime * dashSpeed);
            timerActive = true;
            yield return null;
        }
        
    }
    public void DashCooldown()
    {
        coolDown -= Time.deltaTime;
        if (coolDown <= 0)
        {
            inDash = false;
            timerActive = false;
        }
    }
}
