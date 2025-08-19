namespace TerraSouls.Items.Souls;

public class SoulOfTheBrainOfCthulhu : ConsumableSoul
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Description = """
                      "The mind behind the madness."
                      Pulsing with whispers older than time, this
                      fragment murmurs the secrets of crimson insanity,
                      tainting the soul it touches.
                      """;
        Souls = 5000;
    }
}