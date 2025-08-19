using System.Collections.Generic;
using System.Linq;
using TerraSouls.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using TerraSouls.Enums;
using TerraSouls.Extensions;
using TerraSouls.UI.Elements;

namespace TerraSouls;

// ReSharper disable once ClassNeverInstantiated.Global
public class RingSystem : ModSystem
{
    public override void Load()
    {
        base.Load();
        
        if (Main.dedServ)
        {
            return;
        }
        
        _userInterfaces = new Dictionary<UserTypeInterface, (UserInterface, CustomUiState)>();

        // foreach (Type type in Mod.Code.GetTypes())
        // {
        //     if (!type.IsAbstract && type.IsSubclassOf(typeof(UIState)))
        //     {
        //         
        //     }
        // }
        
        InitComponent();
    }

    public override void Unload()
    {
        base.Unload();
        
        _userInterfaces.Clear();
        _userInterfaces = null;
    }

    public override void UpdateUI(GameTime gameTime)
    {
        foreach (var userInterface in _userInterfaces.Values.Where(userInterface =>
                     userInterface.Item1.CurrentState != null &&
                     ((CustomUiState)userInterface.Item1.CurrentState).Visible &&
                     Main.InGameUI.CurrentState != userInterface.Item1.CurrentState
                     ))
        {
            userInterface.Item1.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        if (DrawGrid)
        {
            var index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer("TerraSouls: Grid Debug", delegate
                    {
                        Main.spriteBatch.DrawGridOverlay();
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
        
        var index2 = layers.FindIndex(l => l.Name.Equals("Vanilla: Inventory"));
        if (index2 == -1)
        {
            index2 = layers.Count;
        }

        layers.Insert(index2, new LegacyGameInterfaceLayer("TerraSouls: Bloodstain Pointer", delegate
            {
                DrawBloodstainPointer();
                return true;
            }, InterfaceScaleType.UI
        ));
        
        // var buffIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
        // if (buffIndex != -1)
        // {
        //     layers.Insert(buffIndex + 1, new LegacyGameInterfaceLayer("TerraSouls: Status Effects", delegate
        //         {
        //             if (!Main.gameMenu && !Main.playerInventory)
        //             {
        //                 DrawStatusEffects();
        //             }
        //             return true;
        //         },
        //         InterfaceScaleType.UI
        //     ));
        // }

        foreach (var uiState in _userInterfaces.Values)
        {
            AddLayer(layers, uiState.Item2, uiState.Item2.InsertionIndex(layers), uiState.Item2.Visible, uiState.Item2.Scale);
        }
    }

    private void DrawBloodstainPointer()
    {
        var player = Main.LocalPlayer.GetModPlayer<RingPlayer>();
            
        var target = player.Bloodstains.LastOrDefault();
        if (target == null)
        {
            return;
        }

        var playerPos = Main.player[Main.myPlayer].Center;
        var bloodPos = target.Position;

        if (IsOnScreen(bloodPos) || !(Vector2.Distance(playerPos, bloodPos) > 100f))
        {
            return;
        }

        var screenCenter = new Vector2(Main.screenWidth, Main.screenHeight) / 2;
        var toTarget = bloodPos - playerPos;
        var arrowTex = ModContent.Request<Texture2D>("TerraSouls/Assets/CursorArrowProjectile_legacy").Value;
        var angle = toTarget.ToRotation();
        var dir = angle.ToRotationVector2();
        var drawPos = screenCenter + dir * 100f;
            
        Main.spriteBatch.Draw(arrowTex, drawPos, null, Color.White, angle, arrowTex.Size() / 2f, 1f, SpriteEffects.None, 0f);
    }
    
    private static bool IsOnScreen(Vector2 objectPosition)
    {
        var screenPos = objectPosition - Main.screenPosition;
        return screenPos.X >= 0 && screenPos.X <= Main.screenWidth && screenPos.Y >= 0 && screenPos.Y <= Main.screenHeight;
    }
    
    private static void DrawStatusEffects()
    {
        // var player = Main.LocalPlayer;
        // var startPos = new Vector2(35, 120);
        // const int iconSize = 28;
        // const int padding = 10;

        // var ringPlayer = player.GetModPlayer<RingPlayer>();
        
        // for (var i = 0; i < ringPlayer.StatusEffects.Count; i++)
        // {
        //     var effect = ringPlayer.StatusEffects[i];
        //
        //     var destRect = new Rectangle(
        //         (int)startPos.X + i * (iconSize + padding),
        //         (int)startPos.Y,
        //         iconSize,
        //         iconSize
        //     );
        //
        //     var hovering = destRect.Contains(Main.MouseScreen.ToPoint());
        //
        //     var alpha = hovering ? 1f : 0.5f;
        //
        //     Main.spriteBatch.Draw(effect.Texture, destRect, Color.White * alpha);
        //
        //     if (hovering)
        //     {
        //         Main.instance.MouseText(effect.Name);
        //         player.mouseInterface = true;
        //     }
        //     // Main.spriteBatch.Draw(effect.Texture, destRect, Color.White);
        // }
    }

    private static void AddLayer(List<GameInterfaceLayer> layers, CustomUiState state, int index, bool visible, InterfaceScaleType scale)
    {
        var name = state == null ? "Unknown" : state.ToString();

        if (state is DeathUi deathUi)
        {
            layers[index] = new LegacyGameInterfaceLayer($"TerraSouls: {name}",() => !deathUi.IsActive, scale);
        }
        
        layers.Insert(index, new LegacyGameInterfaceLayer($"TerraSouls: {name}", delegate
            {
                if (visible)
                {
                    state?.Draw(Main.spriteBatch);
                }

                return true;
            }, scale));
    }

    private void InitComponent()
    {
        var ringUi = new RingSlotsUi();
        var ringInterface = new UserInterface();
        ringInterface.SetState(ringUi);
        ringUi.UserInterface = ringInterface;
        _userInterfaces?.Add(UserTypeInterface.RingInterface, (ringInterface, ringUi));
        
        var deathUi = new DeathUi();
        var deathInterface = new UserInterface();
        deathInterface.SetState(deathUi);
        deathUi.UserInterface = deathInterface;
        _userInterfaces?.Add(UserTypeInterface.DeathInterface, (deathInterface, deathUi));
        
        var bonfireUi = new BonfireUi();
        var bonfireInterface = new UserInterface();
        bonfireInterface.SetState(bonfireUi);
        bonfireUi.UserInterface = bonfireInterface;
        _userInterfaces?.Add(UserTypeInterface.BonfireInterface, (bonfireInterface, bonfireUi));
        
        var levelUpUi = new ClassSelectionUi();
        var levelUpInterface = new UserInterface();
        levelUpInterface.SetState(levelUpUi);
        levelUpUi.UserInterface = levelUpInterface;
        _userInterfaces?.Add(UserTypeInterface.LevelUpInterface, (levelUpInterface, levelUpUi));
        
        var statusBarUi = new StatusBarUi();
        var statusBarInterface = new UserInterface();
        statusBarInterface.SetState(statusBarUi);
        statusBarUi.UserInterface = statusBarInterface;
        _userInterfaces?.Add(UserTypeInterface.StatusBarInterface, (statusBarInterface, statusBarUi));
    }

    public readonly List<Point> Bonfires = [];

    public void ShowBonfiresUi()
    {
        TryGetUserInterface<BonfireUi>(UserTypeInterface.BonfireInterface).Visible = true;
    }

    public void HideBonfiresUi()
    {
        TryGetUserInterface<BonfireUi>(UserTypeInterface.BonfireInterface).Visible = false;
    }

    public void TeleportToBonfire(Player player, int index)
    {
        if (index < 0 || index >= Bonfires.Count)
        {
            Main.NewText($"Teletransportado a Hoguera {index}!", Color.Green);
            return;
        }

        var bonfirePos = Bonfires[index];

        player.Teleport(new Vector2(bonfirePos.X * 16, bonfirePos.Y * 16 - 32));
        Main.NewText($"Teletransportado a Hoguera {index + 1}!", Color.Green);
    }

    public override void SaveWorldData(TagCompound tag)
    {
        base.SaveWorldData(tag);

        tag["BonfiresPlaced"] = Bonfires.Select(p => new TagCompound
        {
            ["X"] = p.X,
            ["Y"] = p.Y
        }).ToList();
    }

    public override void LoadWorldData(TagCompound tag)
    {
        base.LoadWorldData(tag);

        Bonfires.Clear();

        if (!tag.ContainsKey("BonfiresPlaced"))
        {
            return;
        }

        var loadedItems = tag.GetList<TagCompound>("BonfiresPlaced");
        Bonfires.AddRange(loadedItems.Select(tc => new Point(tc.GetInt("X"), tc.GetInt("Y"))));
    }

    public void ShowDeathUi()
    {
        TryGetUserInterface<DeathUi>(UserTypeInterface.DeathInterface).StartFadeIn();
    }

    public void HideDeathUi()
    { 
        TryGetUserInterface<DeathUi>(UserTypeInterface.DeathInterface).StartFadeOut();
    }

    private const bool DrawGrid = false;
    
    private Dictionary<UserTypeInterface, (UserInterface, CustomUiState)> _userInterfaces;
    
    public override void PostDrawInterface(SpriteBatch spriteBatch)
    {
        base.PostDrawInterface(spriteBatch);
        
        var mouseScreenPos = Main.MouseScreen;

        Utils.DrawBorderString(spriteBatch, $"Mouse: X={mouseScreenPos.X:0}, Y={mouseScreenPos.Y:0}", new Vector2(10, 10), Color.Yellow);
        
        BossUiManager.Draw(spriteBatch);
    }

    public override void PostUpdateWorld()
    {
        base.PostUpdateWorld();
        
        var boss = Main.npc.FirstOrDefault(npc => npc.active && npc.boss);

        if (boss != null && !_bossActive)
        {
            _bossActive = true;
            BossUiManager.ShowBossIntro(boss.type);
        }
        
        if (boss == null && _bossActive)
        {
            _bossActive = false;
            BossUiManager.ShowBossDefeated();
        }
    }

    private bool _bossActive;
    
    public void ShowLevelUpUi()
    {
        var levelUpUi = TryGetUserInterface<ClassSelectionUi>(UserTypeInterface.LevelUpInterface);
        
        levelUpUi.LoadData(Main.LocalPlayer.GetModPlayer<RingPlayer>());
        levelUpUi.Visible = true;
    }
    
    public void HideLevelUpUi()
    {
        TryGetUserInterface<ClassSelectionUi>(UserTypeInterface.LevelUpInterface).Visible = false;
    }

    private T TryGetUserInterface<T>(UserTypeInterface typeInterface)
    {
        _userInterfaces.TryGetValue(typeInterface, out var state);

        if (state.Item2 is T stateOut)
        {
            return stateOut;
        }
        
        return default;
    }

    public static readonly List<Vector2> SoulPositions = [];
    
    public override void PostUpdatePlayers()
    {
        base.PostUpdatePlayers();
        
        var player = Main.LocalPlayer;
        
        for (var i = SoulPositions.Count - 1; i >= 0; i--)
        {
            var target = player.Center;
            var direction = target - SoulPositions[i];
            const float speed = 3.5f;

            if (direction.Length() < 10f)
            {
                SoulPositions.RemoveAt(i);
                continue;
            }

            direction.Normalize();
            
            var playerSpeedFactor = player.velocity.Length() * 0.8f;
            
            SoulPositions[i] += direction * (speed + playerSpeedFactor);

            Dust.NewDustPerfect(SoulPositions[i], DustID.GoldCoin, Vector2.Zero, 150, Color.Yellow, 1.2f).noGravity = true;
        }
    }
}