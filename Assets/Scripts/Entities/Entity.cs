using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamage
{
    [SerializeField] private int _maxHealth;

    private HealthManager _health;

    protected virtual void Awake()
    {
        _health = new HealthManager(_maxHealth);

        _health.OnDeath += Death;
    }

    public void Damage(int amount)
    {
        _health.Damage(amount);
    }

    protected virtual void Death() { }
}
