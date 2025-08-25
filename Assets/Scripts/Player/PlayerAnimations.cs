using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimations : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void OnDamaged()
    {
        DOTween.Kill(this);
        Sequence sequence = DOTween.Sequence(this);
        sequence.Append(_spriteRenderer.DOColor(Color.red, 0.1f));
        sequence.Append(_spriteRenderer.DOColor(Color.white, 0.1f));
    }

    public void OnHealed()
    {
        DOTween.Kill(this);
        Sequence sequence = DOTween.Sequence(this);
        sequence.Append(_spriteRenderer.DOColor(Color.green, 0.1f));
        sequence.Append(_spriteRenderer.DOColor(Color.white, 0.1f));
    }
}
