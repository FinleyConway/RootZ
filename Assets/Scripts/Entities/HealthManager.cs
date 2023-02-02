using System;

public class HealthManager
{
    private int _health;
    private int _maxHealth;

    public event Action OnHealthChange;
    public event Action OnDeath;

    public HealthManager(int maxHealth)
    {
        _maxHealth = maxHealth;
        _health = _maxHealth;
    }

    public int GetHealth() => _health;

    public int GetMaxHealth() => _maxHealth;

    public float GetHealthPerc()
    {
        return (float)_health / _maxHealth;
    }

    public void Damage(int damage)
    {
        _health -= damage;

        if (_health < 0)
        {
            _health = 0;
            OnDeath?.Invoke();
        }
        OnHealthChange?.Invoke();
    }

    public void Kill()
    {
        Damage(_maxHealth);
    }

    public void Heal(int healAmount)
    {
        _health += healAmount;

        if (_health > _maxHealth) _health = _maxHealth;
        OnHealthChange?.Invoke();
    }
}
