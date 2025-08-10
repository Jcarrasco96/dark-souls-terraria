using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CustomRecipes;

// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
// ReSharper disable once ClassNeverInstantiated.Global
public class CustomRecipes : Mod
{

    public static DynamicSpriteFont OptimusPrincepsFont;
    public static SoundStyle DsThruDeath;

    public override void Load()
    {
        base.Load();
        
        OptimusPrincepsFont = ModContent.Request<DynamicSpriteFont>("CustomRecipes/Fonts/OptimusPrinceps", AssetRequestMode.ImmediateLoad).Value;
        
        DsThruDeath = new SoundStyle("CustomRecipes/Sounds/DS_ThruDeath") { Volume = 0.7f };
    }

    public override void Unload()
    {
        base.Unload();
        
        OptimusPrincepsFont = null;
        
        DsThruDeath = default;
    }
    
}

