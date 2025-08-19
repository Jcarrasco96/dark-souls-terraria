using Terraria.ID;

namespace TerraSouls.Items.Souls;

public class SoulOfTheEyeOfCthulhu : ConsumableSoul
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.rare = ItemRarityID.LightRed;

        Description = """
                      "The harbinger of awakening."
                      A restless fragment, torn from the eye that once
                      pierced endless darkness. It watches still, hungry
                      for the day when oblivion falls.
                      """;
        Souls = 5000;
    }
}