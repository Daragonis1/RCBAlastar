using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Unit,
    Spell,
    Hero
}

[CreateAssetMenu(menuName = "Card/New Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    public CardType type;
    public int cost;
    public int attack;
    public int health;
    public string description;
    public List<EffectData> effect;
}
