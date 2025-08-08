using CustomRecipes.Rings;
using Terraria;
using Terraria.ModLoader;

namespace CustomRecipes;

public class CustomRingSlot : ModAccessorySlot
{

    public override string Name => "RingSlot"; // Identificador interno
    public override bool DrawVanitySlot => false;

    public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
    {
        // Solo permite ítems que sean de tipo ModRing
        return checkItem.ModItem is ModRing;
    }

    public override bool IsVisibleWhenNotEnabled() => true;
   
}