using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Items;

public class Darksign : ModItem
{
    private const string Description = """
                                       Returns you to the last spawn used at the cost of deleting all of your current
                                       souls. The Darksign returns its bearer to the last bonfire rested at, or the
                                       bonfire at Firelink Shrine, but at the cost of all souls held. Carriers of the
                                       Darksign are reborn after death, and eventually lose their minds, turning Hollow.
                                       And so it is they are driven from their homeland.
                                       """;
    
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.value = Item.buyPrice(1);
        Item.shopCustomPrice = Item.buyPrice(1);
    }

    public override bool? UseItem(Player player)
    {
        var modPlayer = player.GetModPlayer<RingPlayer>();
        
        modPlayer.SpawnUsingDarkSign();

        // if (Main.netMode == NetmodeID.MultiplayerClient)
        // {
        //     NetMessage.SendData(MessageID.Teleport, -1, -1, null, player.whoAmI, spawnPosition.X, spawnPosition.Y, TeleportationStyleID.RodOfDiscord);
        // }

        return true;
    }
}