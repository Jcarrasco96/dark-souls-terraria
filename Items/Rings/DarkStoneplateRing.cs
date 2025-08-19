using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Items.Rings;

// ReSharper disable once ClassNeverInstantiated.Global
public class DarkStoneplateRing : ModRing
{
    private const string Description = """
                                       Increases damage absorption.
                                       Stoneplates are symbols of true knights, and dark purple stoneplates are granted
                                       to Undead Knights.
                                       """;
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "+10% reduced damage")); // 13%
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.endurance += 0.1f; // Reduce daño total en 10%
    }
}