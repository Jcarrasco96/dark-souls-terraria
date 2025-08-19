using System;
using TerraSouls.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace TerraSouls.UI;

public class RingSlot : UIElement
{
    private readonly int _index;

    private static RingPlayer RingPlayer => Main.LocalPlayer.GetModPlayer<RingPlayer>();

    public const int Size = 64;
    private const int ItemSize = 32;

    public RingSlot(int index)
    {
        _index = index;

        _textureNormal = ModContent.Request<Texture2D>("TerraSouls/Assets/BG_RING_DS");
        _textureHover = ModContent.Request<Texture2D>("TerraSouls/Assets/BG_RING_HOVER_DS");

        Width.Set(_textureNormal.Width(), 0f);
        Height.Set(_textureNormal.Height(), 0f);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        var isHover = IsMouseHovering;

        var pos = GetDimensions().Position();

        spriteBatch.Draw(_textureNormal.Value, pos, Color.White);
        
        if (isHover)
        {
            var alpha = RingSlotsUi.GetPulseAlpha();
            spriteBatch.Draw(_textureHover.Value, pos, Color.White * alpha);
        }

        var item = RingPlayer.RingSlots[_index];
        
        if (item.IsAir)
        {
            return;
        }

        var itemTexture = TextureAssets.Item[item.type].Value;
        var itemPos = pos + new Vector2(_textureNormal.Value.Width / 2f, _textureNormal.Value.Height / 2f);
        var scale = 1f;

        if (itemTexture.Width > ItemSize || itemTexture.Height > ItemSize)
        {
            scale = (float)ItemSize / Math.Max(itemTexture.Width, itemTexture.Height);
        }

        spriteBatch.Draw(itemTexture, itemPos, null, Color.White, 0f, itemTexture.Size() / 2f, scale, SpriteEffects.None, 0f);
            
        if (isHover)
        {
            Main.hoverItemName = item.Name;
            Main.HoverItem = item.Clone();
        }
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);

        if (!Main.mouseItem.IsAir && Main.mouseItem.ModItem is not ModRing)
        {
            SoundEngine.PlaySound(SoundID.MenuClose);
            return;
        }

        Utils.Swap(ref RingPlayer.RingSlots[_index], ref Main.mouseItem);
        SoundEngine.PlaySound(SoundID.Grab);
    }
    
    private readonly Asset<Texture2D> _textureNormal;
    private readonly Asset<Texture2D> _textureHover;
}