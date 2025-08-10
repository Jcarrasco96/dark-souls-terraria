using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.Rings;

public class MagicClutchRing : ModRing
{
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        tooltips.Add(new TooltipLine(Mod, "WeaponParams", "-50% defense\n+10% damage"));
        base.ModifyTooltips(tooltips);
    }

    private const int DefenseReductionPercent = 50; // This is the percentage of defense reduction applied by the buff.
    private const float DefenseMultiplier = 1 - DefenseReductionPercent / 100f;

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.statDefense *= DefenseMultiplier;
        player.GetDamage(DamageClass.Generic) += 0.10f;
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