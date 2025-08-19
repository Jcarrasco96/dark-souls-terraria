using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using TerraSouls.Enums;

namespace TerraSouls.UI.Elements;

public class UiTextCustom(string text, float textScale = 1f) : UIText(text, textScale)
{
    private readonly float _textScale1 = textScale;
    
    public VerticalAlignment VAlignText { get; init; } = VerticalAlignment.Top;
    public HorizontalAlignment HAlignText { get; init; } = HorizontalAlignment.Left;

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        var dims = GetDimensions();

        var textSize = FontAssets.MouseText.Value.MeasureString(Text) * _textScale1;

        var x = HAlignText switch
        {
            HorizontalAlignment.Left => dims.X,
            HorizontalAlignment.Center => dims.X + dims.Width / 2f - textSize.X / 2f,
            HorizontalAlignment.Right => dims.X + dims.Width - textSize.X,
            _ => dims.X
        };
        
        var y = VAlignText switch
        {
            VerticalAlignment.Top => dims.Y,
            VerticalAlignment.Center => dims.Y + dims.Height / 2f - textSize.Y / 2f,
            VerticalAlignment.Bottom => dims.Y + dims.Height - textSize.Y,
            _ => dims.Y
        };
        
        Utils.DrawBorderString(spriteBatch, Text, new Vector2(x, y), TextColor, _textScale1);
    }
}