using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CustomRecipes.Bonfire;

public class BonfireCommand : ModCommand
{
    public override void Action(CommandCaller caller, string input, string[] args)
    {
        // throw new System.NotImplementedException();
        
        if (args.Length == 0 || !int.TryParse(args[0], out var index) || index < 1)
        {
            Main.NewText("Uso: /bonfire [número]", Color.Red);
            return;
        }

        ModContent.GetInstance<RingSystem>().TeleportToBonfire(caller.Player, index - 1); // Índice 0-based
    }

    public override string Command => "bonfire";
    public override CommandType Type => CommandType.Chat;
    
}