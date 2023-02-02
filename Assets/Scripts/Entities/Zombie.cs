using UnityEngine;
using UnityEngine.AI;

public class Zombie : Entity
{
    [SerializeField] private Transform _hitCheckPosition;
    [SerializeField] private float _hitDistance;
    [SerializeField] private float _hitDamage;
    [SerializeField] private float _hitTime;
    private float _lastHitTime;
    private Transform _playerPosition;
    private bool _isDead;

    private NavMeshAgent _agent;
    private Animator _anim;

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponentInChildren<Animator>();

        _playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (_isDead) return;

        IsMoving();

        if (Physics.Raycast(_hitCheckPosition.position, _hitCheckPosition.forward, out RaycastHit hit, _hitDistance))
        {
            if (Time.time > _hitTime + _lastHitTime)
            {
                _agent.SetDestination(transform.position);
                _anim.SetTrigger("Hit");
                _lastHitTime = Time.time;
            }
        }
        else
        {
            _agent.SetDestination(_playerPosition.position);
        }
    }

    private void IsMoving()
    {
        float velocity = _agent.velocity.magnitude;
        if (velocity > 0.1f)
        {
            _anim.SetBool("Run", true);
        }
        else
        {
            _anim.SetBool("Run", false);
        }
    }

    protected override void Death()
    {
        _agent.SetDestination(transform.position);
        _isDead = true;
        _anim.SetTrigger("Dead");
        Destroy(gameObject, 2.5f);
    }
}
