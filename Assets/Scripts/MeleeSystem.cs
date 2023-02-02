using UnityEngine;

public class MeleeSystem : MonoBehaviour
{
    [Header("Melee")]
    [SerializeField] private int _damageDealingAmount;
    [SerializeField] private float _timeEachSwing;
    [SerializeField] private float _hitDistance;
    [SerializeField] private LayerMask _damagables;
    private float _lastSwingTime;
    private Camera _cam;

    [Header("Sound")]
    [SerializeField] private SimpleAudioEvent _swingSound;
    private AudioSource _source;

    [Header("Animation")]
    [SerializeField] private Animator _axeAnimator;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();

        _cam = Camera.main;

        _axeAnimator.speed = _timeEachSwing * 10;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (Time.time > _timeEachSwing + _lastSwingTime)
        {
            _axeAnimator.SetTrigger("Swing");

            Transform cam = _cam.transform;
            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, _hitDistance, _damagables))
            {
                if (hit.transform.TryGetComponent(out IDamage damage))
                {
                    damage.Damage(_damageDealingAmount);
                }
            }
            _lastSwingTime = Time.time;
        }
    }
}
