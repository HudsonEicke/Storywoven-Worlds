using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public PlayerHealthController playerHealthController;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Vector3 lockedCameraPosition = new Vector3(0f, 10f, -10f);
    public Transform GFX;

    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask safeGroundMask;
    public bool isGrounded;
    public Vector3 lastGroundPosition;
    public bool moveBack = false;
    bool isFroze = false;
    bool onSafeGround = true;

    public bool QueuedMove = false;
    public Vector3 newPosition;

    public CinemachineFreeLook cameraSettings;
    public float defaultXSpeed = 450f;

    public bool hasDoubleJumpPowerUp = false;
    private bool doubleJumpCharged = false;

    private void FixedUpdate()
    {
        if(isFroze)
        {
            return;
        }

        if (moveBack)
        {
            moveBack = false;
            MovePlayer(lastGroundPosition);
        }

        if (QueuedMove)
        {
            QueuedMove = false;
            MovePlayer(newPosition);
        }
    }

    public void MovePlayer(Vector3 newPos)
    {
        gameObject.transform.position = newPos;
    }

    private void Awake()
    {
        GameManager3D.freezeWorld += Freeze;
        GameManager3D.unFreezeWorld += unFreeze;
    }

    private void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Freeze()
    {
        isFroze = true;
    }

    void unFreeze()
    {
        isFroze = false;
    }

    // Update is called once per frame
    void Update()
    {
        //for combat is running
        if (isFroze)
        {
            return;
        }

        if(ImportantComponentsManager.Instance.invetoryUIManager.isInventoryOpen)
        {
            cameraSettings.m_XAxis.m_MaxSpeed = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            cameraSettings.m_XAxis.m_MaxSpeed = defaultXSpeed;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //Added jumping logic stuff
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        onSafeGround = Physics.CheckSphere(groundCheck.position, groundDistance, safeGroundMask);

        //For reverting back to last platform the player was on
        if (onSafeGround)
        {
            lastGroundPosition = gameObject.transform.position;
        }

        //gravity stuff
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            if(hasDoubleJumpPowerUp)
            {
                doubleJumpCharged = true;
            }
        }

        //This is what actually handles jumping
        if(Input.GetButtonDown("Jump"))
        {
            if(isGrounded)
            {
                Jump();
            }
            else if(doubleJumpCharged)
            {
                Jump();
                doubleJumpCharged = false;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //End of added logic
        
        //cam.position = lockedCameraPosition;
        //cam.rotation = Quaternion.Euler(0f, 45f, 0f);
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(GFX.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            GFX.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    public void MoveWithPlatform(Vector3 platformVelocity)
    {
        controller.Move(platformVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
