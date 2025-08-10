using System.Linq;
using CustomRecipes.Extensions;
using CustomRecipes.Rings;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CustomRecipes;

// ReSharper disable once ClassNeverInstantiated.Global
public class RingPlayer : ModPlayer
{
    public readonly Item[] RingSlots = new Item[4];

    // public override void Load()
    // {
    // base.Load();

    // On_Player.DropItems += (orig, player) =>
    // {
    //     if (player.dead)
    //     {
    //         return;
    //     }
    //
    //     orig(player);
    // };
    //
    // On_Player.DropCoins += (orig, player) => player.dead ? 0L : orig(player);
    // }

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
        _customLuckBonus = 0f;
        
        HasSerpentRingEffect = false;
        HasCovetousGoldSerpentRingEffect = false;

        foreach (var ring in RingSlots)
        {
            if (!ring.IsAir && ring.ModItem is ModRing modRing)
            {
                modRing.UpdateAccessory(Player, false);
            }
        }

        if (HasCovetousGoldSerpentRingEffect)
        {
            _customLuckBonus += 0.2f;
        }

        Player.GetDamage(DamageClass.Melee) += Strength * 0.02f; // +2% por punto de fuerza
        Player.statDefense += Dexterity / 2; // +0.5 defensa por punto de destreza
        Player.GetDamage(DamageClass.Magic) += Intelligence * 0.03f; // +3% daño mágico por inteligencia
    }

    public override void OnEnterWorld()
    {
        base.OnEnterWorld();
        //Main.NewText("Bienvenido, no olvides equipar tus anillos.", Color.Cyan);
    }

    public void DropMoney(float value)
    {
        if (value <= 0f || !HasSerpentRingEffect)
        {
            return;
        }

        var valueAdded = (int)(value * 20 / 100);

        var platine = valueAdded / 1000000;
        var gold = valueAdded % 1000000 / 10000;
        var silver = valueAdded % 10000 / 100;
        var copper = valueAdded % 100;

        if (platine > 0)
        {
            Item.NewItem(Player.GetSource_Loot(), new Vector2(Player.Center.X, Player.Center.Y - 100),
                ItemID.PlatinumCoin, platine);
        }

        if (gold > 0)
        {
            Item.NewItem(Player.GetSource_Loot(), new Vector2(Player.Center.X, Player.Center.Y - 100), ItemID.GoldCoin,
                gold);
        }

        if (silver > 0)
        {
            Item.NewItem(Player.GetSource_Loot(), new Vector2(Player.Center.X, Player.Center.Y - 100),
                ItemID.SilverCoin, silver);
        }

        if (copper > 0)
        {
            Item.NewItem(Player.GetSource_Loot(), new Vector2(Player.Center.X, Player.Center.Y - 100),
                ItemID.CopperCoin, copper);
        }

        //Main.NewText($"¡Dinero +20%! {platine} platino, {gold} oro, {silver} plata, {copper} cobre", Colors.CoinPlatinum);
    }

    private float _customLuckBonus; // Bonus adicional de suerte

    public bool HasSerpentRingEffect;
    public bool HasCovetousGoldSerpentRingEffect;

    public override void ModifyLuck(ref float luck)
    {
        luck += _customLuckBonus; // Aplica el bonus
    }

    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
        base.Kill(damage, hitDirection, pvp, damageSource);

        if (Main.myPlayer == Player.whoAmI)
        {
            ModContent.GetInstance<RingSystem>().ShowDeathUi();
        }
    }

    public override void OnRespawn()
    {
        base.OnRespawn();

        ModContent.GetInstance<RingSystem>().HideDeathUi();
    }

    public int Level = 1;
    public int Experience = 0; // opcional si quieres XP
    public int Strength = 5;
    public int Dexterity = 5;
    public int Intelligence = 5;

    public bool TryLevelUp()
    {
        var cost = Level * 1000; // costo en cobre (ejemplo)

        if (Player.CountCoins() >= cost)
        {
            Player.SpendCoins(cost);
            Level++;
            Strength += 1;
            Dexterity += 1;
            Intelligence += 1;
            // Opcional: mensaje al jugador
            Main.NewText($"¡Has subido al nivel {Level}!", Color.Gold);
            return true;
        }

        Main.NewText($"No tienes suficiente oro. Necesitas {cost / 100f} monedas de oro.", Color.Red);
        return false;
    }
}