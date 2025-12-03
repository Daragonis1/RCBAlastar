using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    private readonly Stack<EffectOnStack> _effectStack = new Stack<EffectOnStack>();

    public event System.Action<EffectOnStack> OnEffectPushed;
    public event System.Action<EffectOnStack> OnEffectResolved;

    /// <summary>
    /// Ajoute un effet en haut de la pile.
    /// </summary>
    public void PushEffect(EffectOnStack effect)
    {
        _effectStack.Push(effect);
        OnEffectPushed?.Invoke(effect);
    }

    /// <summary>
    /// Résout le dernier effet ajouté.
    /// </summary>
    public void ResolveTop()
    {
        if (_effectStack.Count == 0)
            return;

        var effect = _effectStack.Pop();
        effect.Resolve();
        OnEffectResolved?.Invoke(effect);
    }

    /// <summary>
    /// Résout tous les effets dans l'ordre LIFO.
    /// </summary>
    public void ResolveAll()
    {
        while (_effectStack.Count > 0)
        {
            ResolveTop();
        }
    }

    public bool HasPendingEffects => _effectStack.Count > 0;
}
