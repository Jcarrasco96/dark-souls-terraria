using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Globals;

// ReSharper disable once ClassNeverInstantiated.Global
public class GlobalProjectileChanges : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    public Dictionary<GlobalType, float> Elements = new();

    public override GlobalProjectile Clone(Projectile projectile, Projectile newProjectile)
    {
        var clone = (GlobalProjectileChanges)base.Clone(projectile, newProjectile);
        clone.Elements = new Dictionary<GlobalType, float>(Elements);
        return clone;
    }

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        // if (projectile.type == ProjectileID.WandOfFrostingFrost)
        // {
        //     Main.NewText($"time left: {projectile.timeLeft}");
        //     projectile.timeLeft *= 5;
        //     Main.NewText($"new time left: {projectile.timeLeft}");
        // }
        
        if (source is EntitySource_Parent { Entity: NPC npc })
        {
            var npcAttr = npc.GetGlobalNPC<GlobalNpcChanges>();
            var projAttr = projectile.GetGlobalProjectile<GlobalProjectileChanges>();

            projAttr.BleedPower = npcAttr.BleedPower;
            projAttr.PoisonPower = npcAttr.PoisonPower;
        }
        
        if (source is not EntitySource_ItemUse_WithAmmo src || !src.Item.TryGetGlobalItem<GlobalItemChanges>(out var elemItem))
        {
            return;
        }

        foreach (var kv in elemItem.Elements)
        {
            Elements[kv.Key] = kv.Value;
        }
    }
    
    public float BleedPower;
    public float PoisonPower;
}