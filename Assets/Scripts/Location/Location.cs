using System;
using System.Collections.Generic;

public enum LocationType
{
    Deck,
    Hand,
    Base,
    Battlefield,
    Graveyard,
    Stack,
    Show,
    Exile
}

public class Location
{
    public LocationType Type { get; protected set; }
    public PlayerController Controller { get; protected set; }

    protected readonly List<Card> cards = new();

    // Events pour la vue
    public event Action<Card> OnCardAdded;
    public event Action<Card> OnCardRemoved;
    public event Action OnCleared;

    public virtual void Initialize(LocationType type, PlayerController controller)
    {
        Type = type;
        Controller = controller;
    }

    public virtual void AddCard(Card card)
    {
        // ajoute au modèle
        cards.Add(card);
        // met à jour la référence de position côté modèle (internal setter)
        card.SetPosition(this);
        // notifie la vue
        OnCardAdded?.Invoke(card);
    }

    public virtual void RemoveCard(Card card)
    {
        if (cards.Remove(card))
        {
            OnCardRemoved?.Invoke(card);
        }
    }

    public virtual void Clear()
    {
        cards.Clear();
        OnCleared?.Invoke();
    }

    public IReadOnlyList<Card> Cards => cards;

    // Renvoie l'index d'une carte dans la liste (ou -1 si absente)
    public int IndexOf(Card card)
    {
        return cards.IndexOf(card);
    }
}
