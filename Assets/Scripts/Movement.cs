using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private KeyCode[] AbilityControls = new KeyCode[4];

    [SerializeField]
    private KeyCode jumpKey;
    [SerializeField]
    private KeyCode slideKey;

    [SerializeField]
    private float speed = 12f;
    [SerializeField]
    private float slideSpeed = 1.5f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float wallrunGravity;
    [SerializeField]
    private float jumpHeight = 3f;

    [SerializeField]
    private float reducedHeight;
    [SerializeField]
    private float slideDurtion = 3f;
    [SerializeField]
    private float time;

    [SerializeField]
    private LayerMask wall;
    [SerializeField]
    private float wallrunForce;
    [SerializeField]
    private float maxWallrunTime;
    [SerializeField]
    private float maxwallrunSpeed;
    [SerializeField]
    private float maxWallRunCameraTilt;
    [SerializeField]
    private float wallRunCameraTilt;
    [SerializeField]
    private float wallrunDistance;

    private bool isWallRight;
    private bool isWallLeft;

    [SerializeField]
    private GameObject groundCheck;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float groundDistance = 0.4f;

    [SerializeField]
    private KeyCode mouseFocusOn;
    [SerializeField]
    private KeyCode mouseFocusOff;
    [SerializeField]
    private float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private Camera maincamera;

    private float currentSpeed;
    private float originalHeight;
    private bool isGrounded = false;
    private bool isSliding = false;
    private bool isWallRunning = false;

    private Vector3 move;
    private Vector3 velocity;
    private CharacterController characterController;
    private GetStats getStats;
    private GetSkillIcons skillIcons;
    private SphereCollider colliderForRange;
    public List<GetStats> targetsInRange = new List<GetStats>();

    private void Awake()
    {
        skillIcons = GetComponentInChildren<GetSkillIcons>();
        maincamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        colliderForRange = GetComponent<SphereCollider>();
        getStats = GetComponent<GetStats>();
        getStats.selectedSkill = getStats.hero.basicAttack;
        originalHeight = characterController.height;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MyInput();
        Look();
        CheckForAbilities();
        CreateRangeField();
        CheckForWall();
        WallrunInput();
        Gravity();

    }

    private void MyInput()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundDistance, groundMask);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (isWallRunning)
            if (z < 0)
                z = 0;

        move = transform.right * x + transform.forward * z;
        characterController.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetKeyDown(slideKey) && Input.GetKey(KeyCode.W) && !isSliding && isGrounded)
        {
            Sliding();
            Invoke("GetUp", slideDurtion);
        }

        else if (Input.GetKeyUp(slideKey))
        {
            GetUp();
        }
        else if (!Input.GetKey(slideKey))
            currentSpeed = speed;

        if (isWallRunning)
            currentSpeed = maxwallrunSpeed;

        if (Input.GetKeyDown(jumpKey) && (isGrounded || isWallRunning))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //if(Input.GetMouseButtonDown(0))
    }

    private void Look()
    {
        if (Input.GetKeyDown(mouseFocusOn))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (Input.GetKeyDown(mouseFocusOff))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        maincamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

    }

    private void CheckForAbilities()
    {
        for (int i = 0; i < AbilityControls.Length; i++)
        {
            if (Input.GetKeyDown(AbilityControls[i]))
            {
                for (int m = 0; m < skillIcons.icons.Count - 1; m++)
                    skillIcons.icons[m].enabled = false;

                if (skillIcons.icons[i].enabled)
                {
                    getStats.selectedSkill = getStats.hero.basicAttack;
                    skillIcons.icons[i].enabled = false;
                }
                else
                {
                    getStats.selectedSkill = getStats.hero.abilities[i];
                    skillIcons.icons[i].enabled = true;
                }
            }
        }
    }


    private void CreateRangeField()
    {
        targetsInRange.Remove(getStats);

        colliderForRange.radius = getStats.selectedSkill.range;
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        
        if (getStats.selectedSkill.skillType == SkillType.SingleTarget && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(maincamera.transform.position, maincamera.transform.forward, out hit, getStats.selectedSkill.range))
            {
                foreach (GetStats item in targetsInRange)
                {
                    if (hit.collider.gameObject.GetComponent<GetStats>() == item)
                    {
                        DamageHandler.DealDamage(getStats, hit.collider.gameObject.GetComponent<GetStats>());
                        break;
                    }
                }
            }
        }
    }

    private void Gravity()
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (!isWallRunning)
        {
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
        else if (isWallRunning && !Input.GetKey(jumpKey))
        {
            velocity.y = wallrunGravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
    }

    private void Sliding()
    {
        isSliding = true;
        characterController.height = Mathf.Lerp(originalHeight, reducedHeight, time);
        currentSpeed *= slideSpeed;
    }

    private void GetUp()
    {
        isSliding = false;
        characterController.height = Mathf.Lerp(reducedHeight, originalHeight, time);
        currentSpeed = speed;
        if (Input.GetKey(jumpKey))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void WallrunInput()
    {
        if (Input.GetKey(KeyCode.D) && isWallRight)
            StartWallrun();
        if (Input.GetKey(KeyCode.A) && isWallLeft)
            StartWallrun();
    }

    private void StartWallrun()
    {
        isWallRunning = true;
        if (isWallLeft && !Input.GetKeyDown(KeyCode.Space))
            characterController.Move(-transform.right * wallrunForce);
        else if (isWallRight && !Input.GetKeyDown(KeyCode.Space))
            characterController.Move(transform.right * wallrunForce);
    }

    private void StopWallrun()
    {
        isWallRunning = false;
    }

    private void CheckForWall()
    {
        isWallRight = Physics.Raycast(transform.position, transform.right, wallrunDistance + 0.8f, wall);
        Debug.DrawRay(transform.position, transform.right, Color.yellow);
        isWallLeft = Physics.Raycast(transform.position, -transform.right, wallrunDistance, wall);
        Debug.DrawRay(transform.position, -transform.right, Color.red);

        if (!isWallLeft && !isWallRight)
        {
            StopWallrun();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        bool found = false;

        foreach (GetStats item in targetsInRange)
        {
            if (item == other.gameObject.GetComponent<GetStats>())
                found = true;
        }

        if (!found)
            targetsInRange.Add(other.gameObject.GetComponent<GetStats>());
    }
}
/*  if (Mathf.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallRight)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
        if (Mathf.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;

        //Tilts camera back again
        if (wallRunCameraTilt > 0 && !isWallRight && !isWallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;
        if (wallRunCameraTilt < 0 && !isWallRight && !isWallLeft)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;*/