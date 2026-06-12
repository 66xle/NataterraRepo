using TMPro;
using UnityEngine;

public class HexEditorCamera : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float fastMoveMultiplier = 2f;
    [SerializeField] private float panSpeed = 20f;

    [Header("Look")]
    [SerializeField] private float lookSensitivity = 2f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 20f;

    public TMP_InputField inputField;

    private float yaw;
    private float pitch;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
        HandleZoom();
        HandlePan();
    }

    private void HandleMovement()
    {
        if (inputField.isFocused) return;

        float currentSpeed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftControl))
            currentSpeed *= fastMoveMultiplier;

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            moveDirection += forward;

        if (Input.GetKey(KeyCode.S))
            moveDirection -= forward;

        if (Input.GetKey(KeyCode.A))
            moveDirection -= right;

        if (Input.GetKey(KeyCode.D))
            moveDirection += right;

        if (Input.GetKey(KeyCode.Space))
            moveDirection += Vector3.up;

        if (Input.GetKey(KeyCode.LeftShift))
            moveDirection += Vector3.down;

        transform.position += moveDirection.normalized * currentSpeed * Time.deltaTime;
    }

    private void HandleLook()
    {
        if (!Input.GetMouseButton(1))
            return;

        yaw += Input.GetAxis("Mouse X") * lookSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * lookSensitivity;

        pitch = Mathf.Clamp(pitch, -89f, 89f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) < 0.01f)
            return;

        transform.position += transform.forward * scroll * zoomSpeed;
    }

    private void HandlePan()
    {
        if (!Input.GetMouseButton(2))
            return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 right = transform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 movement =
            (-right * mouseX + -forward * mouseY) *
            panSpeed *
            Time.deltaTime;

        transform.position += movement;
    }
}
