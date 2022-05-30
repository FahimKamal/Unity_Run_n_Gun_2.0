using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public CharacterController controller;
    public Animator animationController;
    public GameManager gameManager;

    public float easyMovementSpeed = 15.0f;
    public float normalMovementSpeed = 20.0f;
    public float hardMovementSpeed = 30.0f;
    public float movementSpeed = 15.0f;    
    public float jumpHeight = 3f;
    public float rollTime = 0.3f;
    public float gravity = -9.81f;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    public bool turnAvailable = false;
    public float turnDuration = 0.5f;
    float xValue;

    private bool autoRoll = false;
    private bool autoJump = false;

    private void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameIsActive)
        {
            PlayerMovement();
        }
    }

    void PlayerMovement()
    {
        // Apply gravity to player.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animationController.SetBool("isJump", false);
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || autoJump) && isGrounded)
        {
            autoJump = false;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animationController.SetBool("isJump", true);
            Debug.Log("Jumping");
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || autoRoll)
        {
            autoRoll = false;
            animationController.SetBool("isRoll", true);
            StartRolling();
            Debug.Log("Rolling");
        }
        else
        {
            animationController.SetBool("isRoll", false);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (turnAvailable && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f)
        {
            xValue = Input.GetAxisRaw("Horizontal");
            Turn(xValue);
        }

        xValue = Input.GetAxis("Horizontal");
        // This line make sure that player moves based on the current local rotation of the player.
        Vector3 move = transform.right * xValue + transform.forward;

        controller.Move(move * movementSpeed * Time.deltaTime);
    }

    public void Jump()
    {
        autoJump = true;
    }

    public void Roll()
    {
        autoRoll = true;
    }

    public void Turn(float xValue)
    {
        Quaternion startPos = transform.rotation;
        StartCoroutine(RotatePlayer(xValue, startPos));
        turnAvailable = false;
    }

    void StartRolling()
    {
        controller.height = 1.0f;
        var controllerCenter = new Vector3(0, 0.5f, 0);
        controller.center = controllerCenter;
        StartCoroutine(StandUp());
    }

    IEnumerator StandUp()
    {
        bool wait = true;
        while (wait)
        {
            yield return new WaitForSeconds(rollTime);
            controller.height = 2.0f;
            var controllerCenter = new Vector3(0, 1.0f, 0);
            controller.center = controllerCenter;
            wait = false;
        }
    }

    /// <summary>
    /// Rotate the Player smoothly over given time.
    /// </summary>
    /// <param name="xValue"></param>
    /// <param name="startPos"></param>
    /// <returns></returns>
    IEnumerator RotatePlayer(float xValue, Quaternion startPos)
    {
        Quaternion endPos = startPos;
        endPos *= Quaternion.Euler(0f, 90 * xValue, 0f);
        float rotationFraction = 0.0f;

        while (rotationFraction < 1)
        {
            transform.rotation = Quaternion.Lerp(startPos, endPos, rotationFraction / turnDuration);
            rotationFraction += Time.deltaTime;

            yield return null;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            animationController.enabled = false;
            gameManager.GameOver();
        }
    }
}
