using System;
using System.Collections.Generic;
using System.Linq;
using TerraSouls.Commons;
using TerraSouls.Globals;
using TerraSouls.Items;
using TerraSouls.Projectiles;
using TerraSouls.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TerraSouls.Enums;

namespace TerraSouls;

// ReSharper disable once ClassNeverInstantiated.Global
public class RingPlayer : ModPlayer
{
    public readonly Item[] RingSlots = new Item[4];
    // public Item RuneSlot;

    // public override void Load()
    // {
    // base.Load();

    // On_Player.DropItems += (orig, player) =>
    // {
    //     if (player.dead)
    //     {
    //         return;
    //     }
    //
    //     orig(player);
    // };
    //
    // On_Player.DropCoins += (orig, player) => player.dead ? 0L : orig(player);
    // }

    public override void Initialize()
    {
        for (var i = 0; i < 4; i++)
        {
            RingSlots[i] = new Item();
            RingSlots[i].TurnToAir();
        }

        // RuneSlot = new Item();
        // RuneSlot.TurnToAir();
        
        _playerStats = new Dictionary<PlayerStatsLevel, int>()
        {
            { PlayerStatsLevel.PlayerVitality, 0 },
            { PlayerStatsLevel.PlayerAttunement, 0 },
            { PlayerStatsLevel.PlayerStrength, 0 },
            { PlayerStatsLevel.PlayerDexterity, 0 },
            { PlayerStatsLevel.PlayerIntelligence, 0 },
            { PlayerStatsLevel.PlayerFaith, 0 },
            { PlayerStatsLevel.PlayerResistance, 0 },
        };
        
        // _levelTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Level", AssetRequestMode.ImmediateLoad).Value;
    }

    public override void SaveData(TagCompound tag)
    {
        tag["RingSlots"] = RingSlots.Select(ItemIO.Save).ToList();
        // tag["RuneSlot"] = ItemIO.Save(RuneSlot);
        
        tag["dsSouls"] = Souls;
        tag["dsVitality"] = _playerStats[PlayerStatsLevel.PlayerVitality];
        tag["dsAttunement"] = _playerStats[PlayerStatsLevel.PlayerAttunement];
        tag["dsResistance"] = _playerStats[PlayerStatsLevel.PlayerResistance];
        // tag["dsEndurance"] = dsEndurance;
        tag["dsStrength"] = _playerStats[PlayerStatsLevel.PlayerStrength];
        tag["dsDexterity"] = _playerStats[PlayerStatsLevel.PlayerDexterity];
        tag["dsIntelligence"] = _playerStats[PlayerStatsLevel.PlayerIntelligence];
        tag["dsFaith"] = _playerStats[PlayerStatsLevel.PlayerFaith];
        tag["dsHumanity"] = Humanity;
        
        if (Bloodstains.Count > 0)
        {
            tag["bloodstains"] = Bloodstains.Select(x => x.ToTag()).ToList();
        }
    }

    public override void LoadData(TagCompound tag)
    {
        var loadedItems = tag.GetList<TagCompound>("RingSlots");
        for (var i = 0; i < RingSlots.Length; i++)
        {
            RingSlots[i] = i < loadedItems.Count ? ItemIO.Load(loadedItems[i]) : new Item();
        }
        // RuneSlot = new Item();
        // if (tag.ContainsKey("RuneSlot"))
        // {
        //     RuneSlot = ItemIO.Load(tag.GetCompound("RuneSlot"));
        // }
        
        if (tag.ContainsKey("dsSouls"))
        {
            var rawSouls = tag["dsSouls"];
            Souls = rawSouls switch
            {
                long l => l,
                int i => i,
                _ => 0
            };
        }
        if (tag.ContainsKey("dsVitality"))
        {
            _playerStats[PlayerStatsLevel.PlayerVitality] = tag.GetInt("dsVitality");
        }
        if (tag.ContainsKey("dsAttunement"))
        {
            _playerStats[PlayerStatsLevel.PlayerAttunement] = tag.GetInt("dsAttunement");
        }
        if (tag.ContainsKey("dsResistance"))
        {
            _playerStats[PlayerStatsLevel.PlayerResistance] = tag.GetInt("dsResistance");
        }
        // if (tag.ContainsKey("dsEndurance"))
        // {
        //     dsEndurance = tag.GetInt("dsEndurance");
        // }
        if (tag.ContainsKey("dsStrength"))
        {
            _playerStats[PlayerStatsLevel.PlayerStrength] = tag.GetInt("dsStrength");
        }
        if (tag.ContainsKey("dsDexterity"))
        {
            _playerStats[PlayerStatsLevel.PlayerDexterity] = tag.GetInt("dsDexterity");
        }
        if (tag.ContainsKey("dsIntelligence"))
        {
            _playerStats[PlayerStatsLevel.PlayerIntelligence] = tag.GetInt("dsIntelligence");
        }
        if (tag.ContainsKey("dsFaith"))
        {
            _playerStats[PlayerStatsLevel.PlayerFaith] = tag.GetInt("dsFaith");
        }
        if (tag.ContainsKey("dsHumanity"))
        {
            Humanity = tag.GetInt("dsHumanity");
        }
        if (tag.ContainsKey("bloodstains"))
        {
            Bloodstains = tag.GetList<TagCompound>("bloodstains").Select(Bloodstain.FromTag).ToList();
        }
    }

    public override void ResetEffects()
    {
        _customLuckBonus = 0f;
        _customStrKnightsRing = 0;
        _customDexHuntersRing = 0;
        _customIntScholarRing = 0;
        _customFaiPriestessRing = 0;
        
        HasSerpentRingEffect = false;
        HasCovetousGoldSerpentRingEffect = false;
        HasKnightsRingEffect = false;
        HasHuntersRingEffect = false;
        HasScholarRingEffect = false;
        HasPriestessRingEffect = false;
        
        HasBellowingDragoncrestRingEffect = false;
        HasYoungDragonRingEffect = false;
        
        HasHornetRingEffect = false;
        
        HasDuskCrownRingEffect = false;
        
        // StatusEffects.Clear();

        foreach (var ring in RingSlots)
        {
            if (!ring.IsAir && ring.ModItem is ModRing modRing)
            {
                modRing.UpdateAccessory(Player, false);
            }
        }

        if (HasCovetousGoldSerpentRingEffect)
        {
            _customLuckBonus += 0.2f;
        }
        
        if (HasKnightsRingEffect)
        {
            _customStrKnightsRing = 5;
        }
        if (HasHuntersRingEffect)
        {
            _customDexHuntersRing += 5;
        }
        if (HasScholarRingEffect)
        {
            _customIntScholarRing += 5;
        }
        if (HasPriestessRingEffect)
        {
            _customFaiPriestessRing += 5;
        }

        // Player.GetDamage(DamageClass.Melee) += Strength * 0.02f; // +2% por punto de fuerza
        // Player.statDefense += Dexterity / 2; // +0.5 defensa por punto de destreza
        // Player.GetDamage(DamageClass.Magic) += Intelligence * 0.03f; // +3% daño mágico por inteligencia
    }

    public override void OnEnterWorld()
    {
        base.OnEnterWorld();
        
        BossUiManager.ShowMessage("Welcome, don't forget to equip your rings.", new Color(248, 197, 77));
        
        var lostSoul = Bloodstains.FirstOrDefault(s => s.WorldGuid == Main.ActiveWorldFileData.UniqueId.ToString());
        if (lostSoul != null && Main.myPlayer == Player.whoAmI)
        {
            _currentBloodstainProjectile = Projectile.NewProjectile(Player.GetSource_Death(), lostSoul.Position, Vector2.Zero, ModContent.ProjectileType<BloodstainProjectile>(), 0, 0f, Player.whoAmI);
        }
    }
    
    public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
    {
        base.ModifyManaCost(item, ref reduce, ref mult);
        
        if (HasDuskCrownRingEffect && item.DamageType == DamageClass.Magic)
        {
            mult *= 0.75f; // -25%
        }
    }

    public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
    {
        base.ModifyWeaponDamage(item, ref damage);
        
        if (HasBellowingDragoncrestRingEffect && item.DamageType == DamageClass.Magic)
        {
            damage *= 1.20f; // +20%
        }
        
        if (HasYoungDragonRingEffect && item.DamageType == DamageClass.Magic)
        {
            damage *= 1.12f; // +12%
        }
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        base.ModifyHitNPC(target, ref modifiers);
        
        if (HasHornetRingEffect)
        {
            modifiers.CritDamage *= 1.3f;
        }
    }

    public override void ModifyLuck(ref float luck)
    {
        luck += Math.Clamp(Humanity / 100f, 0f, 0.5f);
        luck += _customLuckBonus;
    }

    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
        base.Kill(damage, hitDirection, pvp, damageSource);

        if (Main.myPlayer != Player.whoAmI)
        {
            return;
        }
        
        ModContent.GetInstance<RingSystem>()?.ShowDeathUi();
            
        // Creating a bloodstain (local only)
        if (_currentBloodstainProjectile != -1 && Main.projectile[_currentBloodstainProjectile].active && Main.projectile[_currentBloodstainProjectile].type == ModContent.ProjectileType<BloodstainProjectile>())
        {
            Main.projectile[_currentBloodstainProjectile].Kill();
            _currentBloodstainProjectile = -1;
        }

        Bloodstains.RemoveAll(x => x.WorldGuid == Main.ActiveWorldFileData.UniqueId.ToString());

        if (Souls <= 0 && Humanity <= 0)
        {
            return;
        }

        Bloodstains.Add(new Bloodstain(Player.Center, Humanity, Souls));
        Souls = 0;
        Humanity = 0;
        _currentBloodstainProjectile = Projectile.NewProjectile(Player.GetSource_Death(), Player.Center, Vector2.Zero, ModContent.ProjectileType<BloodstainProjectile>(), 0, 0f, Player.whoAmI);
    }

    public override void UpdateDead()
    {
        base.UpdateDead();
        
        // StatusEffects.Clear();
    }

    public override void PreUpdate()
    {
        base.PreUpdate();
        
        // var sys = ModContent.GetInstance<RingSystem>();
        // if (sys.UiVisible)
        // {
        //     // Bloquear movimiento y acciones
        //     Player.controlLeft = false;
        //     Player.controlRight = false;
        //     Player.controlUp = false;
        //     Player.controlDown = false;
        //     Player.controlJump = false;
        //     Player.controlUseItem = false;
        //     Player.controlUseTile = false;
        //     Player.controlHook = false;
        // }
    }

    public override void OnRespawn()
    {
        base.OnRespawn();

        ModContent.GetInstance<RingSystem>().HideDeathUi();
    }

    public override void PostUpdateBuffs()
    {
        base.PostUpdateBuffs();
        
        var playerBuffs = Player.buffType.ToList();

        foreach (var debuffType in DarkSoulsBuffChanges.TerrariaDebuff)
        {
            var debuffIndex = playerBuffs.FindIndex(x => x == debuffType);
            if (debuffIndex == -1)
            {
                _newDebuffs[debuffType] = true;
                continue;
            }

            var debuffTime = Player.buffTime[debuffIndex];
            if (debuffTime <= 0)
            {
                _newDebuffs[debuffType] = true;
                continue;
            }

            var isNew = _newDebuffs.GetValueOrDefault(debuffType, false);

            if (!isNew)
            {
                continue;
            }

            Player.buffTime[debuffIndex] = (int)(Player.buffTime[debuffIndex] * (1f - StatFormulas.GetDebuffsResistanceByResistance(LevelResistance)));
            _newDebuffs[debuffType] = false;
        }
    }
    
    public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
    {
        base.ModifyHitByNPC(npc, ref modifiers);
        modifiers.FinalDamage *= 1f - StatFormulas.GetDefenseByResistance(LevelResistance);
    }

    public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
    {
        base.ModifyHitByProjectile(proj, ref modifiers);
        modifiers.FinalDamage *= 1f - StatFormulas.GetDefenseByResistance(LevelResistance);
    }

    public override void PostUpdate()
    {
        base.PostUpdate();
        
        Player.ConsumedLifeCrystals = Math.Clamp(StatFormulas.GetLifeByVitality(LevelVitality) / 20, 0, Player.LifeCrystalMax);
        Player.ConsumedLifeFruit = Math.Clamp((StatFormulas.GetLifeByVitality(LevelVitality) - 400) / 5, 0, Player.LifeFruitMax);
        Player.ConsumedManaCrystals = Math.Clamp(StatFormulas.GetManaByAttunement(LevelAttunement) / 20, 0, Player.ManaCrystalMax);

        // Handling click on the location of bloodstain
        if (_currentBloodstainProjectile != -1 && Main.mouseRight && Main.mouseRightRelease && Main.netMode != NetmodeID.Server && IsBloodstainReachable())
        {
            var proj = Main.projectile[_currentBloodstainProjectile];
            var mouseWorld = Main.MouseWorld;
            if (proj.Hitbox.Contains(mouseWorld.ToPoint()))
            {
                TouchBloodstain();
            }
        }
        
        if (BleedMeter > 0f && !Player.HasBuff(BuffID.Bleeding))
        {
            BleedMeter -= 0.02f;
            if (BleedMeter < 0f)
            {
                BleedMeter = 0f;
            }
        }
        
        if (PoisonMeter > 0f && !Player.HasBuff(BuffID.Poisoned))
        {
            PoisonMeter -= 0.02f;
            if (PoisonMeter < 0f)
            {
                PoisonMeter = 0f;
            }
        }

        if (_pendingSouls > 0)
        {
            _soulTimer += 1f / 60f;
            if (_soulTimer >= SoulDelay)
            {
                CombatText.NewText(Player.getRect(), Color.WhiteSmoke, "+" + _pendingSouls);
                // SoundEngine.PlaySound(TerraSouls.DsSoulSuck, Player.position);
                _pendingSouls = 0;
                _soulTimer = 0;
            }
        }
    }
    
    public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
    {
        base.ModifyMaxStats(out health, out mana);
        
        // simulation of used of health, mana crystals and life fruits, for the correct behavior conditions various events
        var totalHealthBonus = StatFormulas.GetLifeByVitality(LevelVitality);
        var consumedLifeCrystals = Math.Clamp(totalHealthBonus / 20, 0, Player.LifeCrystalMax);
        var consumedLifeFruit = Math.Clamp((StatFormulas.GetLifeByVitality(LevelVitality) - 400) / 5, 0, Player.LifeFruitMax);
        var healthBonus = totalHealthBonus - consumedLifeCrystals * 20;

        var totalManaBonus = StatFormulas.GetManaByAttunement(LevelAttunement);
        var consumedManaCrystals = Math.Clamp(totalManaBonus / 20, 0, Player.ManaCrystalMax);
        var manaBonus = totalManaBonus - consumedManaCrystals * 20;

        Player.ConsumedLifeCrystals = consumedLifeCrystals;
        Player.ConsumedManaCrystals = consumedManaCrystals;
        Player.ConsumedLifeFruit = consumedLifeFruit;

        health = StatModifier.Default;
        health.Base = healthBonus;

        mana = StatModifier.Default;
        mana.Base = manaBonus;
    }
    
    private readonly Dictionary<int, bool> _newDebuffs = new();
    
    private long _pendingSouls;
    private float _soulTimer;
    private const float SoulDelay = 1f;

    public List<Bloodstain> Bloodstains = [];
    private int _currentBloodstainProjectile = -1;

    public void AddSouls(int souls, bool render = true)
    {
        if (Main.rand.NextBool(100))
        {
            Humanity++;
        }
        
        if (souls <= 0)
        {
            return;
        }

        Souls += souls;
        if (!render)
        {
            return;
        }

        _pendingSouls += souls;
        _soulTimer = 0f; // reset timer for each NPC kill
    }

    public void DropMoney(float value)
    {
        if (value <= 0f || !HasSerpentRingEffect)
        {
            return;
        }

        var valueAdded = (int)(value * 20 / 100);

        var platine = valueAdded / 1000000;
        var gold = valueAdded % 1000000 / 10000;
        var silver = valueAdded % 10000 / 100;
        var copper = valueAdded % 100;

        if (platine > 0)
        {
            Item.NewItem(Player.GetSource_Loot(), new Vector2(Player.Center.X, Player.Center.Y - 100),
                ItemID.PlatinumCoin, platine);
        }

        if (gold > 0)
        {
            Item.NewItem(Player.GetSource_Loot(), new Vector2(Player.Center.X, Player.Center.Y - 100), ItemID.GoldCoin,
                gold);
        }

        if (silver > 0)
        {
            Item.NewItem(Player.GetSource_Loot(), new Vector2(Player.Center.X, Player.Center.Y - 100),
                ItemID.SilverCoin, silver);
        }

        if (copper > 0)
        {
            Item.NewItem(Player.GetSource_Loot(), new Vector2(Player.Center.X, Player.Center.Y - 100),
                ItemID.CopperCoin, copper);
        }

        //Main.NewText($"¡Dinero +20%! {platine} platino, {gold} oro, {silver} plata, {copper} cobre", Colors.CoinPlatinum);
    }
    
    private void TouchBloodstain()
    {
        if (_currentBloodstainProjectile == -1)
        {
            return;
        }

        var proj = Main.projectile[_currentBloodstainProjectile];

        if (!proj.active || proj.type != ModContent.ProjectileType<BloodstainProjectile>())
        {
            return;
        }

        var bloodstain = Bloodstains.FirstOrDefault(s => s.WorldGuid == Main.ActiveWorldFileData.UniqueId.ToString());
        if (bloodstain == null)
        {
            return;
        }

        Main.NewText(Language.GetText("Mods.TerraSouls.BloodstainMessage").WithFormatArgs(bloodstain.Souls, bloodstain.Humanity).Value, Color.Cyan);
        SoundEngine.PlaySound(TerraSouls.DsNewAreaSound);
        
        BossUiManager.ShowMessage("RETRIEVAL", new Color(64, 108, 120));
        
        proj.Kill();
        _currentBloodstainProjectile = -1;
        Souls += bloodstain.Souls;
        Humanity += bloodstain.Humanity;
        Bloodstains.RemoveAll(x => x.WorldGuid == Main.ActiveWorldFileData.UniqueId.ToString());
    }

    public int RealStr() => LevelStrength + _customStrKnightsRing;

    public int RealDex() => LevelDexterity + _customDexHuntersRing;

    public int RealInt() => LevelIntelligence + _customIntScholarRing;

    public int RealFai() => LevelFaith + _customFaiPriestessRing;

    public int RealResistance() => LevelResistance;

    public void SpawnUsingDarkSign()
    {
        Player.TeleportationPotion();

        Vector2 spawnPosition;

        if (Player.SpawnX != -1 && Player.SpawnY != -1)
        {
            spawnPosition = new Vector2(Player.SpawnX * 16, (Player.SpawnY - 3) * 16);
        }
        else
        {
            spawnPosition = new Vector2(Main.spawnTileX * 16, (Main.spawnTileY - 3) * 16);
        }

        Souls = 0;
        Player.Teleport(spawnPosition, TeleportationStyleID.RodOfDiscord);
    }
    
    public void DecreaseSouls(long reqSoulsToLevelUp)
    {
        if (Souls < reqSoulsToLevelUp)
        {
            return;
        }
        Souls -= reqSoulsToLevelUp;
    }

    public void SetStats(Dictionary<PlayerStatsLevel, int> stats)
    {
        LevelVitality = stats[PlayerStatsLevel.PlayerVitality];
        LevelAttunement = stats[PlayerStatsLevel.PlayerAttunement];
        LevelStrength = stats[PlayerStatsLevel.PlayerStrength];
        LevelDexterity = stats[PlayerStatsLevel.PlayerDexterity];
        LevelIntelligence = stats[PlayerStatsLevel.PlayerIntelligence];
        LevelFaith = stats[PlayerStatsLevel.PlayerFaith];
        LevelResistance = stats[PlayerStatsLevel.PlayerResistance];
    }

    public bool CanUseItem(WeaponParams weaponParams) => RealStr() >= weaponParams.RStr && RealDex() >= weaponParams.RDex && RealInt() >= weaponParams.RInt && RealFai() >= weaponParams.RFai;

    private bool IsBloodstainReachable(float reachDistance = 200f)
    {
        if (_currentBloodstainProjectile == -1)
        {
            return false;
        }

        var proj = Main.projectile[_currentBloodstainProjectile];
        if (!proj.active || proj.type != ModContent.ProjectileType<BloodstainProjectile>())
        {
            return false;
        }

        var distance = Vector2.Distance(Player.Center, proj.Center);
        return distance <= reachDistance;
    }

    #region EQUIPED RINGS AND STATS
    private float _customLuckBonus;

    public bool HasSerpentRingEffect;
    public bool HasCovetousGoldSerpentRingEffect;
    public bool HasKnightsRingEffect;
    public bool HasHuntersRingEffect;
    public bool HasScholarRingEffect;
    public bool HasPriestessRingEffect;
    
    public bool HasBellowingDragoncrestRingEffect;
    public bool HasYoungDragonRingEffect;
    
    public bool HasHornetRingEffect;
    
    public bool HasDuskCrownRingEffect;

    private int _customStrKnightsRing;
    private int _customDexHuntersRing;
    private int _customIntScholarRing;
    private int _customFaiPriestessRing;
    #endregion

    #region PLAYER STATS
    private Dictionary<PlayerStatsLevel, int> _playerStats;

    public int PlayerLevel => LevelVitality + LevelAttunement + LevelStrength + LevelDexterity + LevelIntelligence + LevelFaith + LevelResistance;
    
    public long Souls { get; private set; }

    public int Humanity { get; set; }
    
    public int LevelVitality 
    {
        get => _playerStats[PlayerStatsLevel.PlayerVitality];
        set
        {
            if (value > 99)
            {
                return;
            }
            _playerStats[PlayerStatsLevel.PlayerVitality] = value;
        }
    }

    public int LevelAttunement 
    {
        get => _playerStats[PlayerStatsLevel.PlayerAttunement];
        private set {
            if (value > 99)
            {
                return;
            }
            _playerStats[PlayerStatsLevel.PlayerAttunement] = value;
        }
    }

    public int LevelResistance 
    {
        get => _playerStats[PlayerStatsLevel.PlayerResistance];
        private set {
            if (value > 99)
            {
                return;
            }
            _playerStats[PlayerStatsLevel.PlayerResistance] = value;
            }
    }
    
    public int LevelStrength
    {
        get => _playerStats[PlayerStatsLevel.PlayerStrength];
        private set {
            if (value > 99)
            {
                return;
            }
            _playerStats[PlayerStatsLevel.PlayerStrength] = value;
            }
    }
    
    public int LevelDexterity
    {
        get => _playerStats[PlayerStatsLevel.PlayerDexterity];
        private set {
            if (value > 99)
            {
                return;
            }
            _playerStats[PlayerStatsLevel.PlayerDexterity] = value;
            }
    }
    
    public int LevelIntelligence
    {
        get => _playerStats[PlayerStatsLevel.PlayerIntelligence];
        private set
        {
            if (value > 99)
            {
                return;
            }
            _playerStats[PlayerStatsLevel.PlayerIntelligence] = value;
        }
    }

    public int LevelFaith
    {
        get => _playerStats[PlayerStatsLevel.PlayerFaith];
        private set
        {
            if (value > 99)
            {
                return;
            }
            _playerStats[PlayerStatsLevel.PlayerFaith] = value;
        }
    }

    #endregion
    
    // public readonly List<StatusEffect> StatusEffects = [];
    // private Texture2D _levelTexture;
    
    // public void AddStatusEffect(string name, int duration)
    // {
        // StatusEffects.Add(new StatusEffect
        // {
        //     Name = name,
        //     Texture = _levelTexture,
        //     Duration = duration
        // });
    // }
    
    public float BleedMeter;
    public const float BleedMax = 100f;
    
    public float PoisonMeter;
    public const float PoisonMax = 100f;
    
    private const int BleedResistance = 2;
    private const int PoisonResistance = 3;

    public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
    {
        base.OnHitByNPC(npc, hurtInfo);
        
        var npcAttr = npc.GetGlobalNPC<GlobalNpcChanges>();
        
        Hit(npcAttr.BleedPower, npcAttr.PoisonPower);
    }

    public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
    {
        base.OnHitByProjectile(proj, hurtInfo);
        
        var projAttr = proj.GetGlobalProjectile<GlobalProjectileChanges>();

        Hit(projAttr.BleedPower, projAttr.PoisonPower);
    }

    #region Netcode
    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        var vitalityPacket = Mod.GetPacket();
        vitalityPacket.Write((byte)NetMessageTypes.SyncVitality);
        vitalityPacket.Write((byte)Player.whoAmI);
        vitalityPacket.Write(LevelVitality);
        vitalityPacket.Send(toWho, fromWho);
    }

    public override void CopyClientState(ModPlayer targetCopy)
    {
        base.CopyClientState(targetCopy);
        if (targetCopy is RingPlayer dsPlayer)
        {
            dsPlayer.LevelVitality = LevelVitality;
        }
    }

    public override void SendClientChanges(ModPlayer clientPlayer)
    {
        if (clientPlayer is not RingPlayer dsPlayer || dsPlayer.LevelVitality == LevelVitality)
        {
            return;
        }

        var packet = Mod.GetPacket();
        packet.Write((byte)NetMessageTypes.SyncVitality);
        packet.Write((byte)Player.whoAmI);
        packet.Write(LevelVitality);
        packet.Send();
    }
    #endregion

    private void Hit(float bleedPower, float poisonPower)
    {
        if (/*!Player.HasBuff(BuffID.Bleeding) && */bleedPower > 0)
        {
            var amount = bleedPower - BleedResistance;
            if (amount < 0)
            {
                amount = 0;
            }

            BleedMeter += amount;
            if (BleedMeter >= BleedMax)
            {
                BleedMeter = 0f;
                Player.AddBuff(BuffID.Bleeding, 500);
            }
        }

        if (/*!Player.HasBuff(BuffID.Poisoned) && */poisonPower > 0)
        {
            var amount = poisonPower - PoisonResistance;
            if (amount < 0)
            {
                amount = 0;
            }

            PoisonMeter += amount;
            if (PoisonMeter >= PoisonMax)
            {
                PoisonMeter = 0f;
                Player.AddBuff(BuffID.Poisoned, 500);
            }
        }
    }
}