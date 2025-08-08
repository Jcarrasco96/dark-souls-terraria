using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.Items;

public class Shortsword : ModItem
{

    public override void SetDefaults()
    {
        Item.damage = 50;
        Item.DamageType = DamageClass.Melee;
        Item.width = 100;
        Item.height = 100;
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 4f;
        Item.value = 10;
        Item.rare = ItemRarityID.White;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;

        // Item.noUseGraphic = true; // The sword is actually a "projectile", so the item should not be visible when used
        // Item.noMelee = true; // The projectile will do the damage and not the item
        // Item.shoot = ModContent.ProjectileType<ExampleShortswordProjectile>(); // The projectile is what makes a shortsword work
        // Item.shootSpeed = 2.1f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Wood, 1)
            // .AddIngredient(ItemID.Gel, 6)
            .AddTile(TileID.Anvils)
            .Register();
    }
    
}