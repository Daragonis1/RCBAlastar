
using UnityEngine;

[CreateAssetMenu(menuName = "Card/New Effect Data")]
public class EffectData : ScriptableObject
{
    public string effectName;
    public string description;
    public Effect CreateEffect(Card card)
    {
        return new Effect(card, this);
    }
}