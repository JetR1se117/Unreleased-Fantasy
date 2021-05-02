using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireBlast : MonoBehaviour
{
    public bool alllowButtonHold;

    bool shooting, readyToShoot, reloading;

    public int magazinSize, bulletsPerTab;
    int bulletLeft, bulletShot;

    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;

    public TextMeshProUGUI text;

    public Camera cam;
    private Vector3 destination;
    public GameObject bulletPrefab;
    public float shootSpeed = 300;
    public Transform playerTrans;
    public Transform firePoint;
    public float lifeTime;


    private void Awake()
    {
        bulletLeft = magazinSize;
        readyToShoot = true;
    }
    private void Update()
    {
        
        MyInput();

        text.SetText(bulletLeft + "/" + magazinSize);
    }
    public void MyInput()
    {
        if (alllowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletLeft < magazinSize && !reloading) Reload();

        if (readyToShoot && shooting && !reloading && bulletLeft > 0)
        {
            bulletShot = bulletsPerTab;
            Shoot();
        }
    }

    public void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    public void ReloadFinished()
    {

        bulletLeft = magazinSize;
        reloading = false;
    }

    public void Shoot()
    {
        readyToShoot = false;


        Ray ray = cam.ViewportPointToRay(new Vector3(0.48f, 0.5f, 0f)); //creates direction based on camera
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))// if raycast hits something make it as destination
        {
            destination = hit.point;
            
        }
        else
        {
            destination = ray.GetPoint(75); // if not just make point on 1000 from camera as destination
            
        }
        Vector3 directionWithoutSpread = destination - firePoint.position;

        //spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
        // Instantiate a new bullet at the players position and rotation
        // later you might want to add an offset here or 
        // use a dedicated spawn transform under the player
        GameObject projectile = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        projectile.transform.forward = directionWithSpread.normalized;
        projectile.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootSpeed, ForceMode.Impulse);

        //Shoot the Bullet in the forward direction of the player
        //projectile.GetComponent<Rigidbody>().velocity = (destination - firePoint.transform.position).normalized * shootSpeed; //make distanation minus start point as direction and increase it by bullet speed
        if (destination != null)
        {
            Destroy(projectile, lifeTime);
        }

        bulletLeft--;
        bulletShot--;
        Invoke("ResetShoot", timeBetweenShooting);
        
        if(bulletShot > 0 && bulletLeft > 0)
        Invoke("Shoot", timeBetweenShots);
    }

    public void ResetShoot()
    {
        readyToShoot = true;
    }

    //void shootfire()
    //{
//    Ray ray = cam.ViewportPointToRay(new Vector3(0.55f, 0.5f, 0f)); //creates direction based on camera
//    RaycastHit hit;
//        if(Physics.Raycast(ray, out hit))// if raycast hits something make it as destination
//        {
//            destination = hit.point;
//        }
//        else
//{
//    destination = ray.GetPoint(1000); // if not just make point on 1000 from camera as destination
//}

//CoolDown = 1f;
//// Instantiate a new bullet at the players position and rotation
//// later you might want to add an offset here or 
//// use a dedicated spawn transform under the player
//var projectile = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);

////Shoot the Bullet in the forward direction of the player
//projectile.GetComponent<Rigidbody>().velocity = (destination - firePoint.transform.position).normalized * shootSpeed; //make distanation minus start point as direction and increase it by bullet speed 
    //}
}
