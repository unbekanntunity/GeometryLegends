using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject head;
    [SerializeField]
    private KeyCode mouseFocusOn;
    [SerializeField]
    private KeyCode mouseFocusOff;
    [SerializeField]
    private float mouseSensitivity = 100f;

    private Camera camera;
    private float xRotation = 0f;

    private void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
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
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f,0f);
        head.transform.Rotate(Vector3.up * mouseX);
    }
}
