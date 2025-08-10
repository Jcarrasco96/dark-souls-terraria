using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace CustomRecipes.UI;

public class RingSlotsUi : UIState
{
    private UIPanel _panel;
    private readonly RingSlot[] _ringSlots = new RingSlot[4];

    public override void OnInitialize()
    {
        const int padding = 5;

        _panel = new UIPanel
        {
            Left = { Pixels = 600, Percent = 0f },
            Top = { Pixels = 20, Percent = 0f },
            Width =
            {
                Pixels = _ringSlots.Length * RingSlot.Size + 2 * padding + (_ringSlots.Length - 1) * padding,
                Percent = 0f
            }, // 4 slots * 52px + 2 * 10 padding + 3 * 10 spacing
            Height = { Pixels = 1 * RingSlot.Size + 2 * padding, Percent = 0f }, // 1 slot * 52px + 2 * 10 padding
            PaddingBottom = padding,
            PaddingTop = padding,
            PaddingLeft = padding,
            PaddingRight = padding
        };
        Append(_panel);

        const float slotSpacing = padding + RingSlot.Size;

        for (var i = 0; i < 4; i++)
        {
            _ringSlots[i] = new RingSlot(i)
            {
                Left = { Pixels = i * slotSpacing, Percent = 0f },
                Top = { Pixels = 0f },
                Width = { Pixels = RingSlot.Size, Percent = 0f },
                Height = { Pixels = RingSlot.Size, Percent = 0f }
            };
            _panel.Append(_ringSlots[i]);
        }
    }
}