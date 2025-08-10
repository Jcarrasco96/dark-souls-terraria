using CustomRecipes.Rings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.UI;

namespace CustomRecipes.UI;

public class RingSlot : UIElement
{
    private readonly int _index;

    private static RingPlayer RingPlayer => Main.LocalPlayer.GetModPlayer<RingPlayer>();

    public const int Size = 52;

    public RingSlot(int index)
    {
        _index = index;

        Width.Set(Size, 1f);
        Height.Set(Size, 1f);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        var bgTexture = IsMouseHovering ? TextureAssets.InventoryBack15.Value : TextureAssets.InventoryBack12.Value;

        spriteBatch.Draw(bgTexture, GetDimensions().Position(), Color.White);

        var item = RingPlayer.RingSlots[_index];

        if (item.IsAir)
        {
            return;
        }

        var itemTexture = TextureAssets.Item[item.type].Value;

        var slotPos = GetDimensions().Position() + new Vector2(Size / 2f);

        spriteBatch.Draw(itemTexture, slotPos, null, Color.White, 0f, itemTexture.Size() * 0.5f,
            (Size - 20f) / itemTexture.Height, SpriteEffects.None, 0f);

        if (!IsMouseHovering)
        {
            return;
        }

        Main.hoverItemName = item.Name;
        Main.HoverItem = item.Clone();
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
}