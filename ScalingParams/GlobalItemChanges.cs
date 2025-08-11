using System.Collections.Generic;
using CustomRecipes.Extensions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.ScalingParams;

public class GlobalItemChanges : GlobalItem
{
    private const string MediumSeaGreenColorTooltip = "c/3cb371";
    
    public override bool CanUseItem(Item item, Player player)
    {
        if (item.type is ItemID.LifeCrystal or ItemID.ManaCrystal or ItemID.LifeFruit)
        {
            return false;
        }

        var dsPlayer = Main.LocalPlayer.GetModPlayer<RingPlayer>();
        if (!CustomRecipes.AllWeaponsParams.TryGetValue(item.type, out var weaponParams))
        {
            return true;
        }

        return dsPlayer.RealStr() >= weaponParams.RStr &&
               dsPlayer.RealDex() >= weaponParams.RDex &&
               dsPlayer.RealInt() >= weaponParams.RInt &&
               dsPlayer.RealFai() >= weaponParams.RFai;
    }

    public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
    {
        if (!CustomRecipes.AllWeaponsParams.TryGetValue(item.type, out _))
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
        if (!CustomRecipes.AllWeaponsParams.TryGetValue(item.type, out var weaponParams))
        {
            return;
        }

        TooltipLine customTooltipLine = new(Mod, "WeaponParams", weaponParams.ToTooltipText());
        tooltips.Add(customTooltipLine);

        var damageBonuses = item.GetBonusDamage();
        if (damageBonuses.Total == 0)
        {
            return;
        }

        var damageLineIndex = tooltips.FindIndex(line => line.Mod == "Terraria" && line.Name == "Damage");
        if (damageLineIndex == -1)
        {
            return;
        }

        var damageLine = tooltips[damageLineIndex];
        damageLine.Text += $" ([{MediumSeaGreenColorTooltip}:+{damageBonuses.Total}] = " +
                           $"[{MediumSeaGreenColorTooltip}:{damageBonuses.ByStrength}]+" +
                           $"[{MediumSeaGreenColorTooltip}:{damageBonuses.ByDexterity}]+" +
                           $"[{MediumSeaGreenColorTooltip}:{damageBonuses.ByIntelligence}]+" +
                           $"[{MediumSeaGreenColorTooltip}:{damageBonuses.ByFaith}])";
    }
}