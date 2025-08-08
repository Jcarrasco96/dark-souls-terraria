using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.Rings;

public class RingOfRay : ModRing
{
    
    private const int DefenseReductionPercent = 50; // This is the percentage of defense reduction applied by the buff.
    private const float DefenseMultiplier = 1 - DefenseReductionPercent / 100f;
    
    public override void ApplyEffects(Player player)
    {
        // player.AddBuff(ModContent.BuffType<ExampleDefenseDebuff>(), 2);
        
        player.statDefense *= DefenseMultiplier;
        player.GetDamage(DamageClass.Generic) += 0.09f;
        
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