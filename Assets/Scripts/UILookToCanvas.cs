using UnityEngine;

public class UILookToCanvas : MonoBehaviour
{
    public Camera mainCam;

    private void FixedUpdate()
    {
        transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);
    }
}
