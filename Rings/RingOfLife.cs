using Terraria;
using Terraria.ID;

namespace CustomRecipes.Rings;

public class RingOfLife : ModRing
{
    
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.rare = ItemRarityID.Red;
    }

    public override void ApplyEffects(Player player)
    {
        player.statLifeMax2 += player.statLifeMax * 20 / 100; // Aumenta la vida máxima en 100
        
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