using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraSouls.Items.Souls;

namespace TerraSouls.Items.Rings;

public class BellowingDragoncrestRing : ModRing
{
    private const string Description = """
                                       Greatly boosts sorceries.
                                       A special ring given to those who are deemed fit to undertake the journey of
                                       discovery in Vinheim, home of sorcery. Apropos to the Dragon School, the seal
                                       depicts an everlasting dragon. A bellowing dragon symbolizes the true nature of
                                       the consumate sorcerer.
                                       """;
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "+20% magic dmg"));
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.GetModPlayer<RingPlayer>().HasBellowingDragoncrestRingEffect = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SoulOfTheKingSlime>()
            .AddIngredient(ItemID.Sapphire, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}