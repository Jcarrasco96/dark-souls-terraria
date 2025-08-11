using System.Collections.Generic;
using CustomRecipes.Souls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.Rings;

// ReSharper disable once ClassNeverInstantiated.Global
public class HuntersRing : ModRing
{
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        tooltips.Add(new TooltipLine(Mod, "WeaponParams", "+5 dex"));
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
            //.AddIngredient(ItemID.IronBar, 5)
            //.AddIngredient(ItemID.FallenStar, 1)
            .AddIngredient(ItemID.Wood, 1)
            .AddTile(TileID.Anvils)
            .Register();

        CreateRecipe()
            //.AddIngredient(ItemID.IronBar, 5)
            //.AddIngredient(ItemID.FallenStar, 1)
            //.AddIngredient(ItemID.Wood, 1)
            .AddIngredient<SoulOfSkeletron>(1)
            .AddIngredient(ItemID.GoldCoin, 5)
            .AddTile(TileID.Anvils)
            .Register();
    }
}