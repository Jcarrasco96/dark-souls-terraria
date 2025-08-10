using System.Collections.Generic;
using System.Linq;
using CustomRecipes.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CustomRecipes.Globals;

// ReSharper disable once ClassNeverInstantiated.Global
public class MyGlobalItem : GlobalItem
{
    public override bool InstancePerEntity => true;

    public readonly Dictionary<GlobalType, float> Elements = new();

    public override void SetDefaults(Item item)
    {
        // switch (item.type)
        // {
        //     case ItemID.Flamarang:
        //         Elements[GlobalType.Fire] = 0.3f;      // +30% fire
        //         Elements[GlobalType.Poison] = 0.15f;   // +15% poison
        //         break;
        //     case ItemID.IceBlade:
        //         Elements[GlobalType.Ice] = 0.3f;       // +30% ice
        //         break;
        //     case ItemID.ThunderStaff:
        //         Elements[GlobalType.Lightning] = 0.3f; // +30% lightning
        //         break;
        // }

        if (item.ModItem is Shortsword)
        {
            Elements[GlobalType.Fire] = 0f;
            Elements[GlobalType.Lightning] = 0f;
            Elements[GlobalType.Ice] = 0f;
            Elements[GlobalType.Poison] = 0f;
            Elements[GlobalType.Holy] = 0f;
            Elements[GlobalType.Dark] = 0f;
        }
    }

    // public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
    // {
    //     if (_elements.Count <= 0 || !target.TryGetGlobalNPC(out ElementalNPC npc))
    //     {
    //         return;
    //     }
    //
    //     foreach (var kv in _elements)
    //     {
    //         var elementalDamage = (int)(damageDone * kv.Value);
    //         npc.ApplyElementalDamage(elementalDamage, kv.Key);
    //
    //         // switch (kv.Key)
    //         // {
    //         //     case GlobalType.Fire:
    //         //         target.AddBuff(BuffID.OnFire, 180); // 3 segundos de fuego
    //         //         break;
    //         //     case GlobalType.Poison:
    //         //         target.AddBuff(BuffID.Poisoned, 300);
    //         //         break;
    //         //     case GlobalType.Lightning:
    //         //         target.AddBuff(BuffID.Bleeding, 300);
    //         //         break;
    //         //     case GlobalType.None:
    //         //     case GlobalType.Ice:
    //         //     case GlobalType.Holy:
    //         //     case GlobalType.Dark:
    //         //     default:
    //         //         break;
    //         // }
    //     }
    // }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (!IsWeapon(item))
        {
            return;
        }

        if (Elements.Count <= 0)
        {
            return;
        }

        var text = Elements.Aggregate("",
            (current, kv) => current + ColorToTag(GetElementColor(kv.Key), kv.Key.ToString()) + " +" +
                             (int)(kv.Value * 100) + "%\n");

        tooltips.Add(new TooltipLine(Mod, "WeaponParams", $"{text.Trim()}"));
    }

    public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
    {
        base.ModifyHitNPC(item, player, target, ref modifiers);
        //
        // if (Elements.Count <= 0 || !target.TryGetGlobalNPC(out ElementalNPC npc))
        // {
        //     return;
        // }
        //
        // foreach (var kv in Elements)
        // {
        //     var elementalDamage = (int)(modifiers.FinalDamage.ApplyTo(item.damage) * kv.Value);
        //     npc.ApplyElementalDamage(elementalDamage, kv.Key);
        // }
    }

    private static bool IsWeapon(Item item) =>
        item.DamageType == DamageClass.Melee ||
        item.DamageType == DamageClass.Ranged ||
        item.DamageType == DamageClass.Magic ||
        item.DamageType == DamageClass.Summon ||
        item.DamageType == DamageClass.Throwing; // si usas lanzables clásicos

    public static Color GetElementColor(GlobalType type)
    {
        return type switch
        {
            GlobalType.Fire => Color.Orange,
            GlobalType.Lightning => Color.Cyan,
            GlobalType.Ice => Color.Blue,
            GlobalType.Poison => Color.Green,
            GlobalType.Holy => Color.Yellow,
            GlobalType.Dark => Color.Purple,
            _ => Color.White
        };
    }

    private static string ColorToTag(Color color, string text)
    {
        return $"[c/{color.R:X2}{color.G:X2}{color.B:X2}:{text}]";
    }
}