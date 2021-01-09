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
    private float jumpHeight = 3f;

    [SerializeField]
    private float slideDurtion = 3f;
    [SerializeField]
    private float time;

    [SerializeField]
    private GameObject groundCheck;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float groundDistance = 0.4f;

    [SerializeField]
    private float reducedHeight;

    private float originalHeight;
    private bool isGrounded = false;
    public bool isSliding = false;
    private Vector3 velocity;
    private CharacterController characterController;
    private float currentSpeed;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        originalHeight = characterController.height;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundDistance, groundMask);

        currentSpeed = speed;

        if (Input.GetKey(slideKey) && Input.GetKey(KeyCode.W) && !isSliding)
        {
            isSliding = true;
            Sliding();
            Invoke("GetUp", slideDurtion);
        }
        else if (Input.GetKeyUp(slideKey))
        {
            isSliding = false;
            GetUp();
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void Sliding()
    {
        characterController.height = Mathf.Lerp(originalHeight, reducedHeight, time);
        currentSpeed *= slideSpeed;
    }

    private void GetUp()
    {
        characterController.height = Mathf.Lerp(reducedHeight, originalHeight, time);
        currentSpeed = speed;
    }
}