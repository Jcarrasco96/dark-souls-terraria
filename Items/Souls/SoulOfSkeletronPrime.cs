using Terraria.ID;

namespace TerraSouls.Items.Souls;

public class SoulOfSkeletronPrime : ConsumableSoul
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.rare = ItemRarityID.LightRed;
        
        Description = """
                      "The war machine of the cursed guardian."
                      Rust and ruin weave through this fragment’s core,
                      whispering tales of devastation and unyielding
                      bloodlust.
                      """;
        Souls = 5000;
    }
}