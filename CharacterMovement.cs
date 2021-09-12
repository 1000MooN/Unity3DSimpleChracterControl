using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //Fields marked as public are to be changed in the inspector
    public float speed = 5;
    //cms is current move speed and is here so you can control the move speed via script
    private float cms;
    //cs is current sensitivity and is here so you can control the sensitivity via script
    private float cs;
    private bool isGrounded;
    public float sensitivity = 10;
    private Vector2 movementInput;
    private Vector2 localEulerAnglesInput;
    public LayerMask ground;
    public float JumpForce = 10f;
    public Transform playerCam;
    private Rigidbody rb;
    private bool isWallRunning = false;
    private float camRot;

    [Range(50.0f, 100.0f)]
    public float CamFov;
    //called on start
    private void Start()
    {
        //assign rigidbody
        rb = GetComponent<Rigidbody>();
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    //called every frame
    private void Update()
    {
        float mousey = Input.GetAxisRaw("Mouse Y");
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        localEulerAnglesInput = new Vector2(Input.GetAxisRaw("Mouse X"), mousey);

        if (Input.GetButtonDown("Jump")) Jump();
 
        playerCam.GetComponent<Camera>().fieldOfView = CamFov;

        cs = sensitivity;
        cms = speed;
    
        camRot -= localEulerAnglesInput.y * Time.deltaTime * cs;
        camRot = Mathf.Clamp(camRot, -70, 70);
        isGrounded = Physics.Raycast(transform.position, -transform.up, 2, ground);
        rb.velocity += transform.forward * movementInput.y * cms * Time.deltaTime + transform.right * movementInput.x * cms * Time.deltaTime;
        transform.localEulerAngles += new Vector3(0, localEulerAnglesInput.x, 0) * Time.deltaTime * cs;
        playerCam.localEulerAngles = new Vector3(camRot, playerCam.localEulerAngles.y, playerCam.localEulerAngles.z);
        
    }



    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, JumpForce, rb.velocity.z);
        }
      
    }

    private bool canWallRun = true;
    //called when collision is entered
    private void OnCollisionEnter(Collision other)
    {
        //using dot product to make sure we are infront of or behind the collision normal and not on top of or below
        if (Mathf.Abs(Vector3.Dot(other.GetContact(0).normal, Vector3.up)) < 0.1f && canWallRun)
        {
            rb.velocity = new Vector3(0, 20, 0) + transform.forward * 50;
            isWallRunning = true;
        }
    }
    //called on collision exit
    void OnCollisionExit(Collision other)
    {
        isWallRunning = false;
        StartCoroutine(WallRunCooldown());
    }
    private IEnumerator WallRunCooldown()
    {
        canWallRun = false;
        yield return new WaitForSeconds(0.3f);
        canWallRun = true;
    }
}
