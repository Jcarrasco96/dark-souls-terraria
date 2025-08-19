using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using ReLogic.Graphics;

namespace TerraSouls.UI;

public class BossUiManager
{
    private static float _alpha;
    private static string _currentMessage = "";
    private static int _timer;
    private static Color _color = Color.White;
    private static bool _shakeEffect;
    
    private const int FadeTime = 1 * 60;
    private const int DisplayTime = 5 * 60;
    private const float ShakeAmount = 2f;
    private const float TextScale = 0.5f;
    
    public static void ShowBossIntro(int type)
    {
        Reset();
        _currentMessage = GetBossIntroMessage(type);
        _shakeEffect = true;
        _color = Color.Firebrick;
    }
    
    public static void ShowMessage(string message, Color color, bool shake = false)
    {
        Reset();
        _currentMessage = message;
        _color = color;
        _shakeEffect = shake;
    }
    
    public static void ShowBossDefeated()
    {
        Reset();
        _currentMessage = "VICTORY ACHIEVED";
        _color = new Color(248, 197, 77);
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        if (_timer <= 0)
        {
            return;
        }

        _timer--;

        switch (_timer)
        {
            case > DisplayTime + FadeTime:
                _alpha += 1f / FadeTime;
                break;
            
            case < FadeTime:
                _alpha -= 1f / FadeTime;
                break;
        }
        
        var font = TerraSouls.OptimusPrincepsFont;
        var textSize = font.MeasureString(_currentMessage) * TextScale;
        var position = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 3f) - textSize / 2f;

        if (_shakeEffect)
        {
            var shakeOffset = new Vector2(
                (float)Math.Sin(Main.GlobalTimeWrappedHourly * 30f) * ShakeAmount + Main.rand.NextFloat(-ShakeAmount, ShakeAmount),
                (float)Math.Cos(Main.GlobalTimeWrappedHourly * 25f) * ShakeAmount + Main.rand.NextFloat(-ShakeAmount, ShakeAmount)
            );
            
            position += shakeOffset;
        }

        spriteBatch.DrawString(font, _currentMessage, position + new Vector2(2, 2), Color.Black * _alpha, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
        spriteBatch.DrawString(font, _currentMessage, position, _color * _alpha, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
    }
    
    private static string GetBossIntroMessage(int bossType)
    {
        return bossType switch
        {
            NPCID.EyeofCthulhu => "The Eye of Cthulhu has appeared...",
            NPCID.KingSlime => "A giant King Slime approaches...",
            NPCID.EaterofWorldsHead => "The Eater of Worlds roars from the depths...",
            NPCID.BrainofCthulhu => "The Brain of Cthulhu distorts reality...",
            NPCID.QueenBee => "The Queen Bee buzzes furiously!",
            NPCID.SkeletronHead => "Skeletron awakens from its prison...",
            NPCID.Deerclops => "A freezing roar heralds the arrival of Deerclops...",
            NPCID.WallofFlesh => "Living Flesh emerges from the Underworld!",
            NPCID.QueenSlimeBoss => "The crystalline Queen Slime descends...",
            NPCID.TheDestroyer => "The metallic Destroyer advances...",
            NPCID.SkeletronPrime => "Skeletron Prime appears with mechanical fury...",
            NPCID.Retinazer => "Retinazer fixes its laser sights on you...",
            NPCID.Spazmatism => "Spazmatism breathes flame and malice...",
            NPCID.Plantera => "Plantera blooms in the darkness...",
            NPCID.Golem => "Golem awakens in the Jungle Temple...",
            NPCID.DukeFishron => "Duke Fishron emerges from the depths...",
            NPCID.CultistBoss => "The Lunatic Cultist summons arcane energies...",
            NPCID.MoonLordCore => "The Moon Lord manifests...",
            _ => "⚔️ A threat approaches!"
        };
    }

    private static void Reset()
    {
        _currentMessage = "";
        _alpha = 0f;
        _timer = FadeTime * 2 + DisplayTime;
        _shakeEffect = false;
        _color = Color.White;
    }
}