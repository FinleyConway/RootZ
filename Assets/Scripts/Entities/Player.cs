using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _mouseSensitivity = 3.5f;
    [SerializeField] private Transform _cameraHolder;
    private float _cameraPitch = 0.0f;
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleSimpleMove();
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = _cameraHolder.position;
        Camera.main.transform.rotation = _cameraHolder.rotation;
    }

    void HandleSimpleMove()
    {
        Vector3 targetDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        Vector3 direction = transform.TransformDirection(targetDir);
        _controller.SimpleMove(direction * _movementSpeed);
    }

    void HandleLook()
    {
        Vector2 delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        _cameraPitch -= delta.y * _mouseSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90.0f, 90.0f);

        _cameraHolder.localEulerAngles = Vector3.right * _cameraPitch;
        transform.Rotate(Vector3.up * delta.x * _mouseSensitivity);
    }
}
