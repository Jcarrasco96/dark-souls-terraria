using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.Souls;

public class Soul10 : ModItem
{
    
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.width = 64;
        Item.height = 64;
        Item.useStyle = ItemUseStyleID.None;

        Item.accessory = false;
        Item.material = true;
        
        Item.rare = ItemRarityID.LightRed;
    }
    
}