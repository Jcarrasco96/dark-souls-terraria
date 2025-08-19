using Terraria.ID;

namespace TerraSouls.Items.Souls;

public class SoulOfTheQueenBee : ConsumableSoul
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.rare = ItemRarityID.Blue;
        
        Description = """
                      "The matriarch of the hive."
                      Slick with venom and the hum of a thousand wings,
                      this soul guards the sweetness of life and the
                      sting of death entwined.
                      """;
        Souls = 5000;
    }
}