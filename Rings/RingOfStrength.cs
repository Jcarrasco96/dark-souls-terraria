using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.Rings;

public class RingOfStrength : ModRing
{

    public override void ApplyEffects(Player player)
    {
        player.GetDamage(DamageClass.Generic) += 0.05f;  // +5% daño
        player.statDefense += 5;                         // +5 defensa
        
        base.ApplyEffects(player);
    }

    public override void UpdateInventory(Player player)
    {
        base.UpdateInventory(player);
        
        // player.GetModPlayer<RingPlayer>().showMyInfo = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);
        
        player.GetModPlayer<RingPlayer>().showMyInfo = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            //.AddIngredient(ItemID.IronBar, 5)
            //.AddIngredient(ItemID.FallenStar, 1)
            .AddIngredient(ItemID.Wood, 1)
            .AddTile(TileID.Anvils)
            .Register();
    }

}