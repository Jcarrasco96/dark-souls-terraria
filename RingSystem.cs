using System.Collections.Generic;
using System.CommandLine;
using CustomRecipes.Bonfire;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace CustomRecipes;

public class RingSystem : ModSystem
{
    private RingSlotsUi _ringUi;
    private UserInterface _userInterface;

    public override void Load()
    {
        InitComponent();
    }

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
        
        if (_bonfireInterface != null && _bonfireInterface?.CurrentState != null)
        {
            _bonfireInterface.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        var mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

        if (mouseTextIndex == -1)
        {
            return;
        }

        layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("CustomRecipes: Ring UI", delegate
            {
                if (Main.playerInventory) // Solo dibujar UI si inventario abierto
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
    }

    private void InitComponent()
    {
        _ringUi = new RingSlotsUi();
        _ringUi.Activate();
        _userInterface = new UserInterface();
        _userInterface.SetState(_ringUi);
        
        _bonfireUi = new BonfireUI();
        _bonfireUi.Activate();
        _bonfireInterface = new UserInterface();
        // _bonfireInterface.SetState(_bonfireUi);
    }
    
    private UserInterface _bonfireInterface;
    private BonfireUI _bonfireUi;

    public readonly List<Point> Bonfires = [];
    
    public void ShowUi()
    {
        _bonfireInterface?.SetState(_bonfireUi);
        _bonfireUi.UpdateBonfiresList(Bonfires);
    }

    public void HideUi()
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
    
}