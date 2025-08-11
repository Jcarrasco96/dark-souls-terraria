using CustomRecipes.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CustomRecipes.Items;

public class BookStats : ModItem
{
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

        var sys = ModContent.GetInstance<MyUiSystem>();
        if (sys.UiVisible)
        {
            return false;
        }

        sys.ShowClassUi();
        return true;

    }
}