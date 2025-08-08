using Terraria;
using Terraria.ModLoader;

namespace CustomRecipes;

public class MoneyModifierGlobalNpc : GlobalNPC
{
    
    public override void OnKill(NPC npc)
    {
        base.OnKill(npc);
        
        if (npc.lastInteraction is < 0 or >= Main.maxPlayers)
        {
            return;
        }

        var modPlayer = Main.player[npc.lastInteraction];
        
        if (npc.value > 0)
        {
            modPlayer.GetModPlayer<RingPlayer>().DropMoney(npc.value);
        }
    }
    
}