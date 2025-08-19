using System.Collections.Generic;
using TerraSouls.Items.Souls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Items.Rings;

// ReSharper disable once ClassNeverInstantiated.Global
public class PriestessRing : ModRing
{
    private const string Description = """
                                       Increases faith.
                                       A ring engraved with a portrait of the High Priestess. In Lothric, the High
                                       Priestess has long been considered one of the Three Pillars of the king's rule.
                                       The High Priestess also served as the prince's wet nurse.
                                       """;
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "+5 fai"));
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        // player.statDefense += 5; // +5 defensa
        // player.GetDamage(DamageClass.Generic) += 0.20f; // +5% daño
        
        player.GetModPlayer<RingPlayer>().HasPriestessRingEffect = true;
        // player.GetModPlayer<RingPlayer>().AddStatusEffect("SOULS", 300);
        // player.GetModPlayer<RingPlayer>().AddStatusEffect("SOULS", 300);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SoulOfTheEyeOfCthulhu>()
            .AddIngredient(ItemID.Ruby, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}