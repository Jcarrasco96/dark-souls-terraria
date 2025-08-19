using System.Collections.Generic;
using TerraSouls.Items.Souls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Items.Rings;

// ReSharper disable once ClassNeverInstantiated.Global
public class KnightsRing : ModRing
{
    private const string Description = """
                                       Increases strength.
                                       Ring engraved with a portrait of a knight. In Lothric, the Knight has long been
                                       considered one of the Three Pillars of the king's rule, and were thus allowed to
                                       rear dragons.
                                       """;
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "+5 str"));
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        // player.statDefense += 5; // +5 defensa
        // player.GetDamage(DamageClass.Generic) += 0.20f; // +5% daño
        
        player.GetModPlayer<RingPlayer>().HasKnightsRingEffect = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SoulOfSkeletron>()
            .AddIngredient(ItemID.Ruby, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}