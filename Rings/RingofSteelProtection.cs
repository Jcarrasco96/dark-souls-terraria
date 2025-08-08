using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.Rings;

public class RingofSteelProtection : ModRing
{
    
    public override void ApplyEffects(Player player)
    {
        player.statDefense += 5; // +5 defensa
        
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