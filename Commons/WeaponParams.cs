using System;
using Terraria;
using TerraSouls.Enums;

namespace TerraSouls.Commons;

public readonly struct WeaponParams(
    int rStr = 0,
    int rDex = 0,
    int rInt = 0,
    int rFai = 0,
    ScalingGrade strScalingGrade = ScalingGrade.None,
    ScalingGrade dexScalingGrade = ScalingGrade.None,
    ScalingGrade intScalingGrade = ScalingGrade.None,
    ScalingGrade faiScalingGrade = ScalingGrade.None,
    float saturation = 100)
{
    public int RStr { get; } = rStr;
    public int RDex { get; } = rDex;
    public int RInt { get; } = rInt;
    public int RFai { get; } = rFai;

    public ScalingGrade StrScalingGrade { get; } = strScalingGrade;
    public ScalingGrade DexScalingGrade { get; } = dexScalingGrade;
    public ScalingGrade IntScalingGrade { get; } = intScalingGrade;
    public ScalingGrade FaiScalingGrade { get; } = faiScalingGrade;

    public float Saturation { get; } = saturation;

    private const string RpName = "ReqParam";
    private const string PbName = "ParamBonus";
    private const string StrengthName = "STR";
    private const string DexterityName = "DEX";
    private const string IntelligenceName = "INT";
    private const string FaithName = "FAI";

    private const string DodgerBlueColorTooltip = "c/1e90ff";
    private const string CrimsonColorTooltip = "c/dc143c";

    public string ToTooltipText()
    {
        var dsPlayer = Main.LocalPlayer.GetModPlayer<RingPlayer>();
        var strengthText = dsPlayer.RealStr() >= RStr ? $"{StrengthName}: [{DodgerBlueColorTooltip}:{RStr}]" : $"{StrengthName}: [{CrimsonColorTooltip}:{RStr}]";
        var dexterityText = dsPlayer.RealDex() >= RDex ? $"{DexterityName}: [{DodgerBlueColorTooltip}:{RDex}]" : $"{DexterityName}: [{CrimsonColorTooltip}:{RDex}]";
        var intelligenceText = dsPlayer.RealInt() >= RInt ? $"{IntelligenceName}: [{DodgerBlueColorTooltip}:{RInt}]" : $"{IntelligenceName}: [{CrimsonColorTooltip}:{RInt}]";
        var faithText = dsPlayer.RealFai() >= RFai ? $"{FaithName}: [{DodgerBlueColorTooltip}:{RFai}]" : $"{FaithName}: [{CrimsonColorTooltip}:{RFai}]";

        return $"{RpName}:\n  {strengthText}, {dexterityText}, {intelligenceText}, {faithText}\n" +
               $"{PbName}:\n  {StrengthName}: {ScalingColorToString(StrScalingGrade, true)}, " +
               $"{DexterityName}: {ScalingColorToString(DexScalingGrade, true)}, " +
               $"{IntelligenceName}: {ScalingColorToString(IntScalingGrade, true)}, " +
               $"{FaithName}: {ScalingColorToString(FaiScalingGrade, true)}";
    }

    private static string ScalingGradeToString(ScalingGrade level) => level == ScalingGrade.None ? "-" : level.ToString();

    private static string ScalingColorToString(ScalingGrade level, bool colored = false)
    {
        if (!colored)
        {
            return level == ScalingGrade.None ? "-" : level.ToString();
        }
        
        return level switch
        {
            ScalingGrade.None => "[c/828282:-]",
            ScalingGrade.A => "[c/FF9696:A]",
            ScalingGrade.B => "[c/FFC896:B]",
            ScalingGrade.C => "[c/96FF96:C]",
            ScalingGrade.D => "[c/9696FF:D]",
            ScalingGrade.E => "[c/FFFFFF:E]",
            ScalingGrade.S => "[c/FF96FF:S]",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }

    public bool IsEmpty()
    {
        return RStr <= 0 && RDex <= 0 && RInt <= 0 && RFai <= 0 &&
               StrScalingGrade == ScalingGrade.None && DexScalingGrade == ScalingGrade.None &&
               IntScalingGrade == ScalingGrade.None && FaiScalingGrade == ScalingGrade.None;
    }
}