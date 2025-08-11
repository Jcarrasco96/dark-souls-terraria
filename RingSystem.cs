using System.Collections.Generic;
using System.Linq;
using CustomRecipes.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace CustomRecipes;

// ReSharper disable once ClassNeverInstantiated.Global
public class RingSystem : ModSystem
{
    private RingSlotsUi _ringUi;
    private UserInterface _userInterface;

    public override void OnModLoad()
    {
        InitComponent();
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (Main.playerInventory)
        {
            _userInterface?.Update(gameTime);
        }

        if (_bonfireInterface is { CurrentState: not null })
        {
            _bonfireInterface.Update(gameTime);
        }

        if (_deathInterface?.CurrentState != null)
        {
            _deathInterface.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        if (DrawGrid)
        {
            var index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer("TuMod: Grid Debug", delegate
                    {
                        DrawGridOverlay(Main.spriteBatch);
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
        
        var mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

        if (mouseTextIndex == -1)
        {
            return;
        }

        layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("CustomRecipes: Ring UI", delegate
            {
                if (Main.playerInventory)
                {
                    _userInterface.Draw(Main.spriteBatch, new GameTime());
                }

                if (_bonfireInterface?.CurrentState != null)
                {
                    _bonfireInterface.Draw(Main.spriteBatch, new GameTime());
                }

                return true;
            },
            InterfaceScaleType.UI)
        );

        var deathTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Death Text"));
        if (deathTextIndex == -1)
        {
            return;
        }

        layers[deathTextIndex] = new LegacyGameInterfaceLayer("CustomRecipes: Block Death Text",
            () => !_deathUi.IsActive, InterfaceScaleType.UI);

        layers.Insert(deathTextIndex, new LegacyGameInterfaceLayer("CustomRecipes: Death Screen UI", () =>
            {
                if (_deathUi.IsActive)
                {
                    _deathInterface.Draw(Main.spriteBatch, new GameTime());
                }

                return true;
            },
            InterfaceScaleType.UI));
    }

    private void InitComponent()
    {
        _ringUi = new RingSlotsUi();
        _ringUi.Activate();
        _userInterface = new UserInterface();
        _userInterface.SetState(_ringUi);

        _bonfireUi = new BonfireUi();
        _bonfireUi.Activate();
        _bonfireInterface = new UserInterface();
        // _bonfireInterface.SetState(_bonfireUi);

        if (Main.dedServ)
        {
            return;
        }

        _deathUi = new DeathUi();
        _deathUi.Activate();
        _deathInterface = new UserInterface();
        _deathInterface.SetState(null);

        _deathUi.OnFadeOutComplete = () => { _deathInterface?.SetState(null); };
    }

    private UserInterface _bonfireInterface;
    private BonfireUi _bonfireUi;

    public readonly List<Point> Bonfires = [];

    public void ShowBonfiresUi()
    {
        _bonfireInterface?.SetState(_bonfireUi);
        _bonfireUi.UpdateBonfiresList(Bonfires);
    }

    public void HideBonfiresUi()
    {
        _bonfireInterface?.SetState(null);
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

    private UserInterface _deathInterface;
    private DeathUi _deathUi;

    public void ShowDeathUi()
    {
        _deathInterface.SetState(_deathUi);
        _deathUi.StartFadeIn();
    }

    public void HideDeathUi()
    {
        if (_deathInterface?.CurrentState != null)
        {
            _deathUi.StartFadeOut();
        }
    }

    private const bool DrawGrid = false;

    private static void DrawGridOverlay(SpriteBatch spriteBatch)
    {
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
            SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);

        var pixel = Texture2DWhite();
        const int spacing = 50; // cada 50px

        for (var x = 0; x < Main.screenWidth; x += spacing)
        {
            spriteBatch.Draw(pixel, new Rectangle(x, 0, 1, Main.screenHeight), Color.Gray * 0.3f);
        }

        for (var y = 0; y < Main.screenHeight; y += spacing)
        {
            spriteBatch.Draw(pixel, new Rectangle(0, y, Main.screenWidth, 1), Color.Gray * 0.3f);
        }

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
            SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
    }

    private static Texture2D Texture2DWhite()
    {
        var tex = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
        tex.SetData([Color.White]);
        return tex;
    }
    
}