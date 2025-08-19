using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraSouls.Items;

public abstract class ConsumableSoul : ModItem
{
    protected string Description = "";
    protected int Souls = 0;
    
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        if (Description != "")
        {
            tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        }
        base.ModifyTooltips(tooltips);
    }
    
    public override void SetDefaults()
    {
        base.SetDefaults();
        
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
    
    public override void OnConsumeItem(Player player)
    {
        base.OnConsumeItem(player);

        if (Souls > 0)
        {
            player.GetModPlayer<RingPlayer>().AddSouls(Souls);
        }
    }

    public override bool ConsumeItem(Player player)
    {
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