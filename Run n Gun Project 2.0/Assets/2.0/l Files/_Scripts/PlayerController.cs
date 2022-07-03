using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using System.Collections;
using Cinemachine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public float playerHP = 100.0f;
    public bool isPlayerDead = false;

    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject aimCamera;
    [SerializeField]
    private GameObject thirdPersonCamera;

    public float easyMovementSpeed = 2.0f;
    public float normalMovementSpeed = 20.0f;
    public float hardMovementSpeed = 30.0f;
    public float aimMovementSpeed = 7.0f;
    public float movementSpeed = 15.0f;
    private float movementTemp;

    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float rollTime = 0.3f;
    [SerializeField]
    private float gravityValue = -9.81f;

    [SerializeField]
    private int bulletDamage = 20;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform barrelTransform;
    [SerializeField]
    private Transform bulletParent;
    [SerializeField]
    private float bulletHitMissDistance = 25f;

    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private Rig weaponArmRig;
    [SerializeField]
    private Rig aimRig;
       
    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction shootAction;

    [SerializeField]
    private float animationPlayTransition = 0.15f;
    [SerializeField]
    private Transform aimTarget;
    [SerializeField]
    private float aimDistance = 5f;
    private Animator animator;
    public bool isAimModeActive = false;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    private int jumpAnimation;
    private int rollAnimation;
    private int recoilAnimation;

    public bool turnAvailable = false;
    public float turnDuration = 0.5f;

    private bool autoRoll = false;
    private bool autoJump = false;

    [SerializeField]
    private Rigidbody[] ragdollBodies;


    [HideInInspector]
    public Vector2 movementInput;
    [HideInInspector]
    public bool touchJump;
    [HideInInspector]
    public bool touchRoll;
    [HideInInspector]
    public bool touchShoot;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();       

        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        shootAction = playerInput.actions["Shoot"];

        //Cursor.lockState = CursorLockMode.Locked;

        animator = GetComponent<Animator>();
        jumpAnimation = Animator.StringToHash("Jump");
        rollAnimation = Animator.StringToHash("Roll");
        recoilAnimation = Animator.StringToHash("PistolShootRecoil");
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
    }

    private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => ShootGun();
    }

    public void ActiveAimMode()
    {
        if (!isAimModeActive)
        {
            movementTemp = movementSpeed;
            movementSpeed = aimMovementSpeed;

            weapon.SetActive(true);
            weaponArmRig.weight = 1.0f;
            aimRig.weight = 1.0f;
            isAimModeActive = true;
            aimCamera.SetActive(true);
            thirdPersonCamera.SetActive(false);
        }        
    }

    public void DeactiveAimMode()
    {
        if (isAimModeActive)
        {
            movementSpeed = movementTemp;
            weapon.SetActive(false);
            weaponArmRig.weight = 0.0f;
            aimRig.weight = 0.0f;
            isAimModeActive = false;
            thirdPersonCamera.SetActive(true);
            aimCamera.SetActive(false);
        }        
    }

    public void ShootGun()
    {
        if (isAimModeActive && gameManager.gameIsActive)
        {
            RaycastHit hit;
            GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
            {
                bulletController.target = hit.point;
                bulletController.hit = true;
            }
            else
            {
                bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
                bulletController.hit = false;
            }
            animator.CrossFade(recoilAnimation, animationPlayTransition);
            FindObjectOfType<AudioManager>().Play("Shoot");


            if (hit.transform.CompareTag("EnemyAI"))
            {
                EnemyScript enemy = hit.transform.GetComponent<EnemyScript>();
                enemy.TakeDamage(bulletDamage);
            }
        }              
    }

    void Update()
    {
        if (gameManager.gameIsActive)
        {
            PlayerMovement();
        }        
    }


    void PlayerMovement()
    {
        aimTarget.position = cameraTransform.position + cameraTransform.forward * aimDistance;

        groundedPlayer = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        // Changes the height position of the player..
        if (groundedPlayer && !isAimModeActive && (Input.GetKeyDown(KeyCode.W) || touchJump || autoJump))
        {
            touchJump = false;
            autoJump = false;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(jumpAnimation, animationPlayTransition);
            Debug.Log("Jumping");
        }

        // Player Roll
        if(groundedPlayer && !isAimModeActive && (Input.GetKeyDown(KeyCode.S) || touchRoll || autoRoll))
        {
            touchRoll = false;
            autoRoll = false;
            animator.CrossFade(rollAnimation, animationPlayTransition);
            StartRolling();
            Debug.Log("Rolling");
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //movementInput = moveAction.ReadValue<Vector2>();
        if (!isAimModeActive && turnAvailable && Mathf.Abs(movementInput.x) > 0f)
        {
            var temp = 0.0f;
            if (movementInput.x < -0.2)
            {
                temp = -1.0f;
            }
            else if (movementInput.x > 0.2f)
            {
                temp = 1.0f;
            }
            Turn(temp);
        }
        // This line make sure that player moves based on the current local rotation of the player.
        Vector3 move = transform.right * movementInput.x + transform.forward;

        controller.Move(move * Time.deltaTime * movementSpeed);


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
            animator.enabled = false;
            gameManager.GameOver();
            isPlayerDead = true;

            foreach(Rigidbody rb in ragdollBodies)
            {
                rb.AddForce(- transform.forward * 30f, ForceMode.Impulse);
            }

        }
    }

    public void TakeDamage(float damage)
    {
        playerHP -= damage;

        if(playerHP <= 0)
        {
            animator.enabled = false;
            gameManager.GameOver();
            isPlayerDead = true;
        }

    }
}