using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;
using TerraSouls.UI.Elements;

namespace TerraSouls.UI;

public class StatusBarUi : CustomUiState
{
    private const int BarWidth = 130;
    private const int BarHeight = 15;
    private const int Margin = 80;
    private const int Spacing = 5;
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        var player = Main.LocalPlayer.GetModPlayer<RingPlayer>();
        
        var statusList = new (string name, float progress, Color color)[]
        {
            ("Bleed", player.BleedMeter / RingPlayer.BleedMax, Color.Red),
            ("Poison", player.PoisonMeter / RingPlayer.PoisonMax, Color.Green)
        };
        
        var totalHeight = statusList.Length * (BarHeight + Spacing);
        var startPos = new Vector2(Main.screenWidth / 2f - BarWidth / 2f, Main.screenHeight - Margin - totalHeight);

        var blank = TextureAssets.MagicPixel.Value;
        
        for (var i = 0; i < statusList.Length; i++)
        {
            var status = statusList[i];
            if (status.progress <= 0f)
            {
                continue;
            }

            var pos = startPos + new Vector2(0, i * (BarHeight + Spacing));

            spriteBatch.Draw(blank, new Rectangle((int)pos.X, (int)pos.Y, BarWidth, BarHeight), Color.Gray * 0.5f);

            spriteBatch.Draw(blank, new Rectangle((int)pos.X, (int)pos.Y, (int)(BarWidth * status.progress), BarHeight), status.color);

            spriteBatch.Draw(blank, new Rectangle((int)pos.X, (int)pos.Y, BarWidth, 2), Color.Black);
            spriteBatch.Draw(blank, new Rectangle((int)pos.X, (int)pos.Y + BarHeight - 2, BarWidth, 2), Color.Black);
            spriteBatch.Draw(blank, new Rectangle((int)pos.X, (int)pos.Y, 2, BarHeight), Color.Black);
            spriteBatch.Draw(blank, new Rectangle((int)pos.X + BarWidth - 2, (int)pos.Y, 2, BarHeight), Color.Black);
        }
    }

    public override int InsertionIndex(List<GameInterfaceLayer> layers)
    {
        return layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
    }
    
    // public override bool Visible { get; set; }
    public override bool Visible
    {
        get
        {
            var ringPlayer = Main.LocalPlayer.GetModPlayer<RingPlayer>();

            return ringPlayer.BleedMeter > 0 || ringPlayer.PoisonMeter > 0;
        }
    }
}