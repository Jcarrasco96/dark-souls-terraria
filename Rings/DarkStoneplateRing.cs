using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.Rings;

public class DarkStoneplateRing : ModRing
{
    
    public override void ApplyEffects(Player player)
    {
        player.endurance += 0.05f; // Reduce daño total en 5%
        
        base.ApplyEffects(player);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            //.AddIngredient(ItemID.IronBar, 5)
            //.AddIngredient(ItemID.FallenStar, 1)
            .AddIngredient(ItemID.Wood, 1)
            .AddTile(TileID.Anvils)
            .Register();
    }

}