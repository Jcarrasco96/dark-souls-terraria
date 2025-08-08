using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CustomRecipes.Bonfire;

public class BonfireItem : ModItem
{
    
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Hoguera");
        // Tooltip.SetDefault("Colócala y haz clic derecho para viajar");
    }

    public override void SetDefaults()
    {
        // Item.width = 28;
        // Item.height = 28;
        // Item.useTurn = true;
        // Item.autoReuse = true;
        // Item.useAnimation = 15;
        // Item.useTime = 10;
        // Item.useStyle = ItemUseStyleID.Swing;
        // Item.consumable = true;
        // Item.createTile = ModContent.TileType<BonfireTile>();
        
        Item.DefaultToPlaceableTile(ModContent.TileType<BonfireTile>());
    }
    
    public override void AddRecipes()
    {
        var r1 = CreateRecipe();
        r1.AddIngredient(ItemID.Campfire);
        r1.AddIngredient(ItemID.IronBroadsword);
        r1.Register();

        var r2 = CreateRecipe();
        r2.AddIngredient(ItemID.Campfire);
        r2.AddIngredient(ItemID.LeadBroadsword);
        r2.Register();
        
        var r3 = CreateRecipe();
        r3.AddIngredient(ItemID.Wood, 2);
        r3.Register();
    }
    
}