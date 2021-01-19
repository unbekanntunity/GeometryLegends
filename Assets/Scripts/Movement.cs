using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;

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
    private float slideLerpTime;

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

    [SerializeField]
    private float aimFOV;
    [SerializeField]
    private float aimFOVLerpTime;
    [SerializeField]
    private float aimSpeed;

    [SerializeField]
    private GameObject weaponPos;
    public GameObject WeaponPos { get { return weaponPos; } private set { weaponPos = value; } }

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

    private float normalFOV = 60f;
    private float xRotation = 0f;
    public float currentSpeed;
    private float originalHeight;
    private bool isWallRight = false;
    private bool isWallLeft = false;
    public bool isGrounded = false;
    public bool isSliding = false;
    public bool isWallRunning = false;
    public bool isAiming = false;

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
        characterController = GetComponent<CharacterController>();
        colliderForRange = GetComponent<SphereCollider>();
        getStats = GetComponent<GetStats>();
        getStats.selectedSkill = getStats.hero.basicAttack;
        originalHeight = characterController.height;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        normalFOV = playerCamera.fieldOfView;
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

        if (Input.GetMouseButton(1) && !isAiming)
            StartAim();
        else if (Input.GetMouseButtonUp(1))
            EndAim();

        if (!isWallRunning && !isAiming && !isSliding)
            currentSpeed = speed;

        if (Input.GetKeyDown(jumpKey) && (isGrounded || isWallRunning))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }


    private void StartAim()
    {
        print("A");
        isAiming = true;
        currentSpeed *= aimSpeed;
        playerCamera.fieldOfView = Mathf.Lerp(normalFOV, aimFOV, aimFOVLerpTime);
    }

    private void EndAim()
    {
        print("AA");
        playerCamera.fieldOfView = Mathf.Lerp(aimFOV, normalFOV, aimFOVLerpTime);
        currentSpeed = speed;
        isAiming = false;
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
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        weaponPos.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
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

        if (Input.GetMouseButtonDown(0))
        {
            if (getStats.selectedSkill.skillType == SkillType.SingleTarget && getStats.selectedSkill.needTarget)
            {

                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, getStats.selectedSkill.range))
                {
                    foreach (GetStats item in targetsInRange)
                    {
                        if (hit.collider.gameObject.GetComponent<GetStats>() == item)
                        {
                            getStats.selectedSkill.CastSkill(gameObject, hit.collider.gameObject);
                            break;
                        }
                    }
                }

            }
            else
            {
                RaycastHit hit;
                if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1000f))
                    getStats.selectedSkill.CastSkill(gameObject, hit.point);
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
        characterController.height = Mathf.Lerp(originalHeight, reducedHeight, slideLerpTime);
        currentSpeed *= slideSpeed;
    }

    private void GetUp()
    {
        isSliding = false;
        characterController.height = Mathf.Lerp(reducedHeight, originalHeight, slideLerpTime);
        if (Input.GetKey(jumpKey))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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

        if (isWallRunning)
            currentSpeed = maxwallrunSpeed;

        if (!isWallLeft && !isWallRight)
            StopWallrun();
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