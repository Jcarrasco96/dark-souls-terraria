using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TerraSouls.Items.Rings;

// ReSharper disable once ClassNeverInstantiated.Global
public class ChloranthyRing : ModRing
{
    private const string Description = """
                                       Raises life and mana recovery speed.
                                       This old ring is named for its decorative green blossom, but its luster is long
                                       since faded.
                                       """;
    
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "+5 mana regen\n+5 life regen")); // 7 points by second
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.manaRegenBonus += 5;
        player.lifeRegen += 5 * 2;
    }
}