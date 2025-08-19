using Terraria.ID;

namespace TerraSouls.Items.Souls;

public class SoulOfTheDestroyer : ConsumableSoul
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.rare = ItemRarityID.LightRed;
        
        Description = """
                      "The endless machine of annihilation."
                      Cold metal and soulless gears grind within this
                      fragment, echoing the relentless march of death
                      without mercy or rest.
                      """;
        Souls = 5000;
    }
}