using System.Collections.Generic;

public enum ZoneType
{
    Deck,
    Hand,
    Base,
    Battlefield,
    Graveyard,
    Stack,
    NonGame,
    Exile
}

public class Location
{
    public ZoneType ZoneType { get; protected set; }
    public PlayerController Controller { get; protected set; }

    protected readonly List<Card> cards = new();

    public virtual void Initialize(ZoneType type, PlayerController controller)
    {
        ZoneType = type;
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
