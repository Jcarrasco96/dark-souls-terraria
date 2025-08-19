using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraSouls.Extensions;

namespace TerraSouls.Globals;

// ReSharper disable once ClassNeverInstantiated.Global
public class GlobalItemChanges : GlobalItem
{
    private const string MediumSeaGreenColorTooltip = "c/3cb371";
    
    public override bool CanUseItem(Item item, Player player)
    {
        if (item.type is ItemID.LifeCrystal or ItemID.ManaCrystal or ItemID.LifeFruit)
        {
            return false;
        }

        var wp = item.WeaponParams();
        
        if (wp.IsEmpty())
        {
            return true;
        }
        
        var dsPlayer = Main.LocalPlayer.GetModPlayer<RingPlayer>();

        return dsPlayer.CanUseItem(wp);
    }

    public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
    {
        if (!TerraSouls.AllWeaponsParams.TryGetValue(item.type, out _))
        {
            return;
        }

        var damageBonuses = item.GetBonusDamage();
        if (damageBonuses.Total == 0)
        {
            return;
        }

        damage.Flat = damageBonuses.Total;
    }
    
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        var wp = item.WeaponParams();
        
        if (!wp.IsEmpty())
        {
            var damageBonuses = item.GetBonusDamage();
            var damageLineIndex = tooltips.FindIndex(line => line.Mod == "Terraria" && line.Name == "Damage");

            if (damageBonuses.Total != 0 && damageLineIndex != -1)
            {
                tooltips[damageLineIndex].Text += $" ([{MediumSeaGreenColorTooltip}:+{damageBonuses.Total}] = " +
                                                  $"[{MediumSeaGreenColorTooltip}:{damageBonuses.ByStrength}]+" +
                                                  $"[{MediumSeaGreenColorTooltip}:{damageBonuses.ByDexterity}]+" +
                                                  $"[{MediumSeaGreenColorTooltip}:{damageBonuses.ByIntelligence}]+" +
                                                  $"[{MediumSeaGreenColorTooltip}:{damageBonuses.ByFaith}])";
            }
            
            tooltips.Add(new TooltipLine(Mod, "WeaponParams", wp.ToTooltipText()));
        }
        
        if (IsWeapon(item) && Elements.Count > 0)
        {
            var text = Elements.Aggregate("", (current, kv) => current + ColorToTag(GetElementColor(kv.Key), kv.Key.ToString()) + " +" + (int)(kv.Value * 100) + "%\n");

            tooltips.Add(new TooltipLine(Mod, "WeaponParams", $"{text.Trim()}"));
        }
    }
    
    public override bool InstancePerEntity => true;
    
    public readonly Dictionary<GlobalType, float> Elements = new();
    
    public override void SetDefaults(Item item)
    {
        switch (item.type)
        {
            case ItemID.Flamarang:
                Elements[GlobalType.Fire] = 0.3f;      // +30% fire
                Elements[GlobalType.Poison] = 0.15f;   // +15% poison
                return;
            case ItemID.IceBlade:
                Elements[GlobalType.Ice] = 0.3f;       // +30% ice
                return;
            case ItemID.ThunderStaff:
                Elements[GlobalType.Lightning] = 0.3f; // +30% lightning
                return;
        }
    }
    
    private static bool IsWeapon(Item item) => item.damage > 0;

    public static Color GetElementColor(GlobalType type)
    {
        return type switch
        {
            GlobalType.Fire => Colors.RarityOrange,
            GlobalType.Lightning => Colors.RarityNormal,
            GlobalType.Ice => Colors.RarityBlue,
            GlobalType.Poison => Colors.RarityGreen,
            GlobalType.Holy => Colors.RarityRed,
            GlobalType.Dark => Colors.RarityPurple,
            _ => Colors.RarityTrash
        };
    }

    private static string ColorToTag(Color color, string text)
    {
        return $"[c/{color.R:X2}{color.G:X2}{color.B:X2}:{text}]";
    }
}