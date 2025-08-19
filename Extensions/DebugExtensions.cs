using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace TerraSouls.Extensions;

public static class DebugExtensions
{
    public static void DrawDebugBordersRecursive(this SpriteBatch spriteBatch, UIElement element)
    {
        spriteBatch.DrawBorder(element.GetDimensions().ToRectangle(), Color.WhiteSmoke, 1);

        foreach (var child in element.Children)
        {
            spriteBatch.DrawDebugBordersRecursive(child);
        }
    }
    
    private static void DrawBorder(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, int thickness)
    {
        var pixel = TextureAssets.MagicPixel.Value;

        spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, thickness), color);
        spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Bottom - thickness, rectangle.Width, thickness), color);

        spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Top, thickness, rectangle.Height), color);
        spriteBatch.Draw(pixel, new Rectangle(rectangle.Right - thickness, rectangle.Top, thickness, rectangle.Height), color);
    }

    public static void DrawGridOverlay(this SpriteBatch spriteBatch, int spacing = 20)
    {
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
            SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);

        var pixel = Texture2DWhite();

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