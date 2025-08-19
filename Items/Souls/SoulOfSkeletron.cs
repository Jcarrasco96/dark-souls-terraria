using Terraria.ID;

namespace TerraSouls.Items.Souls;

// ReSharper disable once ClassNeverInstantiated.Global
public class SoulOfSkeletron : ConsumableSoul
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.rare = ItemRarityID.Red;
        
        Description = """
                      "The cursed guardian of the dungeon."
                      Bound in bone and shadow, this echo of a forgotten
                      sentinel aches with the burden of eternal
                      vigilance and endless curses.
                      """;
        Souls = 5000;
    }
}