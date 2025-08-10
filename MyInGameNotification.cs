using CustomRecipes.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace CustomRecipes;

/**
 * how to use
 * InGameNotificationsTracker.AddNotification(new MyInGameNotification("text goes here"));
 */
public class MyInGameNotification(string title) : IInGameNotification
{
    
    private readonly Asset<Texture2D> _iconTexture = TextureAssets.Buff[ModContent.BuffType<ExampleDefenseDebuff>()];
    
    public void Update()
    {
        _timeLeft--;

        // Keep the timer kept to a minimum value of 0 to avoid issues, since we
        // use it for lerping and other effects.
        if (_timeLeft < 0) {
            _timeLeft = 0;
        }
    }

    public void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition)
    {
        if (Opacity <= 0f) {
            return;
        }

        // Below is draw-code directly from vanilla with some tweaks to suit our needs.
        // Changes are minimal; important things to note:
        // - we draw the panel with Utils.DrawInvBG,
        // - we calculate the panel size based on the title size,
        // - we draw the title and icon after the panel,
        // - we utilize the calculated opacity and scale values.

        var effectiveScale = Scale * 1.1f;
        var size = (FontAssets.ItemStack.Value.MeasureString(title) + new Vector2(58f, 10f)) * effectiveScale;
        var panelSize = Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, (0f - size.Y) * 0.5f), size);

        // Check if the mouse is hovering over the notification.
        var hovering = panelSize.Contains(Main.MouseScreen.ToPoint());

        Utils.DrawInvBG(spriteBatch, panelSize, new Color(64, 109, 164) * (hovering ? 0.75f : 0.5f));
        var iconScale = effectiveScale * 0.7f;
        var vector = panelSize.Right() - Vector2.UnitX * effectiveScale * (12f + iconScale * _iconTexture.Width());
        spriteBatch.Draw(_iconTexture.Value, vector, null, Color.White * Opacity, 0f, new Vector2(0f, _iconTexture.Width() / 2f), iconScale, SpriteEffects.None, 0f);
        Utils.DrawBorderString(color: new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor / 5, Main.mouseTextColor) * Opacity, sb: spriteBatch, text: title, pos: vector - Vector2.UnitX * 10f, scale: effectiveScale * 0.9f, anchorx: 1f, anchory: 0.4f);

        if (hovering) {
            OnMouseOver();
        }
    }

    public void PushAnchor(ref Vector2 positionAnchorBottom)
    {
        // Anchoring is used for determining how much space a popup takes up, essentially.
        // This is because notifications visually stack. In our case, we want to let other notifications
        // go in front of ours once we start fading off, so we scale the offset based on opacity.
        positionAnchorBottom.Y -= 50f * Opacity;
    }

    public bool ShouldBeRemoved => _timeLeft <= 0;

    private int _timeLeft = 5 * 60;
    
    private float Scale {
        get
        {
            return _timeLeft switch
            {
                < 30 => MathHelper.Lerp(0f, 1f, _timeLeft / 30f),
                > 285 => MathHelper.Lerp(1f, 0f, (_timeLeft - 285) / 15f),
                _ => 1f
            };
        }
    }

    private float Opacity {
        get {
            if (Scale <= 0.5f) {
                return 0f;
            }

            return (Scale - 0.5f) / 0.5f;
        }
    }
    
    private void OnMouseOver() {
        // This method is called when the user hovers over the notification.

        // Skip if we're ignoring mouse input.
        if (PlayerInput.IgnoreMouseInterface) {
            return;
        }

        // We are now interacting with a UI.
        Main.LocalPlayer.mouseInterface = true;

        if (!Main.mouseLeft || !Main.mouseLeftRelease) {
            return;
        }

        Main.mouseLeftRelease = false;

        // In our example, we just accelerate the exiting process on click.
        // If you want it to close immediately, you can just set timeLeft to 0.
        // This allows the notification time to shrink and fade away, as expected.
        if (_timeLeft > 30) {
            _timeLeft = 30;
        }
    }
    
}