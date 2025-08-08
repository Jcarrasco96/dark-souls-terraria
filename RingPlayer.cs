using System.Linq;
using CustomRecipes.Rings;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CustomRecipes;

public class RingPlayer : ModPlayer
{

    public readonly Item[] RingSlots = new Item[4];

    public override void Initialize()
    {
        for (var i = 0; i < 4; i++)
        {
            RingSlots[i] = new Item();
            RingSlots[i].TurnToAir();
        }
    }

    public override void SaveData(TagCompound tag)
    {
        tag["RingSlots"] = RingSlots.Select(ItemIO.Save).ToList();
    }

    public override void LoadData(TagCompound tag)
    {
        var loadedItems = tag.GetList<TagCompound>("RingSlots");
        for (var i = 0; i < RingSlots.Length; i++)
        {
            RingSlots[i] = i < loadedItems.Count ? ItemIO.Load(loadedItems[i]) : new Item();
        }
    }

    public override void ResetEffects()
    {
        MoneyAmuletEquipped = false;
        CustomLuckBonus = 0f;
        showMyInfo = false;
        
        // Aquí luego aplicaremos las estadísticas que otorgan los anillos
        // Por ejemplo, recorres tus ranuras de anillos y aplicas sus efectos
        foreach (var ring in RingSlots)
        {
            if (!ring.IsAir && ring.ModItem is ModRing modRing)
            {
                modRing.ApplyEffects(Player);
            }
        }
    }

    public override void OnEnterWorld()
    {
        Main.NewText("Bienvenido, no olvides equipar tus anillos.", Color.Cyan);
    }
    
    public bool MoneyAmuletEquipped = false;

    public void DropMoney(float value)
    {
        if (value <= 0f)
        {
            return;
        }
        
        if (!MoneyAmuletEquipped)
        {
            return;
        }

        var valueAdded = (int)(value * 20 / 100);

        var platine = valueAdded / 1000000;
        var gold = (valueAdded % 1000000) / 10000;
        var silver = (valueAdded % 10000) / 100;
        var copper = valueAdded % 100;
        
        if (platine > 0)
        {
            Item.NewItem(Player.GetSource_Loot(), new Vector2(Player.Center.X, Player.Center.Y - 100), ItemID.PlatinumCoin, platine);
        }

        if (gold > 0)
        {
            Item.NewItem(Player.GetSource_Loot(), new Vector2(Player.Center.X, Player.Center.Y - 100), ItemID.GoldCoin, gold);
        }

        if (silver > 0)
        {
            Item.NewItem(Player.GetSource_Loot(), new Vector2(Player.Center.X, Player.Center.Y - 100), ItemID.SilverCoin, silver);
        }

        if (copper > 0)
        {
            Item.NewItem(Player.GetSource_Loot(), new Vector2(Player.Center.X, Player.Center.Y - 100), ItemID.CopperCoin, copper);
        }
        
        Main.NewText($"¡Dinero +20%! {platine} platino, {gold} oro, {silver} plata, {copper} cobre", Colors.CoinPlatinum);
    }
    
    public float CustomLuckBonus = 0f; // Bonus adicional de suerte
    
    public override void ModifyLuck(ref float luck)
    {
        luck += CustomLuckBonus; // Aplica el bonus
    }

    public bool showMyInfo;
    
}