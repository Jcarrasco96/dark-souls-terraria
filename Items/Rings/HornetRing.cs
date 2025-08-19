using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraSouls.Items.Souls;

namespace TerraSouls.Items.Rings;

public class HornetRing : ModRing
{
    private const string Description = """
                                       Boosts critical attacks.
                                       Ring associated with the Lord's Blade Ciaran, one of the Four Knights of Gwyn,
                                       the First Lord. The masked Ciaran was the only woman to serve in Gwyn's Four
                                       Knights, and her curved sword granted a swift death to any and all enemies of the
                                       throne.
                                       """;
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "+30% critic dmg"));
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.GetModPlayer<RingPlayer>().HasHornetRingEffect = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SoulOfTheQueenBee>()
            .AddTile(TileID.Anvils)
            .Register();
    }
}