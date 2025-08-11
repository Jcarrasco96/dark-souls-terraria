using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CustomRecipes.Bonfire;

public class ResetStatsCommand : ModCommand
{
    public override void Action(CommandCaller caller, string input, string[] args)
    {
        if (args.Length != 0)
        {
            Main.NewText("Uso: /reset-stats", Color.Red);
        }

        // var tag = new TagCompound
        // {
        //     // ["dsSouls"] = 0,
        //     ["dsVitality"] = 0,
        //     ["dsAttunement"] = 0,
        //     // tag["dsEndurance"] = dsEndurance;
        //     ["dsStrength"] = 0,
        //     ["dsDexterity"] = 0,
        //     // tag["dsResistance"] = dsResistance;
        //     ["dsIntelligence"] = 0,
        //     ["dsFaith"] = 0,
        //     ["dsHumanity"] = 0
        // };
        
        // var ringPlayer = Main.LocalPlayer.GetModPlayer<RingPlayer>();
        
        // ringPlayer.SaveData(tag);

        // ringPlayer.DsSouls = 0;
        // ringPlayer.DsStrength = 0;
        // ringPlayer.DsDexterity = 0;
        // ringPlayer.DsIntelligence = 0;
        // ringPlayer.DsFaith = 0;
    }

    public override string Command => "reset-stats";
    public override CommandType Type => CommandType.Chat;
}