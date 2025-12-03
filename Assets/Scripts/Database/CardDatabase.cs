using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Card Database")]
public class CardDatabase : ScriptableObject
{
    public List<CardData> cards;

    public CardData GetCardByName(string name)
    {
        return cards.Find(c => c.cardName == name);
    }

    public CardData GetRandomCard()
    {
        return cards[Random.Range(0, cards.Count)];
    }
}
