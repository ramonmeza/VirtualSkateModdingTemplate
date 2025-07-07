using UnityEngine;

[RequireComponent(typeof(Camera))] // Works best with a camera
public class NoClipCamera : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float sprintSpeed = 20f;
    public float rotationSpeed = 2f;
    public float verticalSpeed = 5f;

    [Header("Key Bindings")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode upKey = KeyCode.Space;
    public KeyCode downKey = KeyCode.LeftControl;

    private float _currentSpeed;
    private float _yaw = 0f; // Horizontal rotation (Y-axis)
    private float _pitch = 0f; // Vertical rotation (X-axis)

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center
        Cursor.visible = false; // Hide cursor
        _currentSpeed = moveSpeed;
    }

    private void Update()
    {
        // Toggle cursor lock on Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ?
                CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }

        // Mouse look
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            _yaw += rotationSpeed * Input.GetAxis("Mouse X");
            _pitch -= rotationSpeed * Input.GetAxis("Mouse Y");
            _pitch = Mathf.Clamp(_pitch, -90f, 90f); // Prevent over-rotation
            transform.eulerAngles = new Vector3(_pitch, _yaw, 0f);
        }

        // Movement
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.S)) moveDirection -= transform.forward;
        if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;
        if (Input.GetKey(KeyCode.A)) moveDirection -= transform.right;

        // Vertical movement
        if (Input.GetKey(upKey)) moveDirection += Vector3.up;
        if (Input.GetKey(downKey)) moveDirection += Vector3.down;

        // Sprinting
        _currentSpeed = Input.GetKey(sprintKey) ? sprintSpeed : moveSpeed;

        // Apply movement
        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection.normalized * _currentSpeed * Time.deltaTime;
        }
    }
}