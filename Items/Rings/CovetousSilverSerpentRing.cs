using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Items.Rings;

public class CovetousSilverSerpentRing : ModRing
{
    private const string Description = """
                                       Fallen foes yield more souls.
                                       A silver ring depicting a snake that could have been, but never was, a dragon.
                                       Snakes are known as creatures of great avarice, devouring prey even larger than
                                       themselves by swallowing them whole. If one's shackles are cause for discontent,
                                       perhaps it is time for some old-fashioned greed.
                                       """;
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "+20% gold from killed monsters"));
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.GetModPlayer<RingPlayer>().HasSerpentRingEffect = true;
    }
}