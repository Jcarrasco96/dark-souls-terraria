using System;

namespace TerraSouls.Commons;

public static class StatFormulas
{
    public static int GetLifeByVitality(int vitality)
    {
        var hp = 0;
        hp += Math.Max(0, Math.Min(vitality - 1, 20)) * 15; // +300
        hp += Math.Max(0, Math.Min(vitality - 21, 20)) * 5; // +400
        hp += Math.Max(0, Math.Min(vitality - 41, 57)) * 2; // +514
        if (vitality >= 99)
        {
            hp = (hp + 9) / 10 * 10; // rounding by multiples of 10
        }

        return hp;
    }

    public static int GetManaByAttunement(int attunement)
    {
        var mana = 0;
        mana += Math.Max(0, Math.Min(attunement - 1, 9)) * 20; // +180
        mana += Math.Max(0, Math.Min(attunement - 10, 88)) * 2; // +356
        if (attunement >= 99)
        {
            mana = (mana + 9) / 10 * 10; // rounding by multiples of 10
        }

        return mana;
    }

    public static float GetDefenseByResistance(int resistance)
    {
        var defense = 0f;
        defense += Math.Max(0, Math.Min(resistance - 1, 25)) * 0.006f; // 15%
        defense += Math.Max(0, Math.Min(resistance - 26, 73)) * ((0.25f - defense) / 73); // 25%
        return defense;
    }

    public static float GetDebuffsResistanceByResistance(int resistance)
    {
        var debuffResistance = 0f;
        debuffResistance += Math.Max(0, Math.Min(resistance - 1, 25)) * 0.005f; // 12.5%
        debuffResistance += Math.Max(0, Math.Min(resistance - 26, 73)) * ((0.2f - debuffResistance) / 73); // 20%
        return debuffResistance;
    }
    
    public static float GetPotentialByStrength(int strength)
    {
        var potential = Math.Max(0, Math.Min(strength - 1, 40)) * (0.85f / 40); // 0.00 - 0.85 (strength: 1 - 41)
        potential += Math.Max(0, Math.Min(strength - 41, 58)) * (0.15f / 58); // 0.85 - 1.00 (strength: 41 - 99)
        return potential;
    }

    public static float GetPotentialByDexterity(int dexterity)
    {
        var potential = Math.Max(0, Math.Min(dexterity - 1, 40)) * (0.85f / 40); // 0.00 - 0.85 (dexterity: 1 - 41)
        potential += Math.Max(0, Math.Min(dexterity - 41, 58)) * (0.15f / 58); // 0.85 - 1.00 (dexterity: 41 - 99)
        return potential;
    }

    public static float GetPotentialByIntelligence(int intelligence)
    {
        var potential = Math.Max(0, Math.Min(intelligence - 1, 50)) * (0.85f / 50); // 0.00 - 0.85 (intelligence: 1 - 51)
        potential += Math.Max(0, Math.Min(intelligence - 51, 48)) * (0.15f / 48); // 0.85 - 1.00 (intelligence: 51 - 99)
        return potential;
    }

    public static float GetPotentialByFaith(int faith)
    {
        var potential = Math.Max(0, Math.Min(faith - 1, 50) * (0.85f / 50)); // 0.00 - 0.85 (faith: 1 - 51)
        potential += Math.Max(0, Math.Min(faith - 51, 48)) * (0.15f / 48); // 0.85 - 1.00 (faith: 51 - 99)
        return potential;
    }
    
    public static long GetReqSoulsByLevel(int level)
    {
        return level * 2;
        return level switch
        {
            > 0 and < 35 => (int)(500 * Math.Pow(1.025, level - 1)), // 1 - 35
            >= 35 => (int)(0.02 * Math.Pow(level, 3) + 3.05 * Math.Pow(level, 2) + 90 * level - 6500),
            _ => 0
        };
    }
}