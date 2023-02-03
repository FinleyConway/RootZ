using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _mouseSensitivity = 3.5f;
    [SerializeField] private Transform _cameraHolder;
    private float _cameraPitch = 0.0f;
    private CharacterController _controller;

    protected override void Awake()
    {
        base.Awake();
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
        Camera.main.transform.SetPositionAndRotation(_cameraHolder.position, _cameraHolder.rotation);
    }

    private void HandleSimpleMove()
    {
        Vector3 targetDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        Vector3 direction = transform.TransformDirection(targetDir);
        _controller.SimpleMove(direction * _movementSpeed);
    }

    private void HandleLook()
    {
        if (PauseMenu.isPaused) return;
        
        Vector2 delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        _cameraPitch -= delta.y * _mouseSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90.0f, 90.0f);

        _cameraHolder.localEulerAngles = Vector3.right * _cameraPitch;
        transform.Rotate(Vector3.up * delta.x * _mouseSensitivity);
    }

    protected override void Death()
    {
        SceneManager. LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
