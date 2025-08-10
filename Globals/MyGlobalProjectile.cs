using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CustomRecipes.Globals;

// ReSharper disable once ClassNeverInstantiated.Global
public class MyGlobalProjectile : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    public Dictionary<GlobalType, float> Elements = new();

    public override GlobalProjectile Clone(Projectile projectile, Projectile newProjectile)
    {
        var clone = (MyGlobalProjectile)base.Clone(projectile, newProjectile);
        clone.Elements = new Dictionary<GlobalType, float>(Elements);
        return clone;
    }

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (source is not EntitySource_ItemUse_WithAmmo src ||
            !src.Item.TryGetGlobalItem<MyGlobalItem>(out var elemItem))
        {
            return;
        }

        foreach (var kv in elemItem.Elements)
        {
            Elements[kv.Key] = kv.Value;
        }
    }
}