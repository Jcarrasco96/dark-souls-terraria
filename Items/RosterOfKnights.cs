using System.Collections.Generic;
using TerraSouls.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Items;

// ReSharper disable once ClassNeverInstantiated.Global
public class RosterOfKnights : ModItem
{
    
    private const string Description = """
                                       Online play item. A roster of knights of the Darkmoon who have served since the
                                       age of the old Royals. Use to discover the names of Darkmoon Knights, an order of
                                       elite knights shrouded in shadows.
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
        if (player.whoAmI != Main.myPlayer)
        {
            return false;
        }

        var sys = ModContent.GetInstance<RingSystem>();
        sys.ShowLevelUpUi();
        return true;

    }
}