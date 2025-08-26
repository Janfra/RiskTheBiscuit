using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorAnimationsComponent : MonoBehaviour
{
    [Serializable]
    public struct ColorTransition
    {
        public string Key => _key.ToLower();

        [SerializeField]
        private string _key;
        public Color TargetColor;
        public float FadeInDuration;
        public float FadeOutDuration;
    }

    [SerializeField]
    private Color _returnColor;
    [SerializeField]
    private ColorTransition[] _transitions;
    private Dictionary<string, int> _keyToIndex = new Dictionary<string, int>();

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        for (int i = 0; i < _transitions.Length; i++)
        {
            ColorTransition transition = _transitions[i];
            if (_keyToIndex.ContainsKey(transition.Key))
            {
                Debug.LogWarning($"There are two transitions with the same key in the {nameof(ColorAnimationsComponent)} component inside {name}");
            }
            else
            {
                _keyToIndex.Add(transition.Key, i);
            }
        }
    }

    public void PlayTransitionAtIndex(int index)
    {
        if (index < 0 || index >= _transitions.Length)
        {
            return;
        }

        var transition = _transitions[index];
        PlayTransition(transition);
    }

    public void PlayTransitionWithKey(string key)
    {
        if (_keyToIndex.TryGetValue(key.ToLower(), out int index))
        {
            PlayTransition(_transitions[index]);
        }
    }

    private void PlayTransition(ColorTransition transition)
    {
        DOTween.Kill(this);
        Sequence colorTransition = DOTween.Sequence(this);
        colorTransition.Append(_spriteRenderer.DOColor(transition.TargetColor, transition.FadeInDuration));
        colorTransition.Append(_spriteRenderer.DOColor(_returnColor, transition.FadeOutDuration));
    }
}

