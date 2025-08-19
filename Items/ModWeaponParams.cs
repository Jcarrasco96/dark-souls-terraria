using System.Collections.Generic;
using Terraria.ModLoader;
using TerraSouls.Commons;

namespace TerraSouls.Items;

public abstract class ModWeaponParams : ModItem
{
    public WeaponParams WeaponParams = new();

    protected string Description = "";
    
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        tooltips.Add(new TooltipLine(Mod, "DescriptionParams", Description));
        base.ModifyTooltips(tooltips);
    }
    
}