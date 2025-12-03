using System.Linq;
using UnityEngine;

public class CardDatabaseLoader : MonoBehaviour
{
    public CardDatabase database;

    void Awake()
    {
        var loadedCards = Resources.LoadAll<CardData>("Cards").ToList();

        // Add new CardData
        foreach (var card in loadedCards)
        {
            if (!database.cards.Contains(card))
            {
                database.cards.Add(card);
            }
        }

        // Remove CardData that no longer exists
        database.cards.RemoveAll(card => !loadedCards.Contains(card));
    }
}
