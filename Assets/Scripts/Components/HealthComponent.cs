using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth = 100;
    private int _currentHealth;
    public event EventHandler<HealthChangedArgument> OnHealthChanged;

    protected virtual void Awake()
    {
        ResetHealth();
    }

    public virtual void TakeDamage(HealthChangeData damage)
    {
        _currentHealth -= damage.Value;
        NotifyOfHealthChange(damage, HealthChangedArgument.HealthChangeType.Damage);
        if (_currentHealth <= 0)
        {
            Died();
        }
    }

    public virtual void RestoreHealth(HealthChangeData heal)
    {
        _currentHealth = Mathf.Min(_currentHealth + heal.Value, _maxHealth);
        NotifyOfHealthChange(heal, HealthChangedArgument.HealthChangeType.Heal);
    }

    public virtual void Died()
    {
        gameObject.SetActive(false);
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
    }

    protected void NotifyOfHealthChange(HealthChangeData changeData, HealthChangedArgument.HealthChangeType type)
    {
        OnHealthChanged?.Invoke(this, new HealthChangedArgument(_currentHealth, changeData, type));
    }
}

public struct HealthChangeData
{
    public int Value;
    public GameObject Source;
}

public class HealthChangedArgument : EventArgs
{
    public enum HealthChangeType
    {
        Damage,
        Heal
    }

    public readonly int CurrentHealth;
    public readonly HealthChangeData ChangeData;
    public readonly HealthChangeType ChangeType;

    public HealthChangedArgument(int currentHealth, HealthChangeData changeData, HealthChangeType type)
    {
        CurrentHealth = currentHealth;
        ChangeData = changeData;
        ChangeType = type;
    }
}