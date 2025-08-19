using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;

namespace TerraSouls.UI.Elements;

public class GridPanel : UIPanel
{
    public int PaddingPixels = 8;
    public int CellSize = 20;
    public Color GridColor = Color.Gray * 0.5f;

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);

        var dims = GetDimensions();

        var drawArea = new Rectangle(
            (int)(dims.X + PaddingPixels),
            (int)(dims.Y + PaddingPixels),
            (int)(dims.Width - PaddingPixels * 2),
            (int)(dims.Height - PaddingPixels * 2)
        );

        DrawGrid(spriteBatch, drawArea);
    }

    private void DrawGrid(SpriteBatch spriteBatch, Rectangle area)
    {
        var pixel = TextureAssets.MagicPixel.Value;

        for (var x = area.Left; x <= area.Right; x += CellSize)
        {
            spriteBatch.Draw(pixel, new Rectangle(x, area.Top, 1, area.Height), GridColor);
        }

        for (var y = area.Top; y <= area.Bottom; y += CellSize)
        {
            spriteBatch.Draw(pixel, new Rectangle(area.Left, y, area.Width, 1), GridColor);
        }
    }
}