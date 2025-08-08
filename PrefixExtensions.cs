using Terraria;
using Terraria.ID;

namespace CustomRecipes;

public static class PrefixExtensions
{
    
    public static float GetPrefixDamageBoost(this Item item)
    {
        return item.prefix switch
        {
            PrefixID.Lucky => 0.04f,
            PrefixID.Menacing => 0.04f,
            PrefixID.Quick => 0.03f,
            PrefixID.Violent => 0.04f,
            PrefixID.Arcane => 0f,
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