using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : HealthComponent
{
    [Header("Events")]
    [SerializeField]
    private UnityEvent _unityOnPlayerHealed;
    [SerializeField]
    private UnityEvent _unityOnPlayerDamaged;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        OnHealthChanged += InvokeUnityEvent;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnHealthChanged -= InvokeUnityEvent;
    }

    protected void InvokeUnityEvent(object sender, HealthChangedArgument healthChangedArgument)
    {
        switch (healthChangedArgument.ChangeType)
        {
            case HealthChangedArgument.HealthChangeType.Damage:
                _unityOnPlayerDamaged?.Invoke();
                break;
            case HealthChangedArgument.HealthChangeType.Heal:
                _unityOnPlayerHealed?.Invoke();
                break;
        }
    }
}
