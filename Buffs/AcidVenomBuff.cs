using Terraria;
using Terraria.ModLoader;

namespace CustomRecipes.Buffs;

public class AcidVenomBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        // DisplayName.SetDefault("Acid Venom");
        // Description.SetDefault("Losing health due to acid venom");
        Main.debuff[Type] = true; // Marca como debuff
        Main.pvpBuff[Type] = false; // No se aplica en PVP a jugadores
        Main.buffNoSave[Type] = true; // No se guarda en partidas
        Main.buffNoTimeDisplay[Type] = false; // Muestra duración
    }
}