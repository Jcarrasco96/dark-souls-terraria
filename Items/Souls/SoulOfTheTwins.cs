namespace TerraSouls.Items.Souls;

public class SoulOfTheTwins : ConsumableSoul
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Description = """
                      "The twin gazes of despair."
                      Bound in eternal opposition, these eyes blaze with
                      wrath and venom—two halves of a relentless
                      nightmare.
                      """;
        Souls = 5000;
    }
}