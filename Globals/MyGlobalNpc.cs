using System.Collections.Generic;
using CustomRecipes.Rings;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.Globals;

public class MyGlobalNpc : GlobalNPC
{
    public override bool InstancePerEntity => true;

    // Resistencias o debilidades por tipo
    // +resistance < 1 ... 0.8 =  80% damage or 20% less damage
    // -resistance > 1 ... 1.2 = 120% damage or 20% more damage 
    private readonly Dictionary<GlobalType, float> _resistances = new()
    {
        { GlobalType.Fire, 1f }, // 0.8 es 20% resistencia al fuego
        { GlobalType.Lightning, 1f }, // 1.2f es 20% más de daño por rayo
        { GlobalType.Ice, 1f },
        { GlobalType.Poison, 1f },
        { GlobalType.Holy, 1f },
        { GlobalType.Dark, 1f }
    };

    public override void SetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        base.SetBestiary(npc, database, bestiaryEntry);

        var elementalDef =
            $"Fire: {FormatResistance(GlobalType.Fire)}\n" +
            $"Lightning: {FormatResistance(GlobalType.Lightning)}\n" +
            $"Ice: {FormatResistance(GlobalType.Ice)}\n" +
            $"Poison: {FormatResistance(GlobalType.Poison)}\n" +
            $"Holy: {FormatResistance(GlobalType.Holy)}\n" +
            $"Dark: {FormatResistance(GlobalType.Dark)}";

        bestiaryEntry.Info.AddRange([
            new FlavorTextBestiaryInfoElement(elementalDef)
        ]);
    }

    private string FormatResistance(GlobalType type)
    {
        var multiplier = _resistances[type];
        switch (multiplier)
        {
            case < 1f:
            {
                // Menos de 1 = reduce daño
                var reduction = (1f - multiplier) * 100f;
                return $"Resiste {reduction:F0}%";
            }
            case > 1f:
            {
                // Más de 1 = recibe más daño
                var extra = (multiplier - 1f) * 100f;
                return $"Recibe +{extra:F0}% de daño";
            }
            default:
                return "Sin resistencia";
        }
    }

    public override void SetDefaults(NPC npc)
    {
        switch (npc.type)
        {
            case NPCID.FireImp or NPCID.LavaSlime:
                _resistances[GlobalType.Fire] = 0.5f;
                _resistances[GlobalType.Ice] = 1.5f;
                break;
            case NPCID.IceGolem:
                _resistances[GlobalType.Ice] = 0.5f;
                _resistances[GlobalType.Fire] = 1.5f;
                break;
        }

        if (npc.boss)
        {
            _resistances[GlobalType.Fire] = 0.5f;
            _resistances[GlobalType.Lightning] = 0.5f;
            _resistances[GlobalType.Ice] = 0.5f;
            _resistances[GlobalType.Poison] = 0.5f;
            _resistances[GlobalType.Holy] = 0.5f;
            _resistances[GlobalType.Dark] = 0.5f;
        }
    }

    public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitByItem(npc, player, item, hit, damageDone);

        ShowDustOnHit(npc, item);

        if (!item.TryGetGlobalItem<MyGlobalItem>(out var myGlobalItem) || myGlobalItem.Elements.Count == 0)
        {
            return;
        }

        foreach (var (type, percent) in myGlobalItem.Elements)
        {
            if (type == GlobalType.Poison)
            {
                // TIME / 60 = SECONDS ACTIVE
                // if (!Main.rand.NextBool(5))
                // {
                // npc.AddBuff(ModContent.BuffType<AcidVenomBuff>(), 300);
                npc.AddBuff(BuffID.Poisoned, 300);
                // }
            }
        }
    }

    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitByProjectile(npc, projectile, hit, damageDone);

        ShowDustOnHit(npc, projectile);
    }

    public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        if (!item.TryGetGlobalItem<MyGlobalItem>(out var myGlobalItem) || myGlobalItem.Elements.Count == 0)
        {
            return;
        }

        var currentDamage = modifiers.FinalDamage.ApplyTo(item.damage);

        var debugLines = new List<string>
        {
            $"DEBUG Daño base: {item.damage}",
            $"DEBUG Daño físico con defensa: {currentDamage:F1}"
        };

        var elementalExtraDamage = 0f;
        foreach (var (type, percent) in myGlobalItem.Elements)
        {
            elementalExtraDamage += item.damage * percent * _resistances[type];
            debugLines.Add(
                $"DEBUG {type.ToString()}: {(item.damage * percent):F1} x resistencia {_resistances[type]:F2} = {(item.damage * percent * _resistances[type]):F1}");
        }

        var totalDamage = currentDamage + elementalExtraDamage;
        modifiers.FinalDamage += elementalExtraDamage / item.damage; // Aplicar daño extra al modificador

        debugLines.Add($"DEBUG Daño elemental total ajustado: {elementalExtraDamage:F1}");
        debugLines.Add($"DEBUG Daño total final estimado: {totalDamage:F1}");

        foreach (var line in debugLines)
        {
            Main.NewText(line, Color.YellowGreen);
        }
    }

    public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        //base.ModifyHitByProjectile(npc, projectile, ref modifiers);

        if (!projectile.TryGetGlobalProjectile<MyGlobalProjectile>(out var myGlobalProjectile) ||
            myGlobalProjectile.Elements.Count == 0)
        {
            return;
        }

        // var currentDamage = modifiers.FinalDamage.ApplyTo(projectile.damage);

        // var debugLines = new List<string>
        // {
        //     $"DEBUG Daño base: {projectile.damage}",
        //     $"DEBUG Daño físico con defensa: {currentDamage:F1}"
        // };

        var elementalExtraDamage = 0f;
        foreach (var (type, percent) in myGlobalProjectile.Elements)
        {
            elementalExtraDamage += projectile.damage * percent * _resistances[type];
            // debugLines.Add($"DEBUG {type.ToString()}: {elementDamageBeforeResist:F1} x resistencia {_resistances[type]:F2} = {elementDamageAfterResist:F1}");
        }

        // var totalDamage = currentDamage + elementalExtraDamage;
        modifiers.FinalDamage += elementalExtraDamage / projectile.damage; // Aplicar daño extra al modificador

        // debugLines.Add($"DEBUG Daño elemental total ajustado: {elementalExtraDamage:F1}");
        // debugLines.Add($"DEBUG Daño total final estimado: {totalDamage:F1}");

        // foreach (var line in debugLines)
        // {
        //     Main.NewText(line, Color.LightCyan);
        // }
    }

    private static int ShowDust(GlobalType type, Vector2 position, float width, float height)
    {
        int dustType;

        switch (type)
        {
            case GlobalType.Fire:
                dustType = DustID.GemRuby;
                break;
            case GlobalType.Lightning:
                dustType = DustID.GemDiamond;
                break;
            case GlobalType.Ice:
                dustType = DustID.GemSapphire;
                break;
            case GlobalType.Poison:
                dustType = DustID.GemEmerald;
                break;
            case GlobalType.Holy:
                dustType = DustID.GemTopaz;
                break;
            case GlobalType.Dark:
                dustType = DustID.GemAmethyst;
                break;
            case GlobalType.None:
            default:
                return -1;
        }

        var color = MyGlobalItem.GetElementColor(type);
        var dustId = Dust.NewDust(position, (int)width, (int)height, dustType, 0f, 0f, 150, color);

        if (dustId < 0)
        {
            return dustId;
        }

        Main.dust[dustId].noGravity = true;
        Main.dust[dustId].velocity *= 1.2f;

        return dustId;
    }

    private static void ShowDustOnHit(NPC npc, object o)
    {
        Dictionary<GlobalType, float> elements;

        switch (o)
        {
            case Item item when item.TryGetGlobalItem<MyGlobalItem>(out var elemItem) && elemItem.Elements.Count > 0:
                elements = elemItem.Elements;
                break;

            case Projectile projectile when projectile.TryGetGlobalProjectile<MyGlobalProjectile>(out var elemProj) &&
                                            elemProj.Elements.Count > 0:
                elements = elemProj.Elements;
                break;

            default:
                return;
        }

        var hitbox = npc.Hitbox;

        foreach (var (type, percent) in elements)
        {
            var dustCount = (int)(5 + percent * 10); // Ej: 0.3f -> 8 partículas, 1.0f -> 15

            for (var i = 0; i < dustCount; i++)
            {
                var dustId = ShowDust(type, hitbox.Location.ToVector2(), hitbox.Width, hitbox.Height);

                if (dustId < 0)
                {
                    continue;
                }

                Main.dust[dustId].scale = 1f + percent * 0.5f;
                Main.dust[dustId].fadeIn = 0.8f + percent;
            }
        }
    }

    // private const int DamageInterval = 30; // ticks
    // private int _acidVenomCounter;
    //
    // public override void AI(NPC npc)
    // {
    //     if (npc.HasBuff(ModContent.BuffType<AcidVenomBuff>()))
    //     {
    //         // Partículas verdes aciditas que salen del NPC
    //         if (Main.rand.NextBool(4)) // Aproximadamente 1 cada 4 ticks
    //         {
    //             var position = npc.Center + new Vector2(Main.rand.NextFloat(-npc.width / 2f, npc.width / 2f), Main.rand.NextFloat(-npc.height / 2f, npc.height / 2f));
    //             var dustIndex = Dust.NewDust(position, 0, 0, DustID.GreenTorch, 0f, 0f, 100, default, 1.2f);
    //             if (dustIndex >= 0)
    //             {
    //                 Main.dust[dustIndex].noGravity = true;
    //                 Main.dust[dustIndex].velocity *= 0.3f;
    //                 Main.dust[dustIndex].fadeIn = 1f;
    //             }
    //         }
    //         
    //         _acidVenomCounter++;
    //
    //         if (_acidVenomCounter < DamageInterval)
    //         {
    //             return;
    //         }
    //
    //         _acidVenomCounter = 0;
    //         npc.SimpleStrikeNPC(10, 0, false, 0f, DamageClass.Default);
    //
    //         // npc.ai[0]++; // Usa ai[0] como contador de ticks
    //         //
    //         // if (!(npc.ai[0] >= 30))
    //         // {
    //         //     return;
    //         // }
    //         //
    //         // // Cada medio segundo (30 ticks)
    //         // npc.ai[0] = 0;
    //         // npc.SimpleStrikeNPC(10, 0, false, 0f, DamageClass.Default);
    //     }
    //     else
    //     {
    //         _acidVenomCounter = 0;
    //         // npc.ai[0] = 0; // Resetear contador si no tiene el buff
    //     }
    // }

    public override void OnKill(NPC npc)
    {
        base.OnKill(npc);

        if (npc.lastInteraction is < 0 or >= Main.maxPlayers)
        {
            return;
        }

        if (npc.value > 0)
        {
            Main.player[npc.lastInteraction].GetModPlayer<RingPlayer>().DropMoney(npc.value);
        }
    }

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        base.ModifyNPCLoot(npc, npcLoot);

        // PROBABILITY = 100 / chanceDenominator, ex: 100 / 500 = 0.2%, 100 / 50 = 2%
        switch (npc.type)
        {
            case NPCID.GiantTortoise:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ChloranthyRing>(), 12)); // 8.33%
                break;
            case NPCID.Zombie:
            case NPCID.ZombieRaincoat:
            case NPCID.BigRainZombie:
            case NPCID.SmallRainZombie:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DarkStoneplateRing>(), 500));
                break;
        }

        //else // Humanity
        //{
        //npcLoot.Add(ItemDropRule.ByCondition(new DropsFromNormalEnemiesOnlyCondition(), ModContent.ItemType<Humanity>(), 100)); // 1%
        //}
    }

    // public override void ModifyShop(NPCShop shop)
    // {
    // switch (shop.NpcType)
    // {
    //     case NPCID.Merchant:
    //         shop.Add<RingofSteelProtection>();
    //         break;
    // }
    // }
}