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

    public virtual void Initialize(LocationType type, PlayerController controller)
    {
        Type = type;
        Controller = controller;
    }

    public virtual void AddCard(Card card)
    {
        cards.Add(card);
        card.CurrentZone = this;
    }

    public virtual void RemoveCard(Card card)
    {
        cards.Remove(card);
    }

    public IReadOnlyList<Card> Cards => cards;
}
