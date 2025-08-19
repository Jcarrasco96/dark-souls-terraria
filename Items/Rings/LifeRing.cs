using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Items.Rings;

public class LifeRing : ModRing
{
    private const string Description = """
                                       Raises Maximum HP.
                                       Ring set with a small red jewel.
                                       """;
    
    private const int LifeRaisePercent = 50; // This is the percentage
    
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "+20% life")); // 7%
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.rare = ItemRarityID.LightRed;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.statLifeMax2 += player.statLifeMax * LifeRaisePercent / 100;
    }
}