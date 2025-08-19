using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using TerraSouls.Commons;
using TerraSouls.Enums;
using TerraSouls.UI.Elements;

namespace TerraSouls.UI;

public class ClassSelectionUi : CustomUiState
{
    private UIPanel _mainPanel;
    private UIButton<string> _acceptButton;
    private UIButton<string> _closeButton;

    private static int _startPlaceAttributes = 60;

    private long _playerDataSouls;
    private int _playerDataHumanity;

    private Dictionary<PlayerStatsLevel, int> _stats;
    private Dictionary<PlayerStatsLevel, int> _statsNoChange;

    private long _reqSoulsToLevelUp;
    
    #region BUTTONS AND VALUES
    private Texture2D _upLevelTexture;
    private Texture2D _downLevelTexture;
    
    private Texture2D _levelTexture;
    private Texture2D _soulsTexture;
    private Texture2D _reqSoulsTexture;
    private Texture2D _vitalityTexture;
    private Texture2D _attunementTexture;
    private Texture2D _strengthTexture;
    private Texture2D _dexterityTexture;
    private Texture2D _intelligenceTexture;
    private Texture2D _faithTexture;
    private Texture2D _resistanceTexture;
    private Texture2D _humanityTexture;
    private Texture2D _hpTexture;
    private Texture2D _manaTexture;
    private Texture2D _defenseTexture;
    private Texture2D _debuffsResistanceTexture;
    private Texture2D _luckTexture;
    private Texture2D _potentialTexture;
    
    private UIText _valueLevel;
    private UIText _valueReqSouls;
    private UIText _valuePlayerSouls;
    private UIText _valuePlayerHumanity;
    
    private HoverImage _increaseVitality;
    private HoverImage _decreaseVitality;
    private UIText _valueVitality;
    
    private HoverImage _increaseAttunement;
    private HoverImage _decreaseAttunement;
    private UIText _valueAttunement;
    
    private HoverImage _increaseStrength;
    private HoverImage _decreaseStrength;
    private UIText _valueStrength;
    
    private HoverImage _increaseDexterity;
    private HoverImage _decreaseDexterity;
    private UIText _valueDexterity;
    
    private HoverImage _increaseIntelligence;
    private HoverImage _decreaseIntelligence;
    private UIText _valueIntelligence;
    
    private HoverImage _increaseFaith;
    private HoverImage _decreaseFaith;
    private UIText _valueFaith;
    
    private HoverImage _increaseResistance;
    private HoverImage _decreaseResistance;
    private UIText _valueResistance;

    private UIText _valueHp;
    private UIText _valueMana;
    private UIText _valueDefense;
    private UIText _valueDebuffsResistance;
    private UIText _valueLuck;
    
    private UIText _valuePotentialByStrength;
    private UIText _valuePotentialByDexterity;
    private UIText _valuePotentialByIntelligence;
    private UIText _valuePotentialByFaith;
    
    private UIText _valueHpRegen;
    private UIText _valueManaRegen;
    #endregion
    
    private RingPlayer _ringPlayer;

    public void LoadData(RingPlayer ringPlayer)
    {
        _ringPlayer = ringPlayer;
        
        _playerDataSouls = _ringPlayer.Souls;
        _playerDataHumanity = _ringPlayer.Humanity;
        
        _stats = new Dictionary<PlayerStatsLevel, int>
        {
            { PlayerStatsLevel.PlayerVitality, _ringPlayer.LevelVitality },
            { PlayerStatsLevel.PlayerAttunement, _ringPlayer.LevelAttunement },
            { PlayerStatsLevel.PlayerStrength, _ringPlayer.LevelStrength },
            { PlayerStatsLevel.PlayerDexterity, _ringPlayer.LevelDexterity },
            { PlayerStatsLevel.PlayerIntelligence, _ringPlayer.LevelIntelligence },
            { PlayerStatsLevel.PlayerFaith, _ringPlayer.LevelFaith },
            { PlayerStatsLevel.PlayerResistance, _ringPlayer.LevelResistance },
        };
        _statsNoChange = new Dictionary<PlayerStatsLevel, int>
        {
            { PlayerStatsLevel.PlayerVitality, _ringPlayer.LevelVitality },
            { PlayerStatsLevel.PlayerAttunement, _ringPlayer.LevelAttunement },
            { PlayerStatsLevel.PlayerStrength, _ringPlayer.LevelStrength },
            { PlayerStatsLevel.PlayerDexterity, _ringPlayer.LevelDexterity },
            { PlayerStatsLevel.PlayerIntelligence, _ringPlayer.LevelIntelligence },
            { PlayerStatsLevel.PlayerFaith, _ringPlayer.LevelFaith },
            { PlayerStatsLevel.PlayerResistance, _ringPlayer.LevelResistance },
        };

        // _reqSoulsToLevelUp = StatFormulas.GetReqSoulsByLevel(Level + 1);
    }

    public override void OnInitialize()
    {
        _downLevelTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/minus", AssetRequestMode.ImmediateLoad).Value;
        _upLevelTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/plus", AssetRequestMode.ImmediateLoad).Value;
        
        _levelTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Level", AssetRequestMode.ImmediateLoad).Value;
        _soulsTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Souls", AssetRequestMode.ImmediateLoad).Value;
        _reqSoulsTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/ReqSouls", AssetRequestMode.ImmediateLoad).Value;
        _vitalityTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Vitality", AssetRequestMode.ImmediateLoad).Value;
        _attunementTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Attunement", AssetRequestMode.ImmediateLoad).Value;
        _strengthTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Strength", AssetRequestMode.ImmediateLoad).Value;
        _dexterityTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Dexterity", AssetRequestMode.ImmediateLoad).Value;
        _intelligenceTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Intelligence", AssetRequestMode.ImmediateLoad).Value;
        _faithTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Faith", AssetRequestMode.ImmediateLoad).Value;
        _resistanceTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Resistance", AssetRequestMode.ImmediateLoad).Value;
        _humanityTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Humanity", AssetRequestMode.ImmediateLoad).Value;
        _hpTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/HP", AssetRequestMode.ImmediateLoad).Value;
        _manaTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Mana", AssetRequestMode.ImmediateLoad).Value;
        _defenseTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Defense", AssetRequestMode.ImmediateLoad).Value;
        _debuffsResistanceTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/DebuffsResistance", AssetRequestMode.ImmediateLoad).Value;
        _luckTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Luck", AssetRequestMode.ImmediateLoad).Value;
        _potentialTexture = ModContent.Request<Texture2D>("TerraSouls/Assets/Textures/Potential", AssetRequestMode.ImmediateLoad).Value;

        _startPlaceAttributes = 60;
        
        AddLevelUp();

        //AddSelectClass();
        
        _acceptButton = new UIButton<string>("Accept")
        {
            Width = { Pixels = 80, Percent = 0f },
            Height = { Pixels = 40, Percent = 0f },
            Left = { Pixels = 370f, Percent = 0f },
            // Top = { Pixels = _startPlaceAttributes + 3, Percent = 0f },
            VAlign = 1f
        };
        _acceptButton.OnLeftClick += SaveChanges;
        _mainPanel.Append(_acceptButton);
        
        _closeButton = new UIButton<string>("Cancel")
        {
            Width = { Pixels = 80, Percent = 0f },
            Height = { Pixels = 40, Percent = 0f },
            Left = { Pixels = 460f, Percent = 0f },
            // Top = { Pixels = _startPlaceAttributes + 3, Percent = 0f },
            VAlign = 1f
        };
        _closeButton.OnLeftClick += (_, _) => Visible = false;
        _mainPanel.Append(_closeButton);
    }
    
    private void AddSelectClass()
    {
        const int totalColumns = 5;
        const int totalRows = 2;
        const float panelWidth = 200f;
        const float panelHeight = 160f;
        const float spacingX = 10f;
        const float spacingY = 10f;
        
        // Panel principal
        _mainPanel = new UIPanel
        {
            Width = { Pixels = totalColumns * panelWidth + (totalColumns + 1) * spacingX, Percent = 0f },
            Height = { Pixels = totalRows * panelHeight + (totalRows + 2) * spacingY + _closeButton.Height.Pixels, Percent = 0f },
            HAlign = 0.5f,
            VAlign = 0.5f,
        };
        Append(_mainPanel);
        
        for (var row = 0; row < totalRows; row++)
        {
            for (var col = 0; col < totalColumns; col++)
            {
                var index = row * totalColumns + col;

                var classPanel = new UIPanel();
                classPanel.Width.Set(panelWidth, 0f);
                classPanel.Height.Set(panelHeight, 0f);
                classPanel.Left.Set(col * (panelWidth + spacingX), 0f);
                classPanel.Top.Set(row * (panelHeight + spacingY), 0f);
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
        _mainPanel = new UIPanel // GridPanel or UIPanel
        {
            Width = { Pixels = 570, Percent = 0f },
            Height = { Pixels = 390, Percent = 0f },
            HAlign = 0.5f,
            VAlign = 0.5f,
            // PaddingPixels = 10,
            // CellSize = 20,
            // GridColor = Color.Gray * 0.5f,
            // BackgroundColor = new Color(16, 16, 10, 240),
        };
        Append(_mainPanel);
        
        var uiTitle = new UIText("Level Up", 1.3f)
        {
            // Left = { Pixels = 68, Percent = 0f },
            // Top = { Pixels = 5, Percent = 0f },
            Width = { Pixels = 250, Percent = 0f },
            Height = { Pixels = 25, Percent = 0f }
        };
        _mainPanel.Append(uiTitle);
        
        var uiDescription = new UIText("Select a parameter to boost")
        {
            // Left = { Pixels = 68, Percent = 0f },
            Top = {Pixels = 30, Percent = 0f},
            Width = { Pixels = 250, Percent = 0f },
            Height = { Pixels = 20, Percent = 0f }
        };
        _mainPanel.Append(uiDescription);
        
        AddLevelUi();
        AddSoulsUi();
        AddReqSoulsUi();
        AddVitalityUi();
        AddAttunementUi();
        AddStrUi();
        AddDexUi();
        AddIntUi();
        AddFaiUi();
        AddResistanceUi();
        AddStatLuck();
        AddHumanityUi();
        
        _startPlaceAttributes = 60;
        
        AddStatLife();
        AddStatMana();
        AddStatDefense();
        AddStatDebuffsResistance();
        AddStatPotentialByStrength();
        AddStatPotentialByDexterity();
        AddStatPotentialByIntelligence();
        AddStatPotentialByFaith();
        AddStatHpRegeneration();
        AddStatManaRegeneration();
    }
    
    private void SaveChanges(UIMouseEvent evt, UIElement listeningElement)
    {
        if (_ringPlayer.PlayerLevel == Level)
        {
            Visible = false;
            return;
        }
        
        _ringPlayer.DecreaseSouls(_reqSoulsToLevelUp);
        _ringPlayer.SetStats(_stats);
        
        Visible = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (!Main.gameMenu && Main.LocalPlayer == null)
        {
            return;
        }
        
        if (_mainPanel.ContainsPoint(Main.MouseScreen))
        {
            var player = Main.LocalPlayer;
            
            player.mouseInterface = true;
            player.cursorItemIconEnabled = false;
            player.cursorItemIconText = "Rest and increase stats";
            player.cursorItemIconID = -1;
            player.noThrow = 2;
        }
        
        _valueLevel.SetText(Level.ToString());
        _valuePlayerSouls.SetText(_playerDataSouls.ToString());
        _valueReqSouls.SetText(_reqSoulsToLevelUp.ToString());
        _valuePlayerHumanity.SetText(_playerDataHumanity.ToString());
        
        _valueVitality.SetText(_stats[PlayerStatsLevel.PlayerVitality].ToString());
        _valueAttunement.SetText(_stats[PlayerStatsLevel.PlayerAttunement].ToString());
        _valueStrength.SetText(_stats[PlayerStatsLevel.PlayerStrength].ToString());
        _valueDexterity.SetText(_stats[PlayerStatsLevel.PlayerDexterity].ToString());
        _valueIntelligence.SetText(_stats[PlayerStatsLevel.PlayerIntelligence].ToString());
        _valueFaith.SetText(_stats[PlayerStatsLevel.PlayerFaith].ToString());
        _valueResistance.SetText(_stats[PlayerStatsLevel.PlayerResistance].ToString());
        
        _valueVitality.TextColor = StatValueColor(_stats[PlayerStatsLevel.PlayerVitality], _statsNoChange[PlayerStatsLevel.PlayerVitality]);
        _valueAttunement.TextColor = StatValueColor(_stats[PlayerStatsLevel.PlayerAttunement], _statsNoChange[PlayerStatsLevel.PlayerAttunement]);
        _valueStrength.TextColor = StatValueColor(_stats[PlayerStatsLevel.PlayerStrength], _statsNoChange[PlayerStatsLevel.PlayerStrength]);
        _valueDexterity.TextColor = StatValueColor(_stats[PlayerStatsLevel.PlayerDexterity], _statsNoChange[PlayerStatsLevel.PlayerDexterity]);
        _valueIntelligence.TextColor = StatValueColor(_stats[PlayerStatsLevel.PlayerIntelligence], _statsNoChange[PlayerStatsLevel.PlayerIntelligence]);
        _valueFaith.TextColor = StatValueColor(_stats[PlayerStatsLevel.PlayerFaith], _statsNoChange[PlayerStatsLevel.PlayerFaith]);
        _valueResistance.TextColor = StatValueColor(_stats[PlayerStatsLevel.PlayerResistance], _statsNoChange[PlayerStatsLevel.PlayerResistance]);

        var hpVisual = StatFormulas.GetLifeByVitality(_stats[PlayerStatsLevel.PlayerVitality]);
        var hpReal = StatFormulas.GetLifeByVitality(_statsNoChange[PlayerStatsLevel.PlayerVitality]);
        
        var manaVisual = StatFormulas.GetManaByAttunement(_stats[PlayerStatsLevel.PlayerAttunement]);
        var manaReal = StatFormulas.GetManaByAttunement(_statsNoChange[PlayerStatsLevel.PlayerAttunement]);
        
        double defenseVisual = StatFormulas.GetDefenseByResistance(_stats[PlayerStatsLevel.PlayerResistance]);
        double defenseReal = StatFormulas.GetDefenseByResistance(_statsNoChange[PlayerStatsLevel.PlayerResistance]);
        
        double debuffsResistanceVisual = StatFormulas.GetDebuffsResistanceByResistance(_stats[PlayerStatsLevel.PlayerResistance]);
        double debuffsResistanceReal = StatFormulas.GetDebuffsResistanceByResistance(_statsNoChange[PlayerStatsLevel.PlayerResistance]);
        
        _valueHp.SetText($"{_ringPlayer.Player.statLifeMax2} > {_ringPlayer.Player.statLifeMax2 + hpVisual - hpReal}");
        _valueMana.SetText($"{_ringPlayer.Player.statManaMax2} > {_ringPlayer.Player.statManaMax2 + manaVisual - manaReal}");
        _valueDefense.SetText($"{Math.Round(defenseReal * 100, 2)}% > {Math.Round(defenseVisual * 100, 2)}%");
        _valueDebuffsResistance.SetText($"{Math.Round(debuffsResistanceReal * 100, 2)}% > {Math.Round(debuffsResistanceVisual * 100, 2)}%");
        
        _valueLuck.SetText($"{Math.Round(_ringPlayer.Player.luck * 100, 2)}%");
        
        _valuePotentialByStrength.SetText($"{Math.Round(StatFormulas.GetPotentialByStrength(_stats[PlayerStatsLevel.PlayerStrength]) * 100, 2)}%");
        _valuePotentialByDexterity.SetText($"{Math.Round(StatFormulas.GetPotentialByDexterity(_stats[PlayerStatsLevel.PlayerDexterity]) * 100, 2)}%");
        _valuePotentialByIntelligence.SetText($"{Math.Round(StatFormulas.GetPotentialByIntelligence(_stats[PlayerStatsLevel.PlayerIntelligence]) * 100, 2)}%");
        _valuePotentialByFaith.SetText($"{Math.Round(StatFormulas.GetPotentialByFaith(_stats[PlayerStatsLevel.PlayerFaith]) * 100, 2)}%");
        
        _valueHpRegen.SetText($"{Math.Round(_ringPlayer.Player.lifeRegen / 2d, 0)}/s");
        _valueManaRegen.SetText($"{Math.Round(_ringPlayer.Player.manaRegen / 2d, 0)}/s");
        
        // Close key ESC
        // if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
        // {
        //     _system.HideUI();
        //     Main.blockInput = false;
        // }

        _increaseAttunement.Visible = CanLevelUp;
        _increaseVitality.Visible = CanLevelUp;
        _increaseResistance.Visible = CanLevelUp;
        _increaseStrength.Visible = CanLevelUp;
        _increaseDexterity.Visible = CanLevelUp;
        _increaseIntelligence.Visible = CanLevelUp;
        _increaseFaith.Visible = CanLevelUp;
    }

    private static Color StatValueColor(int currentValue, int playerValue)
    {
        if (currentValue < playerValue)
        {
            return Color.Crimson;
        }
        return currentValue == playerValue ? Color.White : Color.DodgerBlue;
    }

    private void AddLevelUi()
    {
        var uiLevel = new UIImage(_levelTexture)
        {
            Left = { Pixels = 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiLevel);
        
        var uiLevelText = new UiTextCustom("Level")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiLevelText);

        _valueLevel = new UiTextCustom("0")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 120, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valueLevel);

        _startPlaceAttributes += 25;
    }
    
    private void AddSoulsUi()
    {
        var uiSouls = new UIImage(_soulsTexture)
        {
            Left = { Pixels = 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("Souls")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valuePlayerSouls = new UiTextCustom("0")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 120, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valuePlayerSouls);

        _startPlaceAttributes += 25;
    }
    
    private void AddReqSoulsUi()
    {
        var uiSouls = new UIImage(_reqSoulsTexture)
        {
            Left = { Pixels = 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("Req. Souls")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valueReqSouls = new UiTextCustom("0")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 120, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valueReqSouls);
        
        _startPlaceAttributes += 25;
    }
    
    private void AddHumanityUi()
    {
        var uiSouls = new UIImage(_humanityTexture)
        {
            Left = { Pixels = 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("Humanity")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valuePlayerHumanity = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 120, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valuePlayerHumanity);

        _startPlaceAttributes += 25;
    }
    
    private void AddStatLife()
    {
        var uiSouls = new UIImage(_hpTexture)
        {
            Left = { Pixels = 260 + 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("HP")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valueHp = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 150, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valueHp);

        _startPlaceAttributes += 25;
    }
    
    private void AddStatMana()
    {
        var uiSouls = new UIImage(_manaTexture)
        {
            Left = { Pixels = 260 + 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("Mana")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valueMana = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 150, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valueMana);

        _startPlaceAttributes += 25;
    }
    
    private void AddStatDefense()
    {
        var uiSouls = new UIImage(_defenseTexture)
        {
            Left = { Pixels = 260 + 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("Defense")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valueDefense = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 150, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valueDefense);

        _startPlaceAttributes += 25;
    }
    
    private void AddStatDebuffsResistance()
    {
        var uiSouls = new UIImage(_debuffsResistanceTexture)
        {
            Left = { Pixels = 260 + 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("Debuffs Res.")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valueDebuffsResistance = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 150, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valueDebuffsResistance);

        _startPlaceAttributes += 25;
    }
    
    private void AddStatPotentialByStrength()
    {
        var uiSouls = new UIImage(_potentialTexture)
        {
            Left = { Pixels = 260 + 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("Pot. by STR")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valuePotentialByStrength = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 150, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valuePotentialByStrength);

        _startPlaceAttributes += 25;
    }
    
    private void AddStatPotentialByDexterity()
    {
        var uiSouls = new UIImage(_potentialTexture)
        {
            Left = { Pixels = 260 + 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("Pot. by DEX")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valuePotentialByDexterity = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 150, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valuePotentialByDexterity);

        _startPlaceAttributes += 25;
    }
    
    private void AddStatPotentialByIntelligence()
    {
        var uiSouls = new UIImage(_potentialTexture)
        {
            Left = { Pixels = 260 + 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("Pot. by INT")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valuePotentialByIntelligence = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 150, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valuePotentialByIntelligence);

        _startPlaceAttributes += 25;
    }
    
    private void AddStatPotentialByFaith()
    {
        var uiSouls = new UIImage(_potentialTexture)
        {
            Left = { Pixels = 260 + 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("Pot. by FAI")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valuePotentialByFaith = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 150, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valuePotentialByFaith);

        _startPlaceAttributes += 25;
    }
    
    private void AddStatHpRegeneration()
    {
        var uiSouls = new UIImage(_potentialTexture)
        {
            Left = { Pixels = 260 + 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("HP Regen.")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valueHpRegen = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 150, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valueHpRegen);

        _startPlaceAttributes += 25;
    }
    
    private void AddStatManaRegeneration()
    {
        var uiSouls = new UIImage(_potentialTexture)
        {
            Left = { Pixels = 260 + 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("Mana Regen.")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valueManaRegen = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 260 + 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 150, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valueManaRegen);

        _startPlaceAttributes += 25;
    }
    
    private void AddStatLuck()
    {
        var uiSouls = new UIImage(_luckTexture)
        {
            Left = { Pixels = 0, Percent = 0f },
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        _mainPanel.Append(uiSouls);
        
        var uiSoulsText = new UiTextCustom("Luck")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        _mainPanel.Append(uiSoulsText);

        _valueLuck = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 130, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 120, Percent = 0f },
            HAlignText = HorizontalAlignment.Right
        };
        _mainPanel.Append(_valueLuck);

        _startPlaceAttributes += 25;
    }

    private void AddVitalityUi()
    {
        var uiImg = new UIImage(_vitalityTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        
        var uiText = new UiTextCustom("Vitality")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        
        _decreaseVitality = new HoverImage(_downLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 160, Percent = 0f },
            
            Action = OnClickAction.ActionDecrease,
            PlayerStat = PlayerStatsLevel.PlayerVitality
        };
        
        _valueVitality = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 185, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 40, Percent = 0f },
            HAlignText = HorizontalAlignment.Center
        };
        
        _increaseVitality = new HoverImage(_upLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 225, Percent = 0f },
            
            Action = OnClickAction.ActionIncrease,
            PlayerStat = PlayerStatsLevel.PlayerVitality
        };
        
        _mainPanel.Append(uiImg);
        _mainPanel.Append(uiText);
        _mainPanel.Append(_decreaseVitality);
        _mainPanel.Append(_valueVitality);
        _mainPanel.Append(_increaseVitality);

        _startPlaceAttributes += 25;
    }
    
    private void AddAttunementUi()
    {
        var uiImg = new UIImage(_attunementTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        
        var uiText = new UiTextCustom("Attunement")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        
        _decreaseAttunement = new HoverImage(_downLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 160, Percent = 0f },
            Action = OnClickAction.ActionDecrease,
            PlayerStat = PlayerStatsLevel.PlayerAttunement
        };
        
        _valueAttunement = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 185, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 40, Percent = 0f },
            HAlignText = HorizontalAlignment.Center
        };
        
        _increaseAttunement = new HoverImage(_upLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 225, Percent = 0f },
            Action = OnClickAction.ActionIncrease,
            PlayerStat = PlayerStatsLevel.PlayerAttunement
        };
        
        _mainPanel.Append(uiImg);
        _mainPanel.Append(uiText);
        _mainPanel.Append(_decreaseAttunement);
        _mainPanel.Append(_valueAttunement);
        _mainPanel.Append(_increaseAttunement);

        _startPlaceAttributes += 25;
    }
    
    private void AddStrUi()
    {
        var uiImg = new UIImage(_strengthTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        
        var uiText = new UiTextCustom("Strength")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        
        _decreaseStrength = new HoverImage(_downLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 160, Percent = 0f },
            Action = OnClickAction.ActionDecrease,
            PlayerStat = PlayerStatsLevel.PlayerStrength
        };
        
        _valueStrength = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 185, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 40, Percent = 0f },
            HAlignText = HorizontalAlignment.Center
        };
        
        _increaseStrength = new HoverImage(_upLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 225, Percent = 0f },
            Action = OnClickAction.ActionIncrease,
            PlayerStat = PlayerStatsLevel.PlayerStrength
        };
        
        _mainPanel.Append(uiImg);
        _mainPanel.Append(uiText);
        _mainPanel.Append(_decreaseStrength);
        _mainPanel.Append(_valueStrength);
        _mainPanel.Append(_increaseStrength);

        _startPlaceAttributes += 25;
    }
    
    private void AddDexUi()
    {
        var uiImg = new UIImage(_dexterityTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        
        var uiText = new UiTextCustom("Dexterity")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        
        _decreaseDexterity = new HoverImage(_downLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 160, Percent = 0f },
            Action = OnClickAction.ActionDecrease,
            PlayerStat = PlayerStatsLevel.PlayerDexterity
        };
        
        _valueDexterity = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 185, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 40, Percent = 0f },
            HAlignText = HorizontalAlignment.Center
        };
        
        _increaseDexterity = new HoverImage(_upLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 225, Percent = 0f },
            Action = OnClickAction.ActionIncrease,
            PlayerStat = PlayerStatsLevel.PlayerDexterity
        };
        
        _mainPanel.Append(uiImg);
        _mainPanel.Append(uiText);
        _mainPanel.Append(_decreaseDexterity);
        _mainPanel.Append(_valueDexterity);
        _mainPanel.Append(_increaseDexterity);

        _startPlaceAttributes += 25;
    }
    
    private void AddIntUi()
    {
        var uiImg = new UIImage(_intelligenceTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        
        var uiText = new UiTextCustom("Intelligence")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        
        _decreaseIntelligence = new HoverImage(_downLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 160, Percent = 0f },
            Action = OnClickAction.ActionDecrease,
            PlayerStat = PlayerStatsLevel.PlayerIntelligence
        };
        
        _valueIntelligence = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 185, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 40, Percent = 0f },
            HAlignText = HorizontalAlignment.Center
        };
        
        _increaseIntelligence = new HoverImage(_upLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 225, Percent = 0f },
            Action = OnClickAction.ActionIncrease,
            PlayerStat = PlayerStatsLevel.PlayerIntelligence
        };
        
        _mainPanel.Append(uiImg);
        _mainPanel.Append(uiText);
        _mainPanel.Append(_decreaseIntelligence);
        _mainPanel.Append(_valueIntelligence);
        _mainPanel.Append(_increaseIntelligence);

        _startPlaceAttributes += 25;
    }
    
    private void AddFaiUi()
    {
        var uiImg = new UIImage(_faithTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        
        var uiText = new UiTextCustom("Faith")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        
        _decreaseFaith = new HoverImage(_downLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 160, Percent = 0f },
            Action = OnClickAction.ActionDecrease,
            PlayerStat = PlayerStatsLevel.PlayerFaith
        };
        
        _valueFaith = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 185, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 40, Percent = 0f },
            HAlignText = HorizontalAlignment.Center
        };
        
        _increaseFaith = new HoverImage(_upLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 225, Percent = 0f },
            Action = OnClickAction.ActionIncrease,
            PlayerStat = PlayerStatsLevel.PlayerFaith
        };
        
        _mainPanel.Append(uiImg);
        _mainPanel.Append(uiText);
        _mainPanel.Append(_decreaseFaith);
        _mainPanel.Append(_valueFaith);
        _mainPanel.Append(_increaseFaith);

        _startPlaceAttributes += 25;
    }
    
    private void AddResistanceUi()
    {
        var uiImg = new UIImage(_resistanceTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
        };
        
        var uiText = new UiTextCustom("Resistance")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 30, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 100, Percent = 0f },
        };
        
        _decreaseResistance = new HoverImage(_downLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 160, Percent = 0f },
            Action = OnClickAction.ActionDecrease,
            PlayerStat = PlayerStatsLevel.PlayerResistance
        };
        
        _valueResistance = new UiTextCustom("-")
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 185, Percent = 0f },
            Height = { Pixels = 24, Percent = 0f },
            Width = { Pixels = 40, Percent = 0f },
            HAlignText = HorizontalAlignment.Center
        };
        
        _increaseResistance = new HoverImage(_upLevelTexture)
        {
            Top = { Pixels = _startPlaceAttributes, Percent = 0f },
            Left = { Pixels = 225, Percent = 0f },
            Action = OnClickAction.ActionIncrease,
            PlayerStat = PlayerStatsLevel.PlayerResistance
        };
        
        _mainPanel.Append(uiImg);
        _mainPanel.Append(uiText);
        _mainPanel.Append(_decreaseResistance);
        _mainPanel.Append(_valueResistance);
        _mainPanel.Append(_increaseResistance);

        _startPlaceAttributes += 25;
    }
    
    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);

        if (evt.Target is not HoverImage hoverImage)
        {
            return;
        }

        switch (hoverImage.Action)
        {
            case OnClickAction.ActionDecrease:
                TryDecreaseStat(hoverImage.PlayerStat);
                break;
            case OnClickAction.ActionIncrease:
                TryIncreaseStat(hoverImage.PlayerStat);
                break;
            default:
                return;
        }
    }

    private void TryIncreaseStat(PlayerStatsLevel statName)
    {
        if (_stats[statName] >= 99)
        {
            return;
        }
        
        var cost = StatFormulas.GetReqSoulsByLevel(Level + 1);

        if (_playerDataSouls < cost)
        {
            return;
        }
        if (_playerDataSouls < _reqSoulsToLevelUp + cost)
        {
            return;
        }
        
        _stats[statName] += 1;
        // _playerDataSouls -= cost;
        _reqSoulsToLevelUp += cost;
    }

    private void TryDecreaseStat(PlayerStatsLevel statName)
    {
        if (_stats[statName] <= 0)
        {
            return;
        }
        
        if (_stats[statName] <= _statsNoChange[statName])
        {
            return;
        }

        var cost = StatFormulas.GetReqSoulsByLevel(Level);
        _stats[statName] -= 1;
        // _playerDataSouls += cost;
        _reqSoulsToLevelUp -= cost;
    }

    private int Level => _stats[PlayerStatsLevel.PlayerVitality] + _stats[PlayerStatsLevel.PlayerAttunement] + 
                         _stats[PlayerStatsLevel.PlayerStrength] + _stats[PlayerStatsLevel.PlayerDexterity] + 
                         _stats[PlayerStatsLevel.PlayerIntelligence] + _stats[PlayerStatsLevel.PlayerFaith] + 
                         _stats[PlayerStatsLevel.PlayerResistance];


    public void UnloadResources()
    {
        _mainPanel = null;
        _acceptButton = null;
        _closeButton = null;
        _startPlaceAttributes = 0;

        _playerDataSouls = 0;
        _playerDataHumanity = 0;
        _stats = null;
        _statsNoChange = null;
        _reqSoulsToLevelUp = 0;
        _valuePlayerSouls = null;
        _valuePlayerHumanity = null;

        //RingPlayer = null;
        
        _valueLevel = null;
        _valueReqSouls = null;
        _increaseVitality = null;
        _decreaseVitality = null;
        _valueVitality = null;
        _increaseAttunement = null;
        _decreaseAttunement = null;
        _valueAttunement = null;
        _increaseStrength = null;
        _decreaseStrength = null;
        _valueStrength = null;
        _increaseDexterity = null;
        _decreaseDexterity = null;
        _valueDexterity = null;
        _increaseIntelligence = null;
        _decreaseIntelligence = null;
        _valueIntelligence = null;
        _increaseFaith = null;
        _decreaseFaith = null;
        _valueFaith = null;
        _increaseResistance = null;
        _decreaseResistance = null;
        _valueResistance = null;
        
        _downLevelTexture = null;
        _upLevelTexture = null;
        
        _levelTexture = null;
        _soulsTexture = null;
        _reqSoulsTexture = null;
        _vitalityTexture = null;
        _attunementTexture = null;
        _strengthTexture = null;
        _dexterityTexture = null;
        _intelligenceTexture = null;
        _faithTexture = null;
        _resistanceTexture = null;
        _humanityTexture = null;
        _hpTexture = null;
        _manaTexture = null;
        _defenseTexture = null;
        _debuffsResistanceTexture = null;
        _luckTexture = null;
        
        _valueHp = null;
        _valueMana = null;
        _valueDefense = null;
        _valueDebuffsResistance = null;
        _valueLuck = null;
        _valuePotentialByStrength = null;
        _valuePotentialByDexterity = null;
        _valuePotentialByIntelligence = null;
        _valuePotentialByFaith = null;
    }

    public override bool Visible { get; set; }

    public override int InsertionIndex(List<GameInterfaceLayer> layers)
    {
        return layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
    }
    
    public List<(int levelToAdd, long cost)> GetLevelUpPreview(int currentLevel, long availableSouls)
    {
        List<(int, long)> preview = [];
        var level = currentLevel;
        var lastCost = 0L;

        while (true)
        {
            var cost = StatFormulas.GetReqSoulsByLevel(level + 1);
            if (availableSouls >= cost)
            {
                lastCost += cost;
                preview.Add((level + 1, lastCost));
                availableSouls -= cost;
                level++;
            }
            else
            {
                break;
            }
        }

        return preview;
    }

    private bool CanLevelUp => _playerDataSouls > _reqSoulsToLevelUp + StatFormulas.GetReqSoulsByLevel(Level + 1);

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        
        // var preview = GetLevelUpPreview(Level, _playerDataSouls - _reqSoulsToLevelUp);
        //
        // var startPosition = new Vector2(100, 50);
        // const int lineHeight = 20;
        //
        // for (var i = 0; i < preview.Count; i++)
        // {
        //     var (level, cost) = preview[i];
        //     var text = $"Level {level}: {cost} souls";
        //     Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, text, startPosition.X, startPosition.Y + i * lineHeight, Color.Gold, Color.Black, Vector2.Zero);
        // }
    }
}