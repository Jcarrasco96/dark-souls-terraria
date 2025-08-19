using Terraria.ID;

namespace TerraSouls.Items.Souls;

public class SoulOfTheKingSlime : ConsumableSoul
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.rare = ItemRarityID.Blue;

        Description = """
                      "The lord of all things slimy."
                      This viscous remnant weeps with the sorrow of a
                      kingdom dissolved in gelatinous twilight. It
                      lingers, echoing the lost reign of the jiggling
                      monarch.
                      """;
        Souls = 5000;
    }
}