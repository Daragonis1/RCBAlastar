public class Effect
{
    public Card SourceCard;
    public string Description;

    public Effect(Card card, EffectData effet)
    {
        SourceCard = card;
        Description = effet.description;
    }

    public void Resolve()
    {
        
    }
}
