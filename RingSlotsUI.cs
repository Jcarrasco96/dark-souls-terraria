using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace CustomRecipes;

public class RingSlotsUi : UIState
{

    private UIPanel _panel;
    private readonly RingSlot[] _ringSlots = new RingSlot[4];

    public override void OnInitialize()
    {
        _panel = new UIPanel
        {
            Left = { Pixels = 600, Percent = 0f },
            Top = { Pixels = 20, Percent = 0f },
            Width = { Pixels = 258, Percent = 0f },
            // Width = { Pixels = 188f, Percent = 0f }, // 4 slots * 42px + 2 * 10 padding
            Height = { Pixels = 72, Percent = 0f },
            // Height = { Pixels = 52f, Percent = 0f }, // algo más ajustado
            PaddingBottom = 10,
            PaddingTop = 10,
            PaddingLeft = 10,
            PaddingRight = 10,
        };
        Append(_panel);

        const float slotSpacing = 62f;

        for (var i = 0; i < 4; i++)
        {
            _ringSlots[i] = new RingSlot(i)
            {
                Left = { Pixels = i * slotSpacing, Percent = 0f },
                Top = { Pixels = 0f },
                Width = { Pixels = 52f, Percent = 0f },
                Height = { Pixels = 52f, Percent = 0f },
            };
            _panel.Append(_ringSlots[i]);
        }
    } // A2428,A2429,A2430
    
}