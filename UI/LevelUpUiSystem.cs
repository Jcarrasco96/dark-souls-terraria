using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace CustomRecipes.UI;

// ReSharper disable once ClassNeverInstantiated.Global
public class LevelUpUiSystem : ModSystem
{
    private UserInterface _levelUpInterface;
    private LevelUpUi _levelUpUi;

    public override void Load()
    {
        _openLevelUiKeybind = KeybindLoader.RegisterKeybind(Mod, "Abrir UI de Leveleo", "L");

        if (Main.dedServ)
        {
            return;
        }

        _levelUpUi = new LevelUpUi();
        _levelUpUi.Activate();
        _levelUpInterface = new UserInterface();
        _levelUpInterface.SetState(_levelUpUi);
    }

    public override void Unload()
    {
        _openLevelUiKeybind = null;
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (_levelUpInterface?.CurrentState != null)
        {
            _levelUpInterface.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        var mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("MyMod: LevelUpUI", delegate
                {
                    if (_levelUpInterface?.CurrentState != null)
                    {
                        _levelUpInterface.Draw(Main.spriteBatch, new GameTime());
                    }

                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }

    private static ModKeybind _openLevelUiKeybind;

    public override void PostUpdateInput()
    {
        if (!_openLevelUiKeybind.JustPressed)
        {
            return;
        }

        _levelUpInterface.SetState(_levelUpInterface.CurrentState == null ? _levelUpUi : null);
    }

    public void ShowLevelUpInterface()
    {
        _levelUpInterface.SetState(_levelUpInterface.CurrentState == null ? _levelUpUi : null);
    }
}