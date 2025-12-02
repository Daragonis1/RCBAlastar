public class Card
{
    public string InstanceId;
    public CardData Data;
    public Location CurrentZone;

    public Card(CardData data)
    {
        Data = data;
        InstanceId = System.Guid.NewGuid().ToString();
    }
}