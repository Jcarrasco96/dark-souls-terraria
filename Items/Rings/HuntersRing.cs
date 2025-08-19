using System.Collections.Generic;
using TerraSouls.Items.Souls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Items.Rings;

// ReSharper disable once ClassNeverInstantiated.Global
public class HuntersRing : ModRing
{
    private const string Description = """
                                       Increases dexterity.
                                       Ring engraved with a portrait of a hunter. The hunters serve Lothric on the
                                       fringes and in the shadows. For generations, rulers of Lothric have relied
                                       especially upon the Black Hand hunters to punish enemies in ways that the king's
                                       Three Pillars cannot.
                                       """;
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        // tooltips.Add(new TooltipLine(Mod, "DescriptionParams", "+5 dex"));
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        // player.statDefense += 5; // +5 defensa
        // player.GetDamage(DamageClass.Generic) += 0.20f; // +5% daño
        
        player.GetModPlayer<RingPlayer>().HasHuntersRingEffect = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SoulOfDeerclops>()
            .AddIngredient(ItemID.Ruby, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}