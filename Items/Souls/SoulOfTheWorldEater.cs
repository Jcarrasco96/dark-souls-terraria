using Terraria.ID;

namespace TerraSouls.Items.Souls;

public class SoulOfTheWorldEater : ConsumableSoul
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.rare = ItemRarityID.Blue;
        
        Description = """
                      "The devourer from the depths."
                      Coiled within this fragment dwells an endless
                      hunger, an echo of the abyss’s cold maw, devouring
                      light and hope alike.
                      """;
        Souls = 5000;
    }
}