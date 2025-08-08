using CustomRecipes.Rings;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CustomRecipes;

public class MyInfoDisplay : InfoDisplay
{
    
    private static RingPlayer RingPlayer => Main.LocalPlayer.GetModPlayer<RingPlayer>();
    
    public override bool Active()
    {
        // Devuelve true si se debe mostrar la 
        return true;
        // return Main.LocalPlayer.HasItem(ModContent.ItemType<RingOfStrength>());
    }

    public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
    {
        // return base.DisplayValue(ref displayColor, ref displayShadowColor);

        var text = "";

        foreach (var ringSlot in RingPlayer.RingSlots)
        {
            if (ringSlot.ModItem is ModRing)
            {
                text += ringSlot.Name + "\n";
            }
        }
        
        return "Equipped Rings:\n" + text.Trim() + $"\n{RingPlayer.Player.moveSpeed}";
    }
    
}