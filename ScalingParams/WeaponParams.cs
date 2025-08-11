using Terraria;

namespace CustomRecipes.ScalingParams;

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

    private const string ReqParamDisplayName = "ReqParam";
    private const string ParamBonusDisplayName = "ParamBonus";
    private const string StrengthDisplayName = "Strength";
    private const string DexterityDisplayName = "Dexterity";
    private const string IntelligenceDisplayName = "Intelligence";
    private const string FaithDisplayName = "Faith";

    private const string DodgerBlueColorTooltip = "c/1e90ff";
    private const string CrimsonColorTooltip = "c/dc143c";

    public string ToTooltipText()
    {
        var dsPlayer = Main.LocalPlayer.GetModPlayer<RingPlayer>();
        var strengthText = dsPlayer.RealStr() >= RStr
            ? $"{StrengthDisplayName}: [{DodgerBlueColorTooltip}:{RStr}]"
            : $"{StrengthDisplayName}: [{CrimsonColorTooltip}:{RStr}]";
        var dexterityText = dsPlayer.RealDex() >= RDex
            ? $"{DexterityDisplayName}: [{DodgerBlueColorTooltip}:{RDex}]"
            : $"{DexterityDisplayName}: [{CrimsonColorTooltip}:{RDex}]";
        var intelligenceText = dsPlayer.RealInt() >= RInt
            ? $"{IntelligenceDisplayName}: [{DodgerBlueColorTooltip}:{RInt}]"
            : $"{IntelligenceDisplayName}: [{CrimsonColorTooltip}:{RInt}]";
        var faithText = dsPlayer.RealFai() >= RFai
            ? $"{FaithDisplayName}: [{DodgerBlueColorTooltip}:{RFai}]"
            : $"{FaithDisplayName}: [{CrimsonColorTooltip}:{RFai}]";

        return $"{ReqParamDisplayName}:\n  {strengthText}, {dexterityText}, {intelligenceText}, {faithText}\n" +
               $"{ParamBonusDisplayName}:\n  {StrengthDisplayName}: {ScalingGradeToString(StrScalingGrade)}, " +
               $"{DexterityDisplayName}: {ScalingGradeToString(DexScalingGrade)}, " +
               $"{IntelligenceDisplayName}: {ScalingGradeToString(IntScalingGrade)}, " +
               $"{FaithDisplayName}: {ScalingGradeToString(FaiScalingGrade)}";
    }

    private static string ScalingGradeToString(ScalingGrade level) =>
        level == ScalingGrade.None ? "-" : level.ToString();
}