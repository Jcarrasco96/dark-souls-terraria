using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CustomRecipes.Rings;
using Terraria.Audio;
using Terraria.ID;

namespace CustomRecipes;

public class RingSlot : UIElement
{

    private readonly int _index;

    private static RingPlayer RingPlayer => Main.LocalPlayer.GetModPlayer<RingPlayer>();

    public RingSlot(int index)
    {
        _index = index;

        Width.Set(52f, 1f);
        Height.Set(52f, 1f);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        // Texture2D slotTexture = TextureAssets.InventoryBack8.Value;
        // Vector2 position = GetDimensions().Position() + new Vector2(20f);
        // Vector2 slotOrigin = slotTexture.Size() * 0.5f;
        // float scale = 40f / slotTexture.Size().X;

        // spriteBatch.Draw(slotTexture, position, null, Color.White, 0f, slotOrigin, scale, SpriteEffects.None, 0f);

        spriteBatch.Draw(IsMouseHovering ? TextureAssets.InventoryBack8.Value : TextureAssets.InventoryBack7.Value, GetDimensions().Position(), Color.White);
        
        var item = RingPlayer.RingSlots[_index];

        if (item.IsAir)
        {
            return;
        }
        
        var itemTexture = TextureAssets.Item[item.type].Value;

        var slotPos = GetDimensions().Position() + new Vector2(26f);
        var origin = itemTexture.Size() * 0.5f;

        spriteBatch.Draw(itemTexture, slotPos, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);

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