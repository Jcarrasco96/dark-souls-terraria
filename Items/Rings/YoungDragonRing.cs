using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraSouls.Items.Souls;

namespace TerraSouls.Items.Rings;

public class YoungDragonRing : ModRing
{
    private const string Description = """
                                       Boosts sorceries.
                                       Ring given in Vinheim, home of sorcery, when newly ordained as a sorcerer.
                                       Apropos to the Dragon School, the seal depicts an everlasting dragon. A young
                                       dragon presages the great length of the journey to mastery.
                                       """;
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "+12% magic dmg"));
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.GetModPlayer<RingPlayer>().HasYoungDragonRingEffect = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SoulOfTheTwins>()
            .AddIngredient(ItemID.Sapphire, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}