using System;
using Terraria;

namespace CustomRecipes.Extensions;

public static class PlayerExtensions
{
    
    public static int CountCoins(this Player player)
    {
        var totalCopper = 0;
        for (var i = 0; i < 59; i++) // inventario normal
        {
            var item = player.inventory[i];
            if (item != null && item.IsCoin())
            {
                totalCopper += item.stack * item.CoinValue(item.type);
            }
        }
        return totalCopper;
    }

    public static bool SpendCoins(this Player player, int amount)
    {
        if (player.CountCoins() < amount)
        {
            return false;
        }

        var toSpend = amount;

        for (var i = 0; i < 59; i++)
        {
            var item = player.inventory[i];
            if (item == null || !item.IsCoin())
            {
                continue;
            }

            var coinValue = item.CoinValue(item.type) * item.stack;
            if (coinValue <= toSpend)
            {
                toSpend -= coinValue;
                item.TurnToAir();
            }
            else
            {
                var coinsNeeded = (int)Math.Ceiling(toSpend / (float)item.CoinValue(item.type));
                item.stack -= coinsNeeded;
                if (item.stack <= 0) item.TurnToAir();
                toSpend = 0;
            }
            if (toSpend <= 0)
                break;
        }
        return true;
    }
    
}