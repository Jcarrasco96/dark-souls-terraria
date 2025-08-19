using System.Collections.Generic;
using TerraSouls.Items.Souls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Items.Rings;

// ReSharper disable once ClassNeverInstantiated.Global
public class ScholarRing : ModRing
{
    private const string Description = """
                                       Increases intelligence.
                                       A ring engraved with a potrait of a scholar. In Lothric, the Scholar has long
                                       been considered one of the Three Pillars of the king's rule, and is therefore
                                       master of the Grand Archives.
                                       """;
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "+5 int"));
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        // player.statDefense += 5; // +5 defensa
        // player.GetDamage(DamageClass.Generic) += 0.20f; // +5% daño
        
        player.GetModPlayer<RingPlayer>().HasScholarRingEffect = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SoulOfTheWorldEater>()
            .AddIngredient(ItemID.Ruby, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}