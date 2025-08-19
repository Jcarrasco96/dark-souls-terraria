using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraSouls.Items.Souls;

namespace TerraSouls.Items.Rings;

public class RingOfSteelProtection : ModRing
{
    private const string Description = """
                                       Increases physical damage absorption.
                                       Ring of the Knight King of ancient legend. The Knight King was said to be lined
                                       with steel on the inside, such that even the talons of mighty dragons did him
                                       little harm.
                                       """;
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "+5 defense")); // 10%
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.statDefense += 5; // +5 defensa
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SoulOfSkeletronPrime>()
            .AddTile(TileID.Anvils)
            .Register();
    }
}