using System.Collections.Generic;
using Terraria.UI;

namespace TerraSouls.UI.Elements;

public abstract class CustomUiState : UIState
{
    public virtual bool Visible { get; set; }
    
    public abstract int InsertionIndex(List<GameInterfaceLayer> layers);
    
    public virtual InterfaceScaleType Scale { get; set; } = InterfaceScaleType.UI;
    
    protected internal virtual UserInterface UserInterface { get; set; }
}