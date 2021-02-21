using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{

    [SerializeField] Transform playerCamera = null;
    [SerializeField] float Sensivity = 3.5f;
    [SerializeField] float Walkspeed = 6.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmooth = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmooth = 0.03f;
    [SerializeField] float jumpHeight = 3.0f;

    bool curserlock = true;
    float camerapitch = 0.0f;
    bool jumpcooldown = false;
    
    Vector2 currentDir = Vector2.zero;
    CharacterController controller = null;
    float velocityY = 0.0f;
    Vector2 DirVelocity = Vector2.zero;
    Vector2 currentMouseD = Vector2.zero;
    Vector2 MouseDVelocity = Vector2.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        if(curserlock)
        {
            controller = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        jumpcooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();
        movement();
    }

    void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseD = Vector2.SmoothDamp(currentMouseD, mouseDelta, ref MouseDVelocity, mouseSmooth);

        camerapitch -= mouseDelta.y * Sensivity;

        camerapitch = Mathf.Clamp(camerapitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * camerapitch;

        transform.Rotate(Vector3.up * mouseDelta.x * Sensivity);
    }


    void movement()
    {

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref DirVelocity, moveSmooth);

        if (controller.isGrounded)
        {
            velocityY = 0.0f;
            jumpcooldown = false;
        }

        if (jumpcooldown == false)
        {

            if (Input.GetKey(KeyCode.Space))
            {
                velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);

                jumpcooldown = true;
            }
        }

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * Walkspeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);



        if (Input.GetKey(KeyCode.LeftShift))
        {

            Walkspeed = 10f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Walkspeed = 6f;
        }
    }
}
