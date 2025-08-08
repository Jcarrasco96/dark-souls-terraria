using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Utilities;

namespace CustomRecipes.Rings;

public class CloranthyRing : ModRing
{
    private const float StaminaRegenRateBonus = 0.25f;
    private const float StaminaRegenDelayReductionBonus = 0.15f;

    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs((int)(StaminaRegenRateBonus * 100), (int)(StaminaRegenDelayReductionBonus * 100));
    
    
    public override void ApplyEffects(Player player)
    {
        player.manaRegenBonus += 25; // Más regeneración de maná
        player.lifeRegen += 3; // Más regeneración de vida
        
        // base.ApplyEffects(player);
        
        ApplyPrefixEffects(Item, player);

        if (Item.prefix == PrefixID.Arcane)
        {
            player.statManaMax2 += 20;
            player.manaRegenBonus += 20;
        }
        
        if (Item.prefix == PrefixID.Quick2)
        {
            player.moveSpeed += player.moveSpeed * 20 / 100;
        }
    }
    
    public override int ChoosePrefix(UnifiedRandom rand)
    {
        // Solo permitir prefijos de accesorios
        return rand.Next([
            PrefixID.Arcane,
            PrefixID.Quick2,
        ]);
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