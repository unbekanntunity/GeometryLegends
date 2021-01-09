using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Variables

    [Header("Controls")]
    [SerializeField]
    private KeyCode JumpKey_Prim;

    [SerializeField]
    private KeyCode JumpKey_Sec;

    [SerializeField]
    private KeyCode SprintKey_Prim;

    [SerializeField]
    private KeyCode SprintKey_Sec;

    [Header("Requiered")]
    public Camera normalCam;
    public Transform groundDetector;
    public LayerMask ground;

    [Header("Settings")]
    public float Speed;
    public float sprintModifier;
    public float sprintFOVModifier = 1.5f;
    public float jumpforce;

    private float baseFOV;
    private Rigidbody rig;

    public GameObject groundA;

    #endregion

    private void Start()
    {
        baseFOV = normalCam.fieldOfView;
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Axes
        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");

        //Control
        bool sprint = Input.GetKey(SprintKey_Prim) || Input.GetKey(SprintKey_Sec);
        bool jump = Input.GetKey(JumpKey_Prim) || Input.GetKey(JumpKey_Sec);

        //States
        bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.5f, ground);
        bool isJumping = jump && isGrounded;
        bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;

        //Jumping
        if (isJumping)
        {
            rig.AddForce(Vector3.up * jumpforce);
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        {
            //Axes
            float t_hmove = Input.GetAxisRaw("Horizontal");

            float t_vmove = Input.GetAxisRaw("Vertical");

            //Control
            bool sprint = Input.GetKey(SprintKey_Prim) || Input.GetKey(SprintKey_Sec);
            bool jump = Input.GetKey(JumpKey_Prim) || Input.GetKey(JumpKey_Sec);

            //States
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.5f, ground);
            bool isJumping = jump && isGrounded;
            bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;

            //Movement
            Vector3 t_direction = new Vector3(t_hmove, 0, t_vmove);
            t_direction.Normalize();

            float t_adjustedSpeed = Speed;
            if (isSprinting) t_adjustedSpeed *= sprintModifier;

            Vector3 t_targetVelocity = transform.TransformDirection(t_direction) * t_adjustedSpeed * Time.deltaTime;
            t_targetVelocity.y = rig.velocity.y;
            rig.velocity = t_targetVelocity;

            //FieldOFView
            if (isSprinting) { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f); }
            else { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f); }

        }
    }
}


