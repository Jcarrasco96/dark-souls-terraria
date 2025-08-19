using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using TerraSouls.Enums;

namespace TerraSouls.UI.Elements;

public class HoverImage : UIImage
{
    public OnClickAction Action;
    public PlayerStatsLevel PlayerStat;

    public HoverImage(Texture2D nonReloadingTexture) : base(nonReloadingTexture)
    {
        Color *= 0.8f;
    }
    
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        if (!Visible)
        {
            return;
        }
        
        if (IsMouseHovering)
        {
            var rect = GetDimensions().ToRectangle();
            
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(rect.X - 2, rect.Y - 2, rect.Width + 4, rect.Height + 4), Color.Yellow * 0.5f);
        }
        
        base.DrawSelf(spriteBatch);
    }

    public bool Visible = true;
}