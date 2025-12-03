using System.Linq;
using UnityEngine;

public class CardDatabaseLoader : MonoBehaviour
{
    public CardDatabase database;

    void Awake()
    {
        Debug.Log("Chargement de la base de données des cartes…");
        var loadedCards = Resources.LoadAll<CardData>("Cards").ToList();

        // Add new CardData
        foreach (var card in loadedCards)
        {
            Debug.Log("Chargée : " + card.cardName);
            if (!database.cards.Contains(card))
            {
                database.cards.Add(card);
            }
        }

        // Remove CardData that no longer exists
        database.cards.RemoveAll(card => !loadedCards.Contains(card));
    }
}
