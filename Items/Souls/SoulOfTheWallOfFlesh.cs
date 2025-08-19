namespace TerraSouls.Items.Souls;

public class SoulOfTheWallOfFlesh : ConsumableSoul
{
    public override void SetDefaults()
    {
        Description = """
                      "The seal of the underworld."
                      Burning with the anguished cries of the damned, it
                      stands as a barrier between mortal light and
                      infernal darkness.
                      """;
        Souls = 5000;
    }
}