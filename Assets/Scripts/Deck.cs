using System.Collections.Generic;

public class Deck : Location
{
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
    
    public List<Card> Draw(int i = 1)
    {
        var drawn = new List<Card>();

        if (i <= 0)
            return drawn;

        for (int n = 0; n < i; n++)
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
