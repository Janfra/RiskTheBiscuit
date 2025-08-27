using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerHealthChangeComponent : MonoBehaviour
{
    [SerializeField]
    private HealthChangedArgument.HealthChangeType changeType = HealthChangedArgument.HealthChangeType.Damage;
    [SerializeField]
    private int _modificationAmount = 1;

    [Header("Events")]
    [SerializeField]
    private UnityEvent _unityOnModification;
    [SerializeField]
    private UnityEvent _unityOnFailedModification;

    private List<HealthComponent> healthComponents = new List<HealthComponent>();
    private Action<HealthComponent> _onApplyModification;


    private void Awake()
    {
        switch (changeType)
        {
            case HealthChangedArgument.HealthChangeType.Damage:
                _onApplyModification = DealDamage;
                break;
            case HealthChangedArgument.HealthChangeType.Heal:
                _onApplyModification = RestoreHealth;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HealthComponent healthComponent))
        {
            if (healthComponents.Contains(healthComponent))
            {
                Debug.LogWarning($"Health component {healthComponent} is already in the list of components being damaged by {name}.");
            }
            else
            {
                healthComponents.Add(healthComponent);
            }

            _onApplyModification(healthComponent);
            _unityOnModification?.Invoke();
            return;
        }

        _unityOnFailedModification?.Invoke();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (var healthComponent in healthComponents)
        {
            if (healthComponent)
            {
                _onApplyModification(healthComponent);
                _unityOnModification?.Invoke();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (healthComponents.Count > 0)
        {
            healthComponents.Remove(collision.GetComponent<HealthComponent>());
        }
    }

    private void DealDamage(HealthComponent healthComponent)
    {
        healthComponent.TakeDamage(new HealthChangeData(_modificationAmount, gameObject));
    }

    private void RestoreHealth(HealthComponent healthComponent)
    {
        healthComponent.RestoreHealth(new HealthChangeData(_modificationAmount, gameObject));
    }
}
