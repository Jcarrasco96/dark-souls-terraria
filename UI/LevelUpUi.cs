using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace CustomRecipes.UI;

public class LevelUpUi : UIState
{
    private UIPanel _panel;
    private UIText _levelText;
    private UIText _statsText;
    private UIText _costText;
    private UIText _buttonText;
    private UIImageButton _levelUpButton;

    public override void OnInitialize()
    {
        // Panel principal
        _panel = new UIPanel();
        _panel.SetPadding(10);
        _panel.Left.Set(600f, 0f);
        _panel.Top.Set(102f, 0f);
        _panel.Width.Set(250f, 0f);
        _panel.Height.Set(170f, 0f);
        Append(_panel);

        // Texto de nivel
        _levelText = new UIText("Nivel: 1");
        _levelText.Top.Set(10f, 0f);
        _panel.Append(_levelText);

        // Texto de stats
        _statsText = new UIText("Fuerza: 0\nDestreza: 0\nInteligencia: 0");
        _statsText.Top.Set(40f, 0f);
        _panel.Append(_statsText);

        // Texto de costo
        _costText = new UIText("Costo: 0 monedas");
        _costText.Top.Set(130f, 0f);
        _panel.Append(_costText);

        // Botón subir nivel
        _levelUpButton = new UIImageButton(ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonPlay"));
        _levelUpButton.Left.Set(190f, 0f);
        _levelUpButton.Top.Set(130f, 0f);
        _levelUpButton.OnLeftClick += LevelUpButton_OnClick;
        _panel.Append(_levelUpButton);

        _buttonText = new UIText("Subir");
        _buttonText.Left.Set(120f, 0f);
        _buttonText.Top.Set(130f, 0f);
        _panel.Append(_buttonText);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        var rpg = Main.LocalPlayer.GetModPlayer<RingPlayer>();
        _levelText.SetText($"Nivel: {rpg.Level}");
        _statsText.SetText($"Fuerza: {rpg.Strength}\nDestreza: {rpg.Dexterity}\nInteligencia: {rpg.Intelligence}");
        _costText.SetText($"Costo: {rpg.Level * 1000 / 100f} oro");
    }

    private static void LevelUpButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
    {
        var rpg = Main.LocalPlayer.GetModPlayer<RingPlayer>();
        rpg.TryLevelUp();
    }
}