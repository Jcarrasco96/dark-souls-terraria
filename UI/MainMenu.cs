using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace TerraSouls.UI;

public class MainMenu : ModMenu
{
    public override void OnSelected()
    {
        base.OnSelected();
        SoundEngine.PlaySound(TerraSouls.DsMainMenuStartSound);
    }

    public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
    {
        // return base.PreDrawLogo(spriteBatch, ref logoDrawCenter, ref logoRotation, ref logoScale, ref drawColor);
        
        var texture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/MenuBackground").Value;

        // Calculate the draw position offset and scale in the event that someone is using a non-16:9 monitor
        var drawOffset = Vector2.Zero;
        var xScale = (float)Main.screenWidth / texture.Width;
        var yScale = (float)Main.screenHeight / texture.Height;
        var scale = xScale;

        // if someone's monitor isn't in wacky dimensions, no calculations need to be performed at all
        if (xScale != yScale)
        {
            // If someone's monitor is tall, it needs to be shifted to the left so that it's still centered on screen
            // Additionally the Y scale is used so that it still covers the entire screen
            if (yScale > xScale)
            {
                scale = yScale;
                drawOffset.X -= (texture.Width * scale - Main.screenWidth) * 0.5f;
            }
            else
                // The opposite is true if someone's monitor is widescreen
            {
                drawOffset.Y -= (texture.Height * scale - Main.screenHeight) * 0.5f;
            }
        }

        spriteBatch.Draw(texture, drawOffset, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

        // Set the logo draw color to be white and the time to be noon
        // This is because there is not a day/night cycle in this menu, and changing colors would look bad
        drawColor = Color.White;
        Main.time = 27000;
        Main.dayTime = true;

        // Draw the logo using a different spritebatch blending setting so it doesn't have a horrible yellow glow
        var drawPos = new Vector2(Main.screenWidth / 2f, 100f);
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
        spriteBatch.Draw(Logo.Value, drawPos, null, drawColor, logoRotation, Logo.Value.Size() * 0.5f, logoScale, SpriteEffects.None, 0f);
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

        return false;
    }

    public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/DS_Logo");
    public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/DarkSoulsIII");
    public override string DisplayName => "Dark Souls Style";
}