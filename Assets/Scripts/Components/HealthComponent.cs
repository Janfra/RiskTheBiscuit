using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public struct HealthChangeData
{
    public int Value;
    public GameObject Source;

    public HealthChangeData(int value, GameObject source = null)
    {
        Value = value;
        Source = source;
    }
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

public class HealthComponent : MonoBehaviour
{
    public const float INVULNERABILITY_TIME = 0.3f;

    [Header("Configuration")]
    [SerializeField]
    private int _maxHealth = 100;

    [Header("Events")]
    [SerializeField]
    protected UnityEvent _UnityOnHealed;
    [SerializeField]
    protected UnityEvent _UnityOnDamaged;

    public event EventHandler<HealthChangedArgument> OnHealthChanged;
    public bool IsInvulnerable => !_canTakeDamage;
    public bool IsHealable => _canHeal;

    private bool _canTakeDamage = true;
    private bool _canHeal = true;
    private int _currentHealth;

    protected virtual void Awake()
    {
        ResetHealth();
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void TakeDamage(HealthChangeData damage)
    {
        if (IsInvulnerable)
        {
            return;
        }

        _currentHealth -= damage.Value;
        NotifyOfHealthChange(damage, HealthChangedArgument.HealthChangeType.Damage);
        _UnityOnDamaged?.Invoke();
        if (_currentHealth <= 0)
        {
            Died();
        }
        else
        {
            StartCoroutine(TriggerInvulnerabilityFrames());
        }
    }

    public virtual void RestoreHealth(HealthChangeData heal)
    {
        if (!IsHealable)
        {
            return;
        }

        _currentHealth = Mathf.Min(_currentHealth + heal.Value, _maxHealth);
        NotifyOfHealthChange(heal, HealthChangedArgument.HealthChangeType.Heal);
        _UnityOnHealed?.Invoke();
        StartCoroutine(TriggerHealInvulnerabilityFrames());
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

    protected IEnumerator TriggerInvulnerabilityFrames()
    {
        _canTakeDamage = false;
        yield return new WaitForSeconds(INVULNERABILITY_TIME);
        _canTakeDamage = true;
    }

    protected IEnumerator TriggerHealInvulnerabilityFrames()
    {
        _canHeal = false;
        yield return new WaitForSeconds(INVULNERABILITY_TIME);
        _canHeal = true;
    }
}
