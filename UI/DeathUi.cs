using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace CustomRecipes.UI;

[Autoload(Side = ModSide.Client)]
public class DeathUi : UIState
{
    private float _alpha;
    private bool _fadingIn;
    private bool _fadingOut;

    public bool IsActive => _alpha > 0f || _fadingIn || _fadingOut;

    public Action OnFadeOutComplete;

    public void StartFadeIn()
    {
        _alpha = 0f;
        _fadingIn = true;
        _fadingOut = false;

        SoundEngine.PlaySound(CustomRecipes.DsThruDeath);
    }

    public void StartFadeOut()
    {
        _fadingOut = true;
        _fadingIn = false;
    }

    private const float FadeInSpeed = 0.5f / (60f * 1.2f);
    private const float FadeOutSpeed = 1f / (40f * 1.2f);
    private const float TextScale = 1f;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_fadingIn)
        {
            _alpha += FadeInSpeed;

            if (!(_alpha >= 0.9f))
            {
                return;
            }

            _alpha = 0.9f;
            _fadingIn = false;
        }
        else if (_fadingOut)
        {
            _alpha -= FadeOutSpeed;
            if (!(_alpha <= 0f))
            {
                return;
            }

            _alpha = 0f;
            _fadingOut = false;

            OnFadeOutComplete?.Invoke();
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight),
            Color.Black * _alpha);

        var text = Language.GetText("Mods.CustomRecipes.UI.YouDiedUI.MainText").Value;
        var font = CustomRecipes.OptimusPrincepsFont;
        var textSize = font.MeasureString(text) * TextScale;
        var position = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f) - textSize / 2f;

        spriteBatch.DrawString(font, text, position + new Vector2(2, 2), Color.Black * _alpha, 0f, Vector2.Zero,
            TextScale, SpriteEffects.None, 0f);
        spriteBatch.DrawString(font, text, position, Color.Firebrick * _alpha, 0f, Vector2.Zero, TextScale,
            SpriteEffects.None, 0f);

        var respawnTimeInSeconds = (int)Math.Ceiling(Main.LocalPlayer.respawnTimer / 60f);
        var respawnText = Language.GetText("Mods.CustomRecipes.UI.YouDiedUI.RespawningText")
            .WithFormatArgs(respawnTimeInSeconds).Value;
        var respawnTextSize = font.MeasureString(respawnText) * 0.3f;
        var respawnPosition = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f) - respawnTextSize / 2f +
                              new Vector2(0, respawnTextSize.Y) * 1.25f;

        spriteBatch.DrawString(font, respawnText, respawnPosition + new Vector2(1, 1), Color.Black * _alpha, 0f,
            Vector2.Zero, 0.3f, SpriteEffects.None, 0f);
        spriteBatch.DrawString(font, respawnText, respawnPosition, Color.Gray * _alpha, 0f, Vector2.Zero, 0.3f,
            SpriteEffects.None, 0f);
    }
}