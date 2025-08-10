using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.Buffs;

public class BurnDebuff : ModBuff
{
    
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
    }

    public override void Update(NPC npc, ref int buffIndex)
    {
        npc.lifeRegen -= 20; // daño por segundo
        Dust.NewDust(npc.position, npc.width, npc.height, DustID.FlameBurst);
    }
    
}