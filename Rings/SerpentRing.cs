using Terraria;
using Terraria.ID;

namespace CustomRecipes.Rings;

public class SerpentRing : ModRing
{
    
    public override void ApplyEffects(Player player)
    {
        player.GetModPlayer<RingPlayer>().MoneyAmuletEquipped = true;
        // player.GetModPlayer<RingPlayer>().CustomLuckBonus += 1.5f; // +1.5 de suerte
        // player.GetModPlayer<RingPlayer>().AddDropRateStack(2.0f);
        
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