using CustomRecipes.Rings;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CustomRecipes.InfoDisplay;

public class MyInfoDisplay : Terraria.ModLoader.InfoDisplay
{
    private static RingPlayer RingPlayer => Main.LocalPlayer.GetModPlayer<RingPlayer>();

    public override bool Active()
    {
        // return true;
        return HasItemInRingInventory(ModContent.ItemType<KnightsRing>());
    }

    public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
    {
        // return base.DisplayValue(ref displayColor, ref displayShadowColor);

        // var text = RingPlayer.RingSlots.Where(ringSlot => ringSlot.ModItem is ModRing).Aggregate("", (current, ringSlot) => current + (ringSlot.Name + "\n"));

        // return "Equipped Rings:\n" + text.Trim() + $"\n{RingPlayer.Player.moveSpeed}";
        return $"Luck: {RingPlayer.Player.luck}";
    }

    private static bool HasItemInRingInventory(int item)
    {
        return RingPlayer.HasCovetousGoldSerpentRingEffect;

        // var rings = player.RingSlots;
        //
        // foreach (var ring in rings)
        // {
        //     if (ring.ModItem is ModRing modRing && modRing.Type == item)
        //     {
        //         return true;
        //     }
        // }
    }
}