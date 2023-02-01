using UnityEngine;

public class InteractController : MonoBehaviour
{
    [Header("Conditions")]
    [SerializeField] private LayerMask _grabbables;

    [Header("Interacting Settings")]
    [SerializeField] private float _interactDistance;

    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    // interact with scene objects
    private void Interact()
    {
        // if raycast hits
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out RaycastHit hit, _interactDistance, _grabbables))
        {
            // if hit is interactable
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                // interactable with object
                interactable.Interact(gameObject);
            }
        }
    }
}