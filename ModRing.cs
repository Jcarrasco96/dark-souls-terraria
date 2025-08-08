using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CustomRecipes;

public abstract class ModRing : ModItem
{

    public override void SetDefaults()
    {
        Item.accessory = true;
        
        Item.width = 32;
        Item.height = 32;
        Item.rare = ItemRarityID.Blue;
        Item.value = 7500;
    }

    public virtual void ApplyEffects(Player player)
    {
        // Puedes poner efectos genéricos aquí, o dejar vacío
        
        // ApplyPrefixEffects(Item, player);
    }

    public override bool CanResearch()
    {
        return true;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        base.ModifyTooltips(tooltips);
        
        tooltips.Add(new TooltipLine(Mod, "RingOnly", "[c/AAAAAA:Solo se puede equipar en ranuras de anillos]"));
    }

    public override int ChoosePrefix(UnifiedRandom rand)
    {
        // Solo permitir prefijos de accesorios
        return rand.Next([
            PrefixID.Lucky,
            PrefixID.Menacing,
            PrefixID.Quick,
            PrefixID.Violent,
            PrefixID.Arcane,
            PrefixID.Warding,
        ]);
    }

    public override bool CanEquipAccessory(Player player, int slot, bool modded)
    {
        // return base.CanEquipAccessory(player, slot, modded);
        return false;
    }

    protected void ApplyPrefixEffects(Item item, Player player)
    {
        if (item.prefix == 0)
        {
            return;
        }

        // Aumentos generales de daño por tipo
        player.GetDamage(DamageClass.Generic) += item.GetPrefixDamageBoost();

        // Aumentos de defensa
        player.statDefense += item.GetPrefixDefenseBoost();

        // Aumentos de velocidad de movimiento
        player.moveSpeed += item.GetPrefixMoveSpeedBoost();

        // Aumentos de regeneración de maná
        player.manaRegenBonus += item.GetPrefixManaRegenBoost();

        // Puedes agregar más efectos si quieres simular cosas como Warding (+4 defensa), Arcane (+20 mana), etc.
    }
    
}