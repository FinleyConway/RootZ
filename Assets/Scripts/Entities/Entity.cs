using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamage
{
    [SerializeField] private int _maxHealth;

    public HealthManager Health {get; private set;}

    protected virtual void Awake()
    {
        Health = new HealthManager(_maxHealth);

        Health.OnDeath += Death;
    }

    public void Damage(int amount)
    {
        Health.Damage(amount);
    }

    protected virtual void Death() { }
}
