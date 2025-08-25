using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerCrosshair : MonoBehaviour
{
    [SerializeField]
    private BaseShootingComponent _shootingComponent;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Color _disabledColor;

    [SerializeField]
    private Color _passiveColor;

    [SerializeField]
    private Color _activeColor;

    private Sequence _colorSequence;

    private void Awake()
    {
        if (!_spriteRenderer)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void LateUpdate()
    {
        transform.position = _shootingComponent.GetIntendedAimDirection();
    }

    public void OnTriggered()
    {
        GetNewSequence();
        _spriteRenderer.material.color = _passiveColor;
        _colorSequence.Append(_spriteRenderer.material.DOColor(_activeColor, 0.05f));
        _colorSequence.Append(_spriteRenderer.material.DOColor(_passiveColor, 0.05f));
        _colorSequence.OnComplete(OnClearColorTween);
    }

    public void OnReloading()
    {
        if (_shootingComponent.ReloadDuration <= 0)
        {
            return;
        }

        GetNewSequence();
        float disabledTransition = 0.1f;
        float reloadTransition = Mathf.Max(_shootingComponent.ReloadDuration - (disabledTransition * 2), 0.0f);

        _colorSequence.Append(_spriteRenderer.material.DOColor(_disabledColor, disabledTransition));
        _colorSequence.AppendInterval(reloadTransition);
        _colorSequence.Append(_spriteRenderer.material.DOColor(_passiveColor, disabledTransition));
        _colorSequence.OnComplete(OnClearColorTween);
    }

    public void OnHit()
    {

    }

    private void GetNewSequence()
    {
        _colorSequence?.Kill();
        _colorSequence = DOTween.Sequence();
    }

    private void OnClearColorTween()
    {
        _colorSequence = null;
    }
}
