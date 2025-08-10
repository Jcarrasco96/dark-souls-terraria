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
    
}