using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.Items;

public class MoonSword : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 35;
        Item.DamageType = DamageClass.Melee;
        Item.width = 45;
        Item.height = 45;
        Item.useTime = 25;
        Item.useAnimation = 25;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 2.5f;
        Item.value = 10;
        Item.rare = ItemRarityID.Blue;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.useTurn = true;

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