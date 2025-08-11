using CustomRecipes.ScalingParams;
using Terraria;
using Terraria.ID;

namespace CustomRecipes.Extensions;

public static class ItemExtensions
{
    public static int CoinValue(this Item item, int type)
    {
        // Valores en cobre de monedas vanilla:
        // Cobre = 1, Plata = 100, Oro = 10000, Platino = 1000000
        return type switch
        {
            ItemID.CopperCoin => 1,
            ItemID.SilverCoin => 100,
            ItemID.GoldCoin => 10000,
            ItemID.PlatinumCoin => 1000000,
            _ => 0
        };
    }

    public static bool IsCoin(this Item item)
    {
        return item.type is ItemID.CopperCoin or ItemID.SilverCoin or ItemID.GoldCoin or ItemID.PlatinumCoin;
    }
    
    public static float GetPrefixDamageBoost(this Item item)
    {
        return item.prefix switch
        {
            PrefixID.Lucky => 0.04f,
            PrefixID.Menacing => 0.04f,
            PrefixID.Quick => 0.03f,
            PrefixID.Violent => 0.04f,
            _ => 0f
        };
    }

    public static int GetPrefixDefenseBoost(this Item item)
    {
        return item.prefix switch
        {
            PrefixID.Warding => 4,
            _ => 0
        };
    }

    public static float GetPrefixMoveSpeedBoost(this Item item)
    {
        return item.prefix switch
        {
            PrefixID.Quick => 0.03f,
            _ => 0f
        };
    }

    public static int GetPrefixManaRegenBoost(this Item item)
    {
        return item.prefix switch
        {
            PrefixID.Arcane => 20,
            _ => 0
        };
    }
    
    public static DamageBonuses GetBonusDamage(this Item item)
    {
        if (!CustomRecipes.AllWeaponsParams.TryGetValue(item.type, out var wp))
        {
            return new DamageBonuses();
        }

        var player = Main.LocalPlayer.GetModPlayer<RingPlayer>();
        
        var bonusDamageByStrength = (int)(GetScalingGradeModifier(wp.StrScalingGrade) * StatFormulas.GetPotentialByStrength(player.RealStr()) * wp.Saturation);
        var bonusDamageByDexterity = (int)(GetScalingGradeModifier(wp.DexScalingGrade) * StatFormulas.GetPotentialByDexterity(player.RealDex()) * wp.Saturation);
        var bonusDamageByIntelligence = (int)(GetScalingGradeModifier(wp.IntScalingGrade) * StatFormulas.GetPotentialByIntelligence(player.RealInt()) * wp.Saturation);
        var bonusDamageByFaith = (int)(GetScalingGradeModifier(wp.FaiScalingGrade) * StatFormulas.GetPotentialByFaith(player.RealFai()) * wp.Saturation);
        var totalBonusDamage = bonusDamageByStrength + bonusDamageByDexterity + bonusDamageByIntelligence + bonusDamageByFaith;
        return new DamageBonuses(totalBonusDamage, bonusDamageByStrength, bonusDamageByDexterity, bonusDamageByIntelligence, bonusDamageByFaith);
    }
    
    private static float GetScalingGradeModifier(ScalingGrade level)
    {
        return level switch
        {
            ScalingGrade.S => 0.85f,
            ScalingGrade.A => 0.65f,
            ScalingGrade.B => 0.45f,
            ScalingGrade.C => 0.35f,
            ScalingGrade.D => 0.25f,
            ScalingGrade.E => 0.15f,
            _ => 0f,
        };
    }
    
}