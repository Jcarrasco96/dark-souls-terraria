using TerraSouls.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Items;

// ReSharper disable once ClassNeverInstantiated.Global
public class BonfireItem : ModItem
{
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
        CreateRecipe()
            .AddIngredient(ItemID.Campfire)
            .AddIngredient(ItemID.IronBroadsword)
            .Register();

        CreateRecipe()
            .AddIngredient(ItemID.Campfire)
            .AddIngredient(ItemID.LeadBroadsword)
            .Register();
    }
}