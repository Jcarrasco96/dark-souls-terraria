using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.Items;

public class CustomSword : ModItem
{

    public override void SetDefaults()
    {
        Item.damage = 100;
        Item.DamageType = DamageClass.Melee;
        Item.width = 100;
        Item.height = 100;
        Item.useTime = 15;
        Item.useAnimation = 15;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 6;
        Item.value = 10000;
        Item.rare = ItemRarityID.Orange;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Wood, 12)
            .AddIngredient(ItemID.Gel, 6)
            .AddTile(TileID.Anvils)
            .Register();
    }

}