using Terraria.GameContent.ItemDropRules;
using Terraria.ID;

namespace TerraSouls.Globals;

public class DropsFromNormalEnemiesOnlyCondition : IItemDropRuleCondition
{
    public string GetConditionDescription() =>
        "Dropped by hostile enemies that are not bosses and were not spawned using a statue";

    public bool CanDrop(DropAttemptInfo info)
    {
        var npc = info.npc;

        return !npc.boss && npc.type is not (NPCID.LunarTowerNebula or NPCID.LunarTowerSolar or NPCID.LunarTowerStardust or NPCID.LunarTowerVortex) && !npc.SpawnedFromStatue && !npc.friendly && !npc.townNPC && npc.lifeMax > 5 && npc.damage != 0;
    }

    public bool CanShowItemDropInUI() => true;
}