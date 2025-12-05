using System.Collections.Generic;

public class Card
{
    private CardData Data;
    public string Name { get; private set; }
    public string Description { get; private set; }
    public CardType Type { get; private set; }
    public Location Position { get; private set; }
    public int CurrentAttack { get; private set; }
    public int MaxAttack { get; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public int Cost { get; private set; }
    public bool IsDead => CurrentHealth <= 0;
    public List<Effect> Effects { get; private set; }

    public Card(CardData data)
    {
        Data = data;
        ExtractStatsFromData();
    }
    private void ExtractStatsFromData()
    {
        Name = Data.cardName;
        Description = Data.description;
        Type = Data.type;
        MaxAttack = Data.attack;
        CurrentAttack = MaxAttack;
        MaxHealth = Data.health;
        CurrentHealth = MaxHealth;
        Cost = Data.cost;
        Effects = new List<Effect>();
        foreach (var effectData in Data.effect)
        {
            Effects.Add(effectData.CreateEffect(this));
        }
    }

    // Méthode utilisée par Location pour synchroniser le modèle
    internal void SetPosition(Location loc)
    {
        Position = loc;
    }
    public void Move(Location newLocation)
    {
        // defensive: s'il y a une position actuelle, on la notifie
        if (Position != null)
        {
            Position.RemoveCard(this);
        }
        newLocation.AddCard(this);
    }

    public void Damage(int amount)
    {
        CurrentHealth -= amount;
    }
}