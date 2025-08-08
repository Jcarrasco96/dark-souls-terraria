using Terraria;
using Terraria.ModLoader;

namespace CustomRecipes.Buffs;

public class ExampleDefenseDebuff : ModBuff
{
    private const int DefenseReductionPercent = 25; // This is the percentage of defense reduction applied by the buff.
    private const float DefenseMultiplier = 1 - DefenseReductionPercent / 100f;

    public override void SetStaticDefaults()
    {
        Main.pvpBuff[Type] = true; // This buff can be applied by other players in Pvp, so we need this to be true.

        // Our BuffImmuneGlobalNPC class changes some buff immunity logic. NPCs immune to Ichor will automatically be immune to this buff.
        //BuffImmuneGlobalNPC.SetDefenseDebuffStaticDefaults(Type);

        Main.buffNoTimeDisplay[Type] = true;
        Main.debuff[Type] = false;
        Main.persistentBuff[Type] = true;
    }

    public override void Update(NPC npc, ref int buffIndex)
    {
        //npc.GetGlobalNPC<DamageModificationGlobalNPC>().exampleDefenseDebuff = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.statDefense *= DefenseMultiplier;
        player.GetDamage(DamageClass.Generic) += 0.09f;
    }
    
}