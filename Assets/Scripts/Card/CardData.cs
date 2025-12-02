using UnityEngine;

public enum CardType
{
    Unit,
    Spell,
    Hero
}

[CreateAssetMenu(menuName = "Game/Card", fileName = "Card_")]
public class CardData : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string displayName;
    [SerializeField] private int cost;
    [SerializeField] private CardType type;

    // Optionnel pour les unités
    [Header("Unit Stats")]
    public int hp;
    public int attack;

    public string Id => id;
    public string DisplayName => displayName;
    public int Cost => cost;
    public CardType Type => type;
}
