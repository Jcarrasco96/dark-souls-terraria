using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraSouls.Commons;

namespace TerraSouls.Globals;

// ReSharper disable once ClassNeverInstantiated.Global
public class DarkSoulsBuffChanges : GlobalBuff
{
    public static readonly List<int> TerrariaDebuff =
    [
        BuffID.Poisoned,
        BuffID.PotionSickness,
        BuffID.Darkness,
        BuffID.Cursed,
        BuffID.OnFire,
        BuffID.Tipsy,
        BuffID.Bleeding,
        BuffID.Confused,
        BuffID.Slow,
        BuffID.Weak,
        BuffID.Silenced,
        BuffID.BrokenArmor,
        BuffID.Horrified,
        BuffID.TheTongue,
        BuffID.CursedInferno,
        BuffID.Frostburn,
        BuffID.Chilled,
        BuffID.Frozen,
        BuffID.Suffocation,
        BuffID.Ichor,
        BuffID.Venom,
        BuffID.Blackout,
        BuffID.ChaosState,
        BuffID.Electrified,
        BuffID.MoonLeech,
        BuffID.Rabies,
        BuffID.Webbed,
        BuffID.Stoned,
        BuffID.VortexDebuff,
        BuffID.BoneJavelin,
        BuffID.WitheredWeapon,
        BuffID.OgreSpit,
        BuffID.OnFire3,
        BuffID.Frostburn2,
        BuffID.BloodButcherer
    ];

    // GlobalBuff.ReApply function overload is used to handle the reapplication of debuffs
    public override bool ReApply(int type, Player player, int time, int buffIndex)
    {
        if (!TerrariaDebuff.Contains(type))
        {
            return false;
        }

        if (time <= player.buffTime[buffIndex])
        {
            return true;
        }

        player.buffTime[buffIndex] = (int)(time * (1 - StatFormulas.GetDebuffsResistanceByResistance(player.GetModPlayer<RingPlayer>().RealResistance())));

        return true;
    }
}