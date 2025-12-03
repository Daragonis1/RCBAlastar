public class Battlefield : Location
{
    int BattlefieldId;
    public Battlefield(PlayerController controller, int battlefieldId)
    {
        Initialize(LocationType.Battlefield, controller);
        BattlefieldId = battlefieldId;
    }
}
