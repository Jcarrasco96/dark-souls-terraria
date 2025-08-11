using System;
using System.Collections.Generic;
using System.Linq;
using CustomRecipes.Globals;
using CustomRecipes.Projectiles;
using CustomRecipes.Rings;
using CustomRecipes.ScalingParams;
using CustomRecipes.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CustomRecipes;

// ReSharper disable once ClassNeverInstantiated.Global
public class RingPlayer : ModPlayer
{
    public readonly Item[] RingSlots = new Item[4];

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
    }

    public override void SaveData(TagCompound tag)
    {
        tag["RingSlots"] = RingSlots.Select(ItemIO.Save).ToList();
        
        tag["dsSouls"] = DsSouls;
        tag["dsVitality"] = _dsVitality;
        tag["dsAttunement"] = _dsAttunement;
        // tag["dsEndurance"] = dsEndurance;
        tag["dsStrength"] = DsStrength;
        tag["dsDexterity"] = DsDexterity;
        tag["dsResistance"] = DsResistance;
        tag["dsIntelligence"] = DsIntelligence;
        tag["dsFaith"] = DsFaith;
        tag["dsHumanity"] = _dsHumanity;
        
        if (_bloodstains.Count > 0)
        {
            tag["bloodstains"] = _bloodstains.Select(x => x.ToTag()).ToList();
        }
    }

    public override void LoadData(TagCompound tag)
    {
        var loadedItems = tag.GetList<TagCompound>("RingSlots");
        for (var i = 0; i < RingSlots.Length; i++)
        {
            RingSlots[i] = i < loadedItems.Count ? ItemIO.Load(loadedItems[i]) : new Item();
        }
        
        if (tag.ContainsKey("dsSouls"))
        {
            var rawSouls = tag["dsSouls"];
            DsSouls = rawSouls switch
            {
                long l => l,
                int i => i,
                _ => 0
            };
        }
        if (tag.ContainsKey("dsVitality"))
        {
            _dsVitality = tag.GetInt("dsVitality");
        }

        if (tag.ContainsKey("dsAttunement"))
        {
            _dsAttunement = tag.GetInt("dsAttunement");
        }

        // if (tag.ContainsKey("dsEndurance"))
        // {
        //     dsEndurance = tag.GetInt("dsEndurance");
        // }

        if (tag.ContainsKey("dsStrength"))
        {
            DsStrength = tag.GetInt("dsStrength");
        }

        if (tag.ContainsKey("dsDexterity"))
        {
            DsDexterity = tag.GetInt("dsDexterity");
        }
        if (tag.ContainsKey("dsResistance"))
        {
            DsResistance = tag.GetInt("dsResistance");
        }

        if (tag.ContainsKey("dsIntelligence"))
        {
            DsIntelligence = tag.GetInt("dsIntelligence");
        }

        if (tag.ContainsKey("dsFaith"))
        {
            DsFaith = tag.GetInt("dsFaith");
        }
        if (tag.ContainsKey("dsHumanity"))
        {
            _dsHumanity = tag.GetInt("dsHumanity");
        }
        
        if (tag.ContainsKey("bloodstains"))
        {
            _bloodstains = tag.GetList<TagCompound>("bloodstains").Select(Bloodstain.FromTag).ToList();
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
        //Main.NewText("Bienvenido, no olvides equipar tus anillos.", Color.Cyan);
        
        var lostSoul = _bloodstains.FirstOrDefault(s => s.WorldGuid == Main.ActiveWorldFileData.UniqueId.ToString());
        if (lostSoul != null && Main.myPlayer == Player.whoAmI)
        {
            _currentBloodstainProjectile = Projectile.NewProjectile(Player.GetSource_Death(), lostSoul.Position, Vector2.Zero, ModContent.ProjectileType<BloodstainProjectile>(), 0, 0f, Player.whoAmI);
        }
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

    public override void ModifyLuck(ref float luck)
    {
        // luck += Math.Clamp(_dsHumanity / 100f, 0f, 0.5f);
        luck += _customLuckBonus; // Aplica el bonus
    }

    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
        base.Kill(damage, hitDirection, pvp, damageSource);

        if (Main.myPlayer != Player.whoAmI)
        {
            return;
        }

        ModContent.GetInstance<RingSystem>().ShowDeathUi();
            
        // Creating a bloodstain (local only)
        if (_currentBloodstainProjectile != -1 && Main.projectile[_currentBloodstainProjectile].active && Main.projectile[_currentBloodstainProjectile].type == ModContent.ProjectileType<BloodstainProjectile>())
        {
            Main.projectile[_currentBloodstainProjectile].Kill();
            _currentBloodstainProjectile = -1;
        }

        _bloodstains.RemoveAll(x => x.WorldGuid == Main.ActiveWorldFileData.UniqueId.ToString());

        if (DsSouls <= 0 && _dsHumanity <= 0)
        {
            return;
        }

        _bloodstains.Add(new Bloodstain(Player.Center, _dsHumanity, DsSouls, Player));
        DsSouls = 0;
        _dsHumanity = 0;
        _currentBloodstainProjectile = Projectile.NewProjectile(Player.GetSource_Death(), Player.Center, Vector2.Zero, ModContent.ProjectileType<BloodstainProjectile>(), 0, 0f, Player.whoAmI);
    }

    public override void PreUpdate()
    {
        base.PreUpdate();
        
        var sys = ModContent.GetInstance<MyUiSystem>();
        if (sys.UiVisible)
        {
            // Bloquear movimiento y acciones
            Player.controlLeft = false;
            Player.controlRight = false;
            Player.controlUp = false;
            Player.controlDown = false;
            Player.controlJump = false;
            Player.controlUseItem = false;
            Player.controlUseTile = false;
            Player.controlHook = false;
        }
    }

    public override void OnRespawn()
    {
        base.OnRespawn();

        ModContent.GetInstance<RingSystem>().HideDeathUi();
    }

    // public int PlayerLevel => dsEndurance + dsResistance;
    public int PlayerLevel => _dsVitality + _dsAttunement + DsStrength + DsDexterity + DsIntelligence + DsFaith + DsResistance;
    
    public bool TryLevelUp()
    {
        var cost = PlayerLevel * 1000; // costo en cobre (ejemplo)
        //
        // if (Player.CountCoins() >= cost)
        // {
        //     Player.SpendCoins(cost);
        //     Level++;
        //     dsStrength += 1;
        //     dsDexterity += 1;
        //     dsIntelligence += 1;
        //     // Opcional: mensaje al jugador
        //     Main.NewText($"¡Has subido al nivel {Level}!", Color.Gold);
        //     return true;
        // }

        Main.NewText($"No tienes suficiente oro. Necesitas {cost / 100f} monedas de oro.", Color.Red);
        return false;
    }

    public override void PreUpdateMovement()
    {
        base.PreUpdateMovement();

        // Player.immune = true;
        // Player.immuneTime = StatFormulas.GetInvincibilityFramesByResistance(DsResistance);
    }

    private Dictionary<int, bool> newDebuffs = new();
    
    public override void PostUpdateBuffs()
    {
        base.PostUpdateBuffs();
        
        // This code fragment handles the first apply of debuffs
        // GlobalBuff.ReApply function overload (DSBuffChanges.cs file) is used to handle the reapplication of debuffs
        Dictionary<int, int> currentDebuffs = new();
        var playerBuffs = Player.buffType.ToList();

        foreach (var debuffType in DarkSoulsBuffChanges.TerrariaDebuff)
        {
            var debuffIndex = playerBuffs.FindIndex(x => x == debuffType);
            if (debuffIndex == -1)
            {
                newDebuffs[debuffType] = true;
                continue;
            }

            var debuffTime = Player.buffTime[debuffIndex];
            if (debuffTime <= 0)
            {
                newDebuffs[debuffType] = true;
                continue;
            }

            var isNew = newDebuffs.GetValueOrDefault(debuffType, false);

            if (!isNew)
            {
                continue;
            }

            Player.buffTime[debuffIndex] = (int)(Player.buffTime[debuffIndex] * (1f - StatFormulas.GetDebuffsResistanceByResistance(DsResistance)));
            newDebuffs[debuffType] = false;
        }
    }
    
    public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
    {
        base.ModifyHitByNPC(npc, ref modifiers);
        modifiers.FinalDamage *= 1f - StatFormulas.GetDefenseByResistance(DsResistance);
    }

    public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
    {
        base.ModifyHitByProjectile(proj, ref modifiers);
        modifiers.FinalDamage *= 1f - StatFormulas.GetDefenseByResistance(DsResistance);
    }
    
    public long DsSouls;
    private int _dsHumanity;
    private int _dsVitality;
    private int _dsAttunement = 1;
    private int DsStrength;
    private int DsDexterity;
    private int DsIntelligence;
    private int DsFaith;
    private int DsResistance;
    
    private long _pendingSouls;
    private float _soulTimer;
    private const float SoulDelay = 1f;

    public override void PostUpdate()
    {
        base.PostUpdate();
        
        Player.ConsumedLifeCrystals = Math.Clamp(StatFormulas.GetLifeByVitality(_dsVitality) / 20, 0, Player.LifeCrystalMax);
        Player.ConsumedLifeFruit = Math.Clamp((StatFormulas.GetLifeByVitality(_dsVitality) - 400) / 5, 0, Player.LifeFruitMax);
        Player.ConsumedManaCrystals = Math.Clamp(StatFormulas.GetManaByAttunement(_dsAttunement) / 20, 0, Player.ManaCrystalMax);

        // Handling click on the location of bloodstain
        if (_currentBloodstainProjectile != -1 && Main.mouseRight && Main.mouseRightRelease && Main.netMode != NetmodeID.Server)
        {
            if (IsBloodstainReachable())
            {
                var proj = Main.projectile[_currentBloodstainProjectile];
                var mouseWorld = Main.MouseWorld;
                if (proj.Hitbox.Contains(mouseWorld.ToPoint()))
                {
                    TouchBloodstain();
                }
            }
        }
        
        if (_pendingSouls <= 0)
        {
            return;
        }

        _soulTimer += 1f / 60f;
        if (_soulTimer >= SoulDelay)
        {
            CombatText.NewText(Player.getRect(), Color.WhiteSmoke, "+" + _pendingSouls);
            // SoundEngine.PlaySound(CustomRecipes.DsSoulSuck, Player.position);
            _pendingSouls = 0;
            _soulTimer = 0;
        }
    }

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

    public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
    {
        base.ModifyMaxStats(out health, out mana);
        
        // simulation of used of health, mana crystals and life fruits, for the correct behavior conditions various events
        var totalHealthBonus = StatFormulas.GetLifeByVitality(_dsVitality);
        var consumedLifeCrystals = Math.Clamp(totalHealthBonus / 20, 0, Player.LifeCrystalMax);
        var consumedLifeFruit = Math.Clamp((StatFormulas.GetLifeByVitality(_dsVitality) - 400) / 5, 0, Player.LifeFruitMax);
        var healthBonus = totalHealthBonus - consumedLifeCrystals * 20;

        var totalManaBonus = StatFormulas.GetManaByAttunement(_dsAttunement);
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

    public void AddSouls(int souls, bool render = true)
    {
        if (souls <= 0)
        {
            return;
        }

        DsSouls += souls;
        if (!render)
        {
            return;
        }

        _pendingSouls += souls;
        _soulTimer = 0f; // reset timer for each NPC kill
    }

    private List<Bloodstain> _bloodstains = [];
    private int _currentBloodstainProjectile = -1;

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

        var bloodstain = _bloodstains.FirstOrDefault(s => s.WorldGuid == Main.ActiveWorldFileData.UniqueId.ToString());
        if (bloodstain == null)
        {
            return;
        }

        Main.NewText(Language.GetText("Mods.CustomRecipes.BloodstainMessage").WithFormatArgs(bloodstain.Souls, bloodstain.Humanity).Value, Color.Cyan);
        SoundEngine.PlaySound(CustomRecipes.DsNewAreaSound);
        proj.Kill();
        _currentBloodstainProjectile = -1;
        DsSouls += bloodstain.Souls;
        _dsHumanity += bloodstain.Humanity;
        _bloodstains.RemoveAll(x => x.WorldGuid == Main.ActiveWorldFileData.UniqueId.ToString());
    }

    public int RealStr()
    {
        return DsStrength + _customStrKnightsRing;
    }
    
    public int RealDex()
    {
        return DsDexterity + _customDexHuntersRing;
    }
    
    public int RealInt()
    {
        return DsIntelligence + _customIntScholarRing;
    }
    
    public int RealFai()
    {
        return DsFaith + _customFaiPriestessRing;
    }

    public int RealResistance()
    {
        return DsResistance;// + _customFaiPriestessRing;
    }
}