using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace CustomRecipes.UI;

public class MyUiSystem : ModSystem
{
    private UserInterface _classInterface;
    private ClassSelectionUi _classUi;
    
    public bool UiVisible => _classInterface?.CurrentState != null;

    public override void OnModLoad()
    {
        if (Main.dedServ)
        {
            return;
        }

        _classInterface = new UserInterface();
        _classUi = new ClassSelectionUi();
        _classUi.Activate();
    }

    public void ShowClassUi()
    {
        _classInterface.SetState(_classUi);
    }

    public void HideUi()
    {
        _classInterface?.SetState(null);
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (!UiVisible)
        {
            return;
        }

        if (_classInterface?.CurrentState != null)
        {
            _classInterface.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        var mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("MyMod: Class Selection UI", delegate
                {
                    if (_classInterface?.CurrentState != null)
                    {
                        _classInterface.Draw(Main.spriteBatch, new GameTime());
                    }
                    return true;
                },
                InterfaceScaleType.UI));
        }
    }
}