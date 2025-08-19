using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Items;

public class Humanity : ModItem
{
    private const string Description = """
                                       Rare tiny black sprite found on corpses. Use to gain 1 humanity
                                       and restore a large amount of HP.
                                       This black sprite is called humanity, but little is known about
                                       its true nature. If the soul is the source of all life, then what
                                       distinguishes the humanity we hold within ourselves?
                                       """;
    
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }

    public override void SetDefaults()
    {
        Item.width = 64;
        Item.height = 64;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useAnimation = 17;
        Item.useTime = 17;
        Item.useTurn = true;
        Item.UseSound = SoundID.Item3;
        Item.maxStack = Item.CommonMaxStack;
        Item.consumable = true;
        Item.value = Item.buyPrice(gold: 1);
        Item.rare = ItemRarityID.Orange;
    }
    
    public override bool ConsumeItem(Player player)
    {
        var modPlayer = player.GetModPlayer<RingPlayer>();

        modPlayer.Humanity++;
        player.Heal(100);
        
        return true;
    }

    public override bool? UseItem(Player player)
    {
        return true;
    }
    
    public override void Update(ref float gravity, ref float maxFallSpeed)
    {
        var pulse = 0.15f + Math.Clamp((float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f) * Main.rand.NextFloat(0.8f, 1.1f), 0.5f, 1f);
        var rgb = new Vector3(0.2f, 0.2f, 0.2f) * pulse;
        Lighting.AddLight(Item.Center, rgb);
    }
}