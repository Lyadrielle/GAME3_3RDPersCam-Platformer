using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float walkSpeed = 12f, runSpeed = 24f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    Vector3 velocity;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    /*public LayerMask wallMask;
    public Transform orientation;
    public float wallRunForce = 5, maxWallRunTime, maxWallRunSpeed;
    bool isWallRunning, isWallRight, isWallLeft;
    public bool maxWallRunCameraTilt, wallRunCameraTilt;*/

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    void start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Gravity();
        Jumping();
        WalkingAndRunning();

        //ChecForWall();
    }

    void Gravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Jumping()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    void WalkingAndRunning()
    {
        float playerSpeed = walkSpeed;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
            playerSpeed = runSpeed;
        else
            playerSpeed = walkSpeed;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 movDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(movDir.normalized * playerSpeed * Time.deltaTime);
        }
    }

    /*private void WallRunInput()
    {
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            StartWallRun();
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            StartWallRun();
    }

    private void StartWallRun()
    {
        if(!Input.GetKey(KeyCode.LeftShift)) return;

        GetComponent<Rigidbody>().useGravity = false;
        isWallRunning = true;

        if(isWallRight)
            GetComponent<Rigidbody>().AddForce(orientation.right * wallRunForce / 5 * Time.deltaTime);
        else
            GetComponent<Rigidbody>().AddForce(-orientation.right * wallRunForce / 5 * Time.deltaTime);
    }

    private void StopWallRun()
    {
        GetComponent<Rigidbody>().useGravity = true;
        isWallRunning = false;
    }

    private void ChecForWall()
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, 1f, wallMask);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 1f, wallMask);

        //Leave wall run if no walls
        if(!isWallRight && !isWallLeft) StopWallRun();
    }*/
}
