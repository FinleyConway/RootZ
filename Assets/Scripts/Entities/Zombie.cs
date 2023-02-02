using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private Vector3 _dest;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Init(Vector3 dest)
    {
        _dest = dest;
    }

    private void Update()
    {
        _agent.SetDestination(_dest);
    }
}
