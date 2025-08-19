using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraSouls.Items.Souls;

namespace TerraSouls.Items.Rings;

public class CursebiteRing : ModRing
{
    private const string Description = """
                                       Immune to Cursed Inferno and Silenced buffs.
                                       One of the bite rings native to Carim. The crafting of these rings is forbidden,
                                       perhaps owing to a fear of malleable stone. Clerics, however, dabble freely in
                                       the art.
                                       """;
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "Immune to Cursed Inferno and Silenced buffs")); // 150 pts
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.buffImmune[BuffID.CursedInferno] = true;
        player.buffImmune[BuffID.Silenced] = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SoulOfTheWallOfFlesh>()
            .AddIngredient(ItemID.Topaz, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}