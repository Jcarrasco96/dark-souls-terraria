using Microsoft.Xna.Framework;
using Terraria;

namespace TerraSouls.InfosDisplay;

public class SoulsInfoDisplay : Terraria.ModLoader.InfoDisplay
{
    private static RingPlayer RingPlayer => Main.LocalPlayer.GetModPlayer<RingPlayer>();
    
    public override bool Active()
    {
        return true;
    }
    
    public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
    {
        return $"Souls: {RingPlayer.Souls}, Humanity: {RingPlayer.Humanity}";
    }
}