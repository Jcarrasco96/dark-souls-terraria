namespace TerraSouls.Commons;

public readonly struct DamageBonuses(int total, int byStrength, int byDexterity, int byIntelligence, int byFaith)
{
    public readonly int Total = total;
    public readonly int ByStrength = byStrength;
    public readonly int ByDexterity = byDexterity;
    public readonly int ByIntelligence = byIntelligence;
    public readonly int ByFaith = byFaith;
}