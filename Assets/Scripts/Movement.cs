using UnityEngine;

public class Movement : MonoBehaviour
{
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

    public bool isWallRight;
    public bool isWallLeft;

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
    private Camera camera;

    private float currentSpeed;
    private float originalHeight;
    private bool isGrounded = false;
    private bool isSliding = false;
    public bool isWallRunning = false;

    private Vector3 velocity;
    private CharacterController characterController;

    private void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        originalHeight = characterController.height;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MyInput();
        Look();
        CheckForWall();
        WallrunInput();
        Gravity();

    }

    private void MyInput()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundDistance, groundMask);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
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
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

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
        else if (isWallRunning)
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

    private void LastJump()
    {
        if (Input.GetKey(jumpKey))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void CheckForWall()
    {
        isWallRight = Physics.Raycast(transform.position, transform.right, wallrunDistance + 0.8f, wall);
        Debug.DrawRay(transform.position, transform.right, Color.yellow);
        isWallLeft = Physics.Raycast(transform.position, -transform.right, wallrunDistance, wall);
        Debug.DrawRay(transform.position, -transform.right, Color.red);

        if (!isWallLeft && !isWallRight)
        {
            if (isWallRunning)
                LastJump();
            StopWallrun();
        }
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