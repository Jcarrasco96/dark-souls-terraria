using System.Collections.Generic;
using CustomRecipes.Dusts;
using CustomRecipes.Items;
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

        var playerIndex = npc.lastInteraction;

        if (playerIndex is < 0 or >= Main.maxPlayers)
        {
            return;
        }

        var player = Main.player[playerIndex].GetModPlayer<RingPlayer>();
        if (npc.value > 0)
        {
            player.DropMoney(npc.value);
        }

        var npcId = npc.type;
        if (npc.boss || npcId is NPCID.LunarTowerNebula or NPCID.LunarTowerSolar or NPCID.LunarTowerStardust or NPCID.LunarTowerVortex)
        {
            return;
        }

        if (npc.SpawnedFromStatue || npc.friendly || npc.townNPC || npc.lifeMax <= 5 ||
            npc.damage ==
            0) // NPC hasn't been damaged by any Player + Souls farming with Statues and friendly NPCs and Boss parts disabled :)
        {
            return;
        }

        var souls = GetSoulsByNpc(npc, out var boss);

        // if (Main.dedServ)
        // {
        //     var packet = Mod.GetPacket();
        //     packet.Write((byte)DarkSouls.NetMessageTypes.GetSouls);
        //     packet.Write(souls);
        //     if (boss) // server sends souls to all clients (if NPC is downed boss)
        //     {
        //         packet.Send();
        //     }
        //     else
        //     {
        //         packet.Send(playerIndex); // if the client (specific player) kills someone other than boss
        //     }
        // }
        // else // single player
        // {
        player.AddSouls(souls);
        // }
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

    public override void ModifyShop(NPCShop shop)
    {
        base.ModifyShop(shop);
        
        switch (shop.NpcType)
        {
            case NPCID.Merchant:
                shop.Add<BookStats>();
                break;
        }
    }
    
    private static int GetSoulsByNpc(NPC npc, out bool boss)
    {
        boss = false;
        var npcId = npc.type;
        var souls = 0;
        
        switch (npcId)
        {
            // Pre-Hardmode Bosses
            // King Slime
            case NPCID.KingSlime:
            {
                NPC kingSlime = new();
                kingSlime.SetDefaults(NPCID.KingSlime);
                souls = kingSlime.lifeMax;
                if (NPC.downedSlimeKing)
                {
                    souls = (int)(kingSlime.lifeMax * 0.15f);
                }

                boss = true;
                return souls;
            }
            // Eye of Cthulhu
            case NPCID.EyeofCthulhu:
            {
                NPC eyeOfCthulhu = new();
                eyeOfCthulhu.SetDefaults(NPCID.EyeofCthulhu);
                souls = eyeOfCthulhu.lifeMax;
                if (NPC.downedBoss1)
                {
                    souls = (int)(eyeOfCthulhu.lifeMax * 0.15f);
                }

                boss = true;
                return souls;
            }
            // Eater of Worlds
            case NPCID.EaterofWorldsHead:
            {
                NPC eaterOfWorlds = new();
                eaterOfWorlds.SetDefaults(NPCID.EaterofWorldsHead);
                souls = NPC.downedBoss2 ? (int)(eaterOfWorlds.lifeMax * 0.1f) : (int)(eaterOfWorlds.lifeMax * 0.5f);
                boss = true;
                return souls;
            }
            // Brain of Cthulhu
            case NPCID.BrainofCthulhu:
            {
                NPC brainOfCthulhu = new();
                brainOfCthulhu.SetDefaults(NPCID.BrainofCthulhu);
                souls = brainOfCthulhu.lifeMax;
                souls = NPC.downedBoss2
                    ? (int)(souls * 0.5f)
                    : (int)(souls * 2.6f); // 50% of HP (only Boss) or 100% of HP (Boss + Creepers = lifeMax * 2.6)

                boss = true;
                return souls;
            }
            // QueenBee
            case NPCID.QueenBee:
            {
                NPC queenBee = new();
                queenBee.SetDefaults(NPCID.QueenBee);
                souls = queenBee.lifeMax;
                souls = NPC.downedQueenBee ? (int)(souls * 0.4f) : (int)(souls * 1.2f);
                boss = true;
                return souls;
            }
            // Deerclops
            case NPCID.Deerclops:
            {
                NPC deerclops = new();
                deerclops.SetDefaults(NPCID.Deerclops);
                souls = deerclops.lifeMax;
                souls = NPC.downedDeerclops ? (int)(souls * 0.25f) : (int)(souls * 0.8f);
                boss = true;
                return souls;
            }
            // Skeletron
            case NPCID.SkeletronHead:
            {
                NPC skeletron = new();
                skeletron.SetDefaults(NPCID.SkeletronHead);
                NPC skeletronHand = new();
                skeletron.SetDefaults(NPCID.SkeletronHand);
                souls = skeletron.lifeMax + 2 * skeletronHand.lifeMax;
                if (NPC.downedBoss3)
                {
                    souls = (int)(souls * 0.4f);
                }

                boss = true;
                return souls;
            }
            case NPCID.WallofFleshEye:
            // Wall of Flesh
            case NPCID.WallofFlesh:
            {
                NPC wallOfFlesh = new();
                wallOfFlesh.SetDefaults(NPCID.WallofFlesh);
                souls = wallOfFlesh.lifeMax;
                if (Main.hardMode)
                {
                    souls = (int)(souls * 0.4f);
                }

                boss = true;
                return souls;
            }
            // Hardmode Bosses
            // Queen Slime
            case NPCID.QueenSlimeBoss:
            {
                NPC queenSlime = new();
                queenSlime.SetDefaults(NPCID.QueenSlimeBoss);
                souls = queenSlime.lifeMax;
                if (NPC.downedQueenSlime)
                {
                    souls = (int)(souls * 0.3f);
                }

                boss = true;
                return souls;
            }
            // Destroyer
            case NPCID.TheDestroyer:
            {
                NPC destroyer = new();
                destroyer.SetDefaults(NPCID.TheDestroyer);
                souls = destroyer.lifeMax;
                souls = NPC.downedMechBoss1 ? (int)(souls * 0.1f) : (int)(souls * 0.65f);
                boss = true;
                return souls;
            }
            case NPCID.Retinazer:
            // Twins
            case NPCID.Spazmatism:
            {
                if (NPC.AnyNPCs(npcId == NPCID.Retinazer ? NPCID.Spazmatism : NPCID.Retinazer))
                {
                    return souls;
                }

                // Second is not alive
                NPC retinazer = new();
                retinazer.SetDefaults(NPCID.Retinazer);
                NPC spazmatism = new();
                spazmatism.SetDefaults(NPCID.Spazmatism);
                souls = retinazer.lifeMax + spazmatism.lifeMax;
                if (NPC.downedMechBoss2)
                {
                    souls = (int)(souls * 0.25f);
                }

                boss = true;
                return souls;
            }
            // Skeletron Prime
            case NPCID.SkeletronPrime:
            {
                NPC primeCannon = new();
                primeCannon.SetDefaults(NPCID.PrimeCannon);
                NPC primeLaser = new();
                primeLaser.SetDefaults(NPCID.PrimeLaser);
                NPC primeSaw = new();
                primeSaw.SetDefaults(NPCID.PrimeSaw);
                NPC primeVice = new();
                primeVice.SetDefaults(NPCID.PrimeVice);
                souls = primeCannon.lifeMax + primeLaser.lifeMax + primeSaw.lifeMax + primeVice.lifeMax;
                if (NPC.downedMechBoss3)
                {
                    souls = (int)(souls * 0.25f);
                }

                boss = true;
                return souls;
            }
            // Plantera
            case NPCID.Plantera:
            {
                NPC plantera = new();
                plantera.SetDefaults(NPCID.Plantera);
                souls = (int)(plantera.lifeMax * 2.5f);
                if (NPC.downedPlantBoss)
                {
                    souls = (int)(plantera.lifeMax * 0.5f);
                }

                boss = true;
                return souls;
            }
            // Golem
            case NPCID.Golem:
            {
                NPC golem = new();
                golem.SetDefaults(NPCID.Golem);
                NPC golemFist = new();
                golemFist.SetDefaults(NPCID.GolemFistLeft);
                NPC golemHead = new();
                golemHead.SetDefaults(NPCID.GolemHead);
                var golemHp = golem.lifeMax + 2 * golemFist.lifeMax + golemHead.lifeMax;
                
                souls = (int)(golemHp * 1.5f); // 90K
                if (NPC.downedGolemBoss)
                {
                    souls = (int)(golemHp * 0.3f);
                }

                boss = true;
                return souls;
            }
            // Duke Fishron
            case NPCID.DukeFishron:
            {
                NPC golem = new();
                golem.SetDefaults(NPCID.Golem);
                NPC golemFist = new();
                golemFist.SetDefaults(NPCID.GolemFistLeft);
                NPC golemHead = new();
                golemHead.SetDefaults(NPCID.GolemHead);
                var golemHp = golem.lifeMax + 2 * golemFist.lifeMax + golemHead.lifeMax;
                
                souls = golemHp; // souls in relation to Golem
                souls = NPC.downedFishron ? (int)(souls * 0.35f) : (int)(souls * 1.5f + 15000); // 21K or 105K
                boss = true;
                return souls;
            }
            // Empress of Light
            case NPCID.HallowBoss:
            {
                NPC empressOfLight = new();
                empressOfLight.SetDefaults(NPCID.HallowBoss);
                souls = (int)(empressOfLight.lifeMax * 1.5f) + 15000; // 120K
                if (NPC.downedEmpressOfLight)
                {
                    souls = (int)(empressOfLight.lifeMax * 0.35f); // 24.5K
                }

                boss = true;
                return souls;
            }
            // Cultist
            case NPCID.CultistBoss:
            {
                NPC empressOfLight = new();
                empressOfLight.SetDefaults(NPCID.HallowBoss); // souls in relation to Empress of Light
                souls = (int)(empressOfLight.lifeMax * 1.5f) + 30000; // 135K
                if (NPC.downedAncientCultist)
                {
                    souls = (int)(empressOfLight.lifeMax * 0.4f); // 28K
                }

                boss = true;
                return souls;
            }
            case NPCID.LunarTowerNebula:
            case NPCID.LunarTowerSolar:
            case NPCID.LunarTowerStardust:
            // Lunar Towers
            case NPCID.LunarTowerVortex:
            {
                NPC tower = new();
                tower.SetDefaults(npcId);
                souls = (int)(tower.lifeMax * 1.5f);
                if ((NPC.downedTowerSolar && npcId == NPCID.LunarTowerSolar) ||
                    (NPC.downedTowerNebula && npcId == NPCID.LunarTowerNebula) ||
                    (NPC.downedTowerVortex && npcId == NPCID.LunarTowerVortex) ||
                    (NPC.downedTowerStardust && npcId == NPCID.LunarTowerStardust)
                   )
                {
                    souls = (int)(souls * 0.5f);
                }

                boss = true;
                return souls;
            }
            // Moon Lord
            case NPCID.MoonLordCore:
            {
                NPC moonLordCore = new();
                moonLordCore.SetDefaults(NPCID.MoonLordCore);
                NPC moonLordHead = new();
                moonLordHead.SetDefaults(NPCID.MoonLordHead);
                NPC moonLordHand = new();
                moonLordHand.SetDefaults(NPCID.MoonLordHand);
                souls = moonLordCore.lifeMax + moonLordHead.lifeMax + 2 * moonLordHand.lifeMax;
                souls = NPC.downedMoonlord ? (int)(souls * 0.5f) : (int)(souls * 1.25f); // 72.5K or 181250

                boss = true;
                return souls;
            }
            // bosses that have not been manually handled.
            default:
                boss = npc.boss;
                break;
        }

        // Blacklist
        if (NpcIdBlackList.Contains(npcId))
        {
            return 0;
        }

        // Other NPCs
        npc.GetLifeStats(out _, out var maxStatLife);
        return maxStatLife;
    }

    private static readonly HashSet<int> NpcIdBlackList =
    [
        NPCID.EaterofWorldsBody,
        NPCID.EaterofWorldsTail,
        NPCID.EaterofWorldsHead,

        NPCID.BrainofCthulhu,
        NPCID.Creeper,

        NPCID.KingSlime,
        NPCID.SlimeSpiked,

        NPCID.EyeofCthulhu,
        NPCID.ServantofCthulhu,
        NPCID.QueenBee,

        NPCID.SkeletronHand,
        NPCID.SkeletronHead,

        NPCID.WallofFlesh,
        NPCID.WallofFleshEye,
        NPCID.TheHungry,
        NPCID.TheHungryII,
        NPCID.LeechBody,
        NPCID.LeechHead,
        NPCID.LeechTail,

        NPCID.QueenSlimeBoss,
        NPCID.QueenSlimeMinionBlue,
        NPCID.QueenSlimeMinionPink,
        NPCID.QueenSlimeMinionPurple,

        NPCID.Retinazer,
        NPCID.Spazmatism,

        NPCID.TheDestroyer,
        NPCID.TheDestroyerBody,
        NPCID.TheDestroyerTail,
        NPCID.Probe,

        NPCID.SkeletronPrime,
        NPCID.PrimeCannon,
        NPCID.PrimeLaser,
        NPCID.PrimeSaw,
        NPCID.PrimeVice,

        NPCID.Plantera,
        NPCID.PlanterasTentacle,

        NPCID.Golem,
        NPCID.GolemFistLeft,
        NPCID.GolemFistRight,
        NPCID.GolemHead,
        NPCID.GolemHeadFree,

        NPCID.DukeFishron,
        NPCID.Sharkron,
        NPCID.Sharkron2,

        NPCID.HallowBoss,

        NPCID.CultistBoss,
        NPCID.CultistBossClone,
        NPCID.CultistDragonBody1,
        NPCID.CultistDragonBody2,
        NPCID.CultistDragonBody3,
        NPCID.CultistDragonBody4,
        NPCID.CultistDragonHead,
        NPCID.CultistDragonTail,

        NPCID.SolarCrawltipedeBody,
        NPCID.SolarCrawltipedeTail,

        NPCID.LunarTowerSolar,
        NPCID.LunarTowerVortex,
        NPCID.LunarTowerNebula,
        NPCID.LunarTowerStardust,

        NPCID.StardustCellSmall,

        NPCID.MoonLordCore,
        NPCID.MoonLordFreeEye,
        NPCID.MoonLordHand,
        NPCID.MoonLordHead,
        NPCID.MoonLordLeechBlob
    ];
}