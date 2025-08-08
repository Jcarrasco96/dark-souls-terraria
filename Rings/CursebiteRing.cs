using Terraria;
using Terraria.ID;

namespace CustomRecipes.Rings;

public class CursebiteRing : ModRing
{
    
    public override void ApplyEffects(Player player)
    {
        player.buffImmune[BuffID.CursedInferno] = true;
        player.buffImmune[BuffID.Silenced] = true;
        
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