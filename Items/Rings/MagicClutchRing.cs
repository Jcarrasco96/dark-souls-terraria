using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraSouls.Items.Souls;

namespace TerraSouls.Items.Rings;

public class MagicClutchRing : ModRing
{
    private const string Description = """
                                       Increases attack, but compromises damage absorption.
                                       Ring depicting a hand grasping a blue stone. An old fable in Londor claims that
                                       the lure of the clutch ring reaches out to the crestfallen, who might otherwise
                                       be overcome by despair.
                                       """;
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "-50% defense\n+10% damage"));
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "Ring depicting a hand grasping a blue stone. Increases magic attack, but compromises damage absorption.\nAn old fable in Londor claims that the lure of the clutch ring reaches out to the crestfallen, who might otherwise be overcome by despair."));
        base.ModifyTooltips(tooltips);
    }

    private const int DefenseReductionPercent = 50; // This is the percentage of defense reduction applied by the buff.
    private const float DefenseMultiplier = 1 - DefenseReductionPercent / 100f;

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        player.statDefense *= DefenseMultiplier;
        player.GetDamage(DamageClass.Generic) += 0.10f;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SoulOfTheDestroyer>()
            .AddIngredient(ItemID.Sapphire, 5)
            .AddTile(TileID.Anvils)
            .Register();
    }
}