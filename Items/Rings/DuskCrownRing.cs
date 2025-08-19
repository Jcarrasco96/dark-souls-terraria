using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraSouls.Items.Souls;

namespace TerraSouls.Items.Rings;

public class DuskCrownRing : ModRing
{
    private const string Description = """
                                       Reduces consumption of mana, but also lowers max life.
                                       Leaf-colored crown ring bestowed upon the princess of Oolacile, ancient land of
                                       golden sorceries. Oolacile is synonymous for its lost sorceries of which the
                                       xanthous sorcerers are dedicated scholars. This crown ring is a rare artifact of
                                       great magic heritage.
                                       """;
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "-25% mana consumption"));
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.GetModPlayer<RingPlayer>().HasDuskCrownRingEffect = true;
        // player.statLifeMax2 += player.statLifeMax * LifeRaisePercent / 100;
        
        player.statLifeMax2 /= 2;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SoulOfTheBrainOfCthulhu>()
            .AddIngredient(ItemID.Emerald, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
    
    // Reduces FP consumption of Sorceries, Pyromancies and Miracles by 25%
    // Reduces maximum HP by 15% (Example: 1000hp to 850hp)
}