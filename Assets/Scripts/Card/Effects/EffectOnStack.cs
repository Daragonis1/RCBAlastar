public class EffectOnStack
{
    public ICardEffect Effect { get; }
    public Card SourceCard { get; } // optionnel mais utile
    public PlayerController Controller { get; } // Qui contrôle l'effet

    public EffectOnStack(ICardEffect effect, Card sourceCard, PlayerController controller)
    {
        Effect = effect;
        SourceCard = sourceCard;
        Controller = controller;
    }

    public void Resolve()
    {
        Effect.Resolve();
    }
}
