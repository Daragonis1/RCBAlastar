using System.Collections.Generic;

public class Deck : Location
{
    public Deck(PlayerController controller)
    {
        Initialize(LocationType.Deck, controller);
    }
    public void Shuffle()
    {
        System.Random rng = new();
        int n = cards.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (cards[n], cards[k]) = (cards[k], cards[n]);
        }
    }
    
    public List<Card> Draw(int draw = 1)
    {
        var drawn = new List<Card>();

        if (draw <= 0)
            return drawn;

        for (int n = 0; n < draw; n++)
        {
            if (cards.Count == 0)
                break;

            Card topCard = cards[0];
            RemoveCard(topCard);
            drawn.Add(topCard);
        }

        return drawn;
    }
}
