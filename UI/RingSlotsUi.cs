using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using TerraSouls.UI.Elements;

namespace TerraSouls.UI;

public class RingSlotsUi : CustomUiState
{
    private readonly RingSlot[] _ringSlots = new RingSlot[4];
    public override bool Visible => Main.playerInventory;

    // private RuneSlot _runeSlot;

    public override void OnInitialize()
    {
        const int padding = 5;

        const float slotSpacing = padding + RingSlot.Size;

        for (var i = 0; i < 4; i++)
        {
            _ringSlots[i] = new RingSlot(i)
            {
                Left = { Pixels = 600 + i * slotSpacing, Percent = 0f },
                Top = { Pixels = 20f },
                Width = { Pixels = RingSlot.Size, Percent = 0f },
                Height = { Pixels = RingSlot.Size, Percent = 0f }
            };
            Append(_ringSlots[i]);
        }

        // _runeSlot = new RuneSlot
        // {
        //     Left = { Pixels = 600f, Percent = 0f },
        //     Top = { Pixels = 90f },
        //     Width = { Pixels = RuneSlot.Size, Percent = 0f },
        //     Height = { Pixels = RuneSlot.Size, Percent = 0f }
        // };
        // Append(_runeSlot);
    }

    public override int InsertionIndex(List<GameInterfaceLayer> layers)
    {
        return layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
    }
    
    public static float GetPulseAlpha()
    {
        // M = 0.2f, A = 0.1f
        return 0.15f + 0.05f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f);
    }
}