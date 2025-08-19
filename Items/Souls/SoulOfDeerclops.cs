namespace TerraSouls.Items.Souls;

public class SoulOfDeerclops : ConsumableSoul
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Description = """
                      "The harbinger of frost and famine."
                      A frozen eye gazes from within this soul, chilling
                      the blood of those who dare to remember its icy
                      wrath.
                      """;
        Souls = 5000;
    }
}