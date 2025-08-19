using System.Collections.Generic;
using TerraSouls.Items;
using TerraSouls.Items.Rings;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TerraSouls.Enums;
using TerraSouls.Items.Souls;

namespace TerraSouls.Globals;

// ReSharper disable once ClassNeverInstantiated.Global
public class GlobalNpcChanges : GlobalNPC
{
    public override bool InstancePerEntity => true;
    
    // +resistance < 1 ... 0.8 =  80% damage or 20% less damage
    // -resistance > 1 ... 1.2 = 120% damage or 20% more damage 
    private readonly Dictionary<GlobalType, float> _resistances = new()
    {
        { GlobalType.Fire, 1f }, // 0.8 es 20%
        { GlobalType.Lightning, 1f }, // 1.2f es 20% more dmg
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
                var reduction = (1f - multiplier) * 100f;
                return $"Resists {reduction:F0}%";
            }
            case > 1f:
            {
                var extra = (multiplier - 1f) * 100f;
                return $"Receive +{extra:F0}%";
            }
            default:
                return "Normal resistance";
        }
    }

    public override void SetDefaults(NPC npc)
    {
        BleedPower = 0;
        PoisonPower = 0;
        
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
            
            // case NPCID.Zombie:
            //     BleedPower = 10;
            //     PoisonPower = 2;
            //     break;
            //
            // case NPCID.Hornet:
            //     BleedPower = 0;
            //     PoisonPower = 10;
            //     break;
            //
            // case NPCID.GoblinThief:
            //     BleedPower = 5;
            //     PoisonPower = 1;
            //     break;
            
            case NPCID.Retinazer:
                _resistances[GlobalType.Fire] = 0.2f;
                BleedPower = 20;
                break;
            
            case NPCID.Spazmatism:
                _resistances[GlobalType.Poison] = 0.2f;
                PoisonPower = 20;
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

        // if (!item.TryGetGlobalItem<GlobalItemChanges>(out var myGlobalItem) || myGlobalItem.Elements.Count == 0)
        // {
        //     return;
        // }
        //
        // foreach (var (type, percent) in myGlobalItem.Elements)
        // {
        //     if (type == GlobalType.Poison)
        //     {
        //         // TIME / 60 = SECONDS ACTIVE
        //         if (!Main.rand.NextBool(5))
        //         {
        //             // npc.AddBuff(ModContent.BuffType<AcidVenomBuff>(), 300);
        //             npc.AddBuff(BuffID.Poisoned, 300);
        //         }
        //     }
        // }
    }

    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitByProjectile(npc, projectile, hit, damageDone);

        ShowDustOnHit(npc, projectile);
    }

    public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        base.ModifyHitByItem(npc, player, item, ref modifiers);
        
        if (!item.TryGetGlobalItem<GlobalItemChanges>(out var myGlobalItem) || myGlobalItem.Elements.Count == 0)
        {
            return;
        }
        
        ChangeDamageByElements(myGlobalItem.Elements, item.damage, ref modifiers);
    }

    public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        base.ModifyHitByProjectile(npc, projectile, ref modifiers);
        
        if (!projectile.TryGetGlobalProjectile<GlobalProjectileChanges>(out var myGlobalProjectile) || myGlobalProjectile.Elements.Count == 0)
        {
            return;
        }

        ChangeDamageByElements(myGlobalProjectile.Elements, projectile.damage, ref modifiers);
    }

    private void ChangeDamageByElements(Dictionary<GlobalType,float> elements, int damage, ref NPC.HitModifiers modifiers)
    {
        // var currentDamage = modifiers.FinalDamage.ApplyTo(projectile.damage);

        // var debugLines = new List<string>
        // {
        //     $"DEBUG Daño base: {projectile.damage}",
        //     $"DEBUG Daño físico con defensa: {currentDamage:F1}"
        // };

        var elementalExtraDamage = 0f;
        foreach (var (type, percent) in elements)
        {
            elementalExtraDamage += damage * percent * _resistances[type];
            // debugLines.Add($"DEBUG {type.ToString()}: {elementDamageBeforeResist:F1} x resistencia {_resistances[type]:F2} = {elementDamageAfterResist:F1}");
        }

        // var totalDamage = currentDamage + elementalExtraDamage;
        modifiers.FinalDamage += elementalExtraDamage / damage; // Aplicar daño extra al modificador

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

        var color = GlobalItemChanges.GetElementColor(type);
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
            case Item item when item.TryGetGlobalItem<GlobalItemChanges>(out var elemItem) && elemItem.Elements.Count > 0:
                elements = elemItem.Elements;
                break;

            case Projectile projectile when projectile.TryGetGlobalProjectile<GlobalProjectileChanges>(out var elemProj) &&
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

        if (npc.boss || npc.type is NPCID.LunarTowerNebula or NPCID.LunarTowerSolar or NPCID.LunarTowerStardust or NPCID.LunarTowerVortex)
        {
            return;
        }

        // NPC hasn't been damaged by any Player + Souls farming with Statues and friendly NPCs and Boss parts disabled :)
        if (npc.SpawnedFromStatue || npc.friendly || npc.townNPC || npc.lifeMax <= 5 || npc.damage == 0)
        {
            return;
        }

        var souls = GetSoulsByNpc(npc);
        
        for (var i = 0; i < 5; i++)
        {
            var randomOffset = Main.rand.NextVector2Circular(20f, 20f);
            RingSystem.SoulPositions.Add(npc.Center + randomOffset);
        }

        if (Main.dedServ)
        {
            var packet = Mod.GetPacket();
            packet.Write((byte)NetMessageTypes.GetSouls);
            packet.Write(souls);
            if (npc.boss) // server sends souls to all clients (if NPC is downed boss)
            {
                packet.Send();
            }
            else
            {
                packet.Send(playerIndex); // if the client (specific player) kills someone other than boss
            }
        }
        else // single player
        {
            player.AddSouls(souls);
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
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DarkStoneplateRing>(), 500)); // 0.2%
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CovetousGoldSerpentRing>(), 100)); // 1%
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CovetousSilverSerpentRing>(), 100)); // 1%
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LifeRing>(), 100)); // 1%
                break;
            
            case NPCID.Deerclops:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfDeerclops>()));
                break;
            
            case NPCID.SkeletronHead:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfSkeletron>()));
                break;
            
            case NPCID.SkeletronPrime:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfSkeletronPrime>()));
                break;
            
            case NPCID.BrainofCthulhu:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfTheBrainOfCthulhu>()));
                break;
            
            case NPCID.TheDestroyer:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfTheDestroyer>()));
                break;
            
            case NPCID.EyeofCthulhu:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfTheEyeOfCthulhu>()));
                break;
            
            case NPCID.KingSlime:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfTheKingSlime>()));
                break;
            
            case NPCID.QueenBee:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfTheQueenBee>()));
                break;
            
            case NPCID.Spazmatism:
            case NPCID.Retinazer:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfTheTwins>()));
                break;
            
            case NPCID.WallofFlesh:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfTheWallOfFlesh>()));
                break;
            
            case NPCID.EaterofWorldsBody:
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SoulOfTheWorldEater>()));
                break;
        }

        npcLoot.Add(ItemDropRule.ByCondition(new DropsFromNormalEnemiesOnlyCondition(), ModContent.ItemType<Humanity>(), 100)); // 1%
    }

    public override void ModifyShop(NPCShop shop)
    {
        base.ModifyShop(shop);
        
        switch (shop.NpcType)
        {
            case NPCID.Merchant:
                shop.Add<RosterOfKnights>();
                shop.Add<Darksign>();
                break;
        }
    }
    
    private static int GetSoulsByNpc(NPC npc)
    {
        var npcId = npc.type;
        var souls = 0;

        NPC golem = new(); golem.SetDefaults(NPCID.Golem);
        NPC golemFist = new(); golemFist.SetDefaults(NPCID.GolemFistLeft);
        NPC golemHead = new(); golemHead.SetDefaults(NPCID.GolemHead);
        var golemHp = golem.lifeMax + 2 * golemFist.lifeMax + golemHead.lifeMax;

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

                return souls;
            }
            // Eater of Worlds
            case NPCID.EaterofWorldsHead:
            {
                NPC eaterOfWorlds = new();
                eaterOfWorlds.SetDefaults(NPCID.EaterofWorldsHead);
                return NPC.downedBoss2 ? (int)(eaterOfWorlds.lifeMax * 0.1f) : (int)(eaterOfWorlds.lifeMax * 0.5f);
            }
            // Brain of Cthulhu
            case NPCID.BrainofCthulhu:
            {
                NPC brainOfCthulhu = new();
                brainOfCthulhu.SetDefaults(NPCID.BrainofCthulhu);
                souls = brainOfCthulhu.lifeMax;
                return NPC.downedBoss2
                    ? (int)(souls * 0.5f) // 50% of HP (only Boss)
                    : (int)(souls * 2.6f); // 100% of HP (Boss + Creepers = lifeMax * 2.6)
            }
            // QueenBee
            case NPCID.QueenBee:
            {
                NPC queenBee = new();
                queenBee.SetDefaults(NPCID.QueenBee);
                souls = queenBee.lifeMax;
                return NPC.downedQueenBee ? (int)(souls * 0.4f) : (int)(souls * 1.2f);
            }
            // Deerclops
            case NPCID.Deerclops:
            {
                NPC deerclops = new();
                deerclops.SetDefaults(NPCID.Deerclops);
                souls = deerclops.lifeMax;
                return NPC.downedDeerclops ? (int)(souls * 0.25f) : (int)(souls * 0.8f);
            }
            // Skeletron
            case NPCID.SkeletronHead:
            {
                NPC skeletron = new();
                skeletron.SetDefaults(NPCID.SkeletronHead);
                NPC skeletronHand = new();
                skeletronHand.SetDefaults(NPCID.SkeletronHand);
                souls = skeletron.lifeMax + 2 * skeletronHand.lifeMax;
                if (NPC.downedBoss3)
                {
                    souls = (int)(souls * 0.4f);
                }

                return souls;
            }
            // Wall of Flesh
            case NPCID.WallofFleshEye:
            case NPCID.WallofFlesh:
            {
                NPC wallOfFlesh = new();
                wallOfFlesh.SetDefaults(NPCID.WallofFlesh);
                souls = wallOfFlesh.lifeMax;
                if (Main.hardMode)
                {
                    souls = (int)(souls * 0.4f);
                }

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

                return souls;
            }
            // Destroyer
            case NPCID.TheDestroyer:
            {
                NPC destroyer = new();
                destroyer.SetDefaults(NPCID.TheDestroyer);
                return NPC.downedMechBoss1 ? (int)(destroyer.lifeMax * 0.1f) : (int)(destroyer.lifeMax * 0.65f);
            }
            // Twins
            case NPCID.Retinazer:
            case NPCID.Spazmatism:
            {
                // Second is not alive
                if (NPC.AnyNPCs(npcId == NPCID.Retinazer ? NPCID.Spazmatism : NPCID.Retinazer))
                {
                    return souls;
                }
                
                NPC retinazer = new();
                retinazer.SetDefaults(NPCID.Retinazer);
                NPC spazmatism = new();
                spazmatism.SetDefaults(NPCID.Spazmatism);
                souls = retinazer.lifeMax + spazmatism.lifeMax;
                if (NPC.downedMechBoss2)
                {
                    souls = (int)(souls * 0.25f);
                }

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

                return souls;
            }
            // Plantera
            case NPCID.Plantera:
            {
                NPC plantera = new(); plantera.SetDefaults(NPCID.Plantera);
                souls = (int)(plantera.lifeMax * 2.5f);
                if (NPC.downedPlantBoss)
                {
                    souls = (int)(plantera.lifeMax * 0.5f);
                }

                return souls;
            }
            // Golem
            case NPCID.Golem:
            {
                souls = (int)(golemHp * 1.5f); // 90K
                if (NPC.downedGolemBoss)
                {
                    souls = (int)(golemHp * 0.3f);
                }

                return souls;
            }
            // Duke Fishron
            case NPCID.DukeFishron:
            {
                return NPC.downedFishron
                    ? (int)(golemHp * 0.35f) // 21K
                    : (int)(golemHp * 1.5f + 15000); // 105K
            }
            // Empress of Light
            case NPCID.HallowBoss:
            {
                NPC empressOfLight = new();
                empressOfLight.SetDefaults(NPCID.HallowBoss);
                souls = (int)(empressOfLight.lifeMax * 1.5f) + 15000;  // 120K
                if (NPC.downedEmpressOfLight)
                {
                    souls = (int)(empressOfLight.lifeMax * 0.35f); // 24.5K
                }

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

                return souls;
            }
            // Lunar Towers
            case NPCID.LunarTowerNebula:
            case NPCID.LunarTowerSolar:
            case NPCID.LunarTowerStardust:
            case NPCID.LunarTowerVortex:
            {
                NPC tower = new(); tower.SetDefaults(npcId);
                souls = (int)(tower.lifeMax * 1.5f);
                if ((NPC.downedTowerSolar && npcId == NPCID.LunarTowerSolar) ||
                    (NPC.downedTowerNebula && npcId == NPCID.LunarTowerNebula) ||
                    (NPC.downedTowerVortex && npcId == NPCID.LunarTowerVortex) ||
                    (NPC.downedTowerStardust && npcId == NPCID.LunarTowerStardust)
                   )
                {
                    souls = (int)(souls * 0.5f);
                }

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
                
                return NPC.downedMoonlord
                    ? (int)(souls * 0.5f) // 72.5K
                    : (int)(souls * 1.25f); // 181250
            }
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
    
    public int BleedPower;
    public int PoisonPower;
}