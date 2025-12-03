using System.Collections.Generic;

public class PlayerController
{
    public string Name { get; private set; }
    public int Mana { get; set; }
    public int Points { get; set; }

    public Deck Deck { get; private set; }
    public Hand Hand { get; private set; }
    public Graveyard Graveyard { get; private set; }
    public Exile Exile { get; private set; }
    public Base Base { get; private set; }
    public Battlefield[] Battlefields { get; private set; }
    public Show Show { get; private set; }

    public PlayerController(string name)
    {
        Name = name;
        Points = 0;
    }

    public void InitializeLocations()
    {
        Deck = new Deck(this);
        Hand = new Hand(this);
        Graveyard = new Graveyard(this);
        Exile = new Exile(this);
        Base = new Base(this);
        Show = new Show(this);

        // Si tu as plusieurs champs de batailles :
        Battlefields = new Battlefield[3];
        for (int i = 0; i < 3; i++)
            Battlefields[i] = new Battlefield(this, i);
    }

    public List<Card> DrawCard(int draw = 1)
    {
        return Deck.Draw(draw);
    }

    public void UntapAll()
    {
        // Plus tard : démarrer toutes les unités
    }
}
