using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    private List<SpriteRenderer> _spriteRenderers;

    private Dictionary<string, int> _keyToIndex = new Dictionary<string, int>();


    private void Awake()
    {
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
        if (_spriteRenderers.Count <= 0) 
        { 
            return; 
        }

        DOTween.Kill(this);
        Sequence colorTransition = DOTween.Sequence(this);
        foreach (var renderer in _spriteRenderers)
        {
            colorTransition.Insert(0, renderer.DOColor(transition.TargetColor, transition.FadeInDuration));
            colorTransition.Insert(transition.FadeInDuration, renderer.DOColor(_returnColor, transition.FadeOutDuration));
        }
    }
}

