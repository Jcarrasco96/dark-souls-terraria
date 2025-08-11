using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace CustomRecipes.UI;

public class ClassSelectionUi : UIState
{
    private UIPanel _mainPanel;
    private UIButton<string> _closeButton;
    
    private const int TotalColumns = 5;
    private const int TotalRows = 2;
    private const float PanelWidth = 200f;
    private const float PanelHeight = 160f;
    private const float SpacingX = 10f;
    private const float SpacingY = 10f;

    public override void OnInitialize()
    {
        _closeButton = new UIButton<string>("Cancel")
        {
            Width = { Pixels = 80, Percent = 0f },
            Height = { Pixels = 40, Percent = 0f },
            Left = { Pixels = 0f, Percent = 0f },
            Top = { Pixels = 0f, Percent = 0f },
            VAlign = 0f,
            HAlign = 1f,
        };

        AddLevelUp();

        //AddSelectClass();
        
        _closeButton.OnLeftClick += HideMainUi;
        _mainPanel.Append(_closeButton);
    }

    private void AddSelectClass()
    {
        // Panel principal
        _mainPanel = new UIPanel
        {
            Width = { Pixels = TotalColumns * PanelWidth + (TotalColumns + 1) * SpacingX, Percent = 0f },
            Height = { Pixels = TotalRows * PanelHeight + (TotalRows + 2) * SpacingY + _closeButton.Height.Pixels, Percent = 0f },
            HAlign = 0.5f,
            VAlign = 0.5f,
        };
        Append(_mainPanel);

        
        for (var row = 0; row < TotalRows; row++)
        {
            for (var col = 0; col < TotalColumns; col++)
            {
                var index = row * TotalColumns + col;

                var classPanel = new UIPanel();
                classPanel.Width.Set(PanelWidth, 0f);
                classPanel.Height.Set(PanelHeight, 0f);
                classPanel.Left.Set(col * (PanelWidth + SpacingX), 0f);
                classPanel.Top.Set(row * (PanelHeight + SpacingY), 0f);
                classPanel.BackgroundColor = new Color(50, 50, 50) * 0.8f;

                var label = new UIText($"Clase {index + 1}\nSTR: 0\nDEX: 0\nINT: 0\nFAI: 0", 0.8f)
                {
                    HAlign = 0.5f
                };
                label.Top.Set(5, 0f);
                classPanel.Append(label);

                _mainPanel.Append(classPanel);
            }
        }
    }

    private void AddLevelUp()
    {
        _mainPanel = new UIPanel
        {
            Width = { Pixels = 800, Percent = 0f },
            Height = { Pixels = 600, Percent = 0f },
            HAlign = 0.5f,
            VAlign = 0.5f,
        };
        Append(_mainPanel);
        
        
    }

    private static void HideMainUi(UIMouseEvent evt, UIElement listeningElement)
    {
        ModContent.GetInstance<MyUiSystem>().HideUi();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_mainPanel.ContainsPoint(Main.MouseScreen))
        {
            Main.LocalPlayer.mouseInterface = true;
        }

        // Cerrar con ESC
        // if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
        // {
        //     _system.HideUI();
        //     Main.blockInput = false;
        // }
    }

    // public override void Draw(SpriteBatch spriteBatch)
    // {
    //     base.Draw(spriteBatch);
    //     
    //     if (IsMouseHovering)
    //     {
    //         Main.LocalPlayer.mouseInterface = true;
    //     }
    // }
}