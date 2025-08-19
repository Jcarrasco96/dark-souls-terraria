using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using TerraSouls.UI.Elements;

namespace TerraSouls.UI;

public class BonfireUi : CustomUiState
{
    private UIPanel _mainPanel;
    private readonly List<UIText> _bonfireOptions = [];

    private readonly RingSystem _system = ModContent.GetInstance<RingSystem>();

    public override void OnInitialize()
    {
        // Panel principal
        _mainPanel = new UIPanel
        {
            HAlign = 0.5f,
            VAlign = 0.5f,
            BackgroundColor = new Color(30, 30, 40) * 0.9f,
            Width = { Pixels = 400f, Percent = 0f },
            Height = { Pixels = 600f, Percent = 0f },
        };

        // Título
        var title = new UIText("Hogueras Descubiertas", 0.8f)
        {
            HAlign = 0.5f,
            Top = { Pixels = 20f, Percent = 0f }
        };
        _mainPanel.Append(title);

        // Botón de cierre
        var closeButton = new UIText("X", 0.5f)
        {
            HAlign = 1f,
            Width = { Pixels = 30f, Percent = 0f },
            Height = { Pixels = 30f, Percent = 0f },
        };
        closeButton.OnLeftClick += (_, _) => _system.HideBonfiresUi();
        _mainPanel.Append(closeButton);

        Append(_mainPanel);
    }

    public void UpdateBonfiresList(List<Point> bonfires)
    {
        _bonfireOptions.ForEach(opt => opt.Remove());
        _bonfireOptions.Clear();

        for (var i = 0; i < bonfires.Count; i++)
        {
            var bonfireOption = new UIText($"{i + 1}. Hoguera en X:{bonfires[i].X}, Y:{bonfires[i].Y}");
            bonfireOption.Top.Set(60f + i * 40f, 0f);
            bonfireOption.HAlign = 0.5f;
            var i1 = i;
            bonfireOption.OnLeftClick += (_, _) => TeleportToBonfire(i1);
            _bonfireOptions.Add(bonfireOption);
            _mainPanel.Append(bonfireOption);
        }
    }

    private void TeleportToBonfire(int index)
    {
        _system.TeleportToBonfire(Main.LocalPlayer, index);
        _system.HideBonfiresUi();
    }

    public override int InsertionIndex(List<GameInterfaceLayer> layers)
    {
        return layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
    }
}