using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CustomRecipes.Bonfire;

public class BonfireTile : ModTile
{
    
    public override void SetStaticDefaults()
    {
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileWaterDeath[Type] = true;
        Main.tileLavaDeath[Type] = true;

        // Main.tileSolid[Type] = false;
        // Main.tileMergeDirt[Type] = false;
        
        AnimationFrameHeight = 52;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
        TileObjectData.newTile.CoordinateHeights = [16, 16, 20];
        TileObjectData.newTile.DrawYOffset = 2;
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(200, 150, 50), Language.GetText("Mods.CustomRecipes.Tiles.DarkSoulsBonfire.DisplayName"));

        // TileID.Sets.HasOutlines[Type] = true;
    }

    public override bool RightClick(int i, int j)
    {
        if (Main.netMode != NetmodeID.Server) // Solo en cliente
        {
            ModContent.GetInstance<RingSystem>().ShowUi();
        }
        
        //BonfireSystem.OpenBonfireMenu();
        return true;
    }

    public override void MouseOver(int i, int j)
    {
        var player = Main.LocalPlayer;
        player.cursorItemIconText = "Teletransportarse";
        player.cursorItemIconID = ModContent.ItemType<BonfireItem>();
        player.noThrow = 2;
    }
    
    public override void AnimateTile(ref int frame, ref int frameCounter)
    {
        if (++frameCounter < 4)
        {
            return;
        }

        frameCounter = 0;
        frame = (frame + 1) % 24;
    }
    
    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        var tile = Main.tile[i, j];
        if (tile.TileFrameY >= 52)
        {
            return;
        }
    
        var pulse = Main.rand.NextFloat(0.05f, 0.15f) + (270 - Main.mouseTextColor) / 700f;
        
        r = 0.9f + pulse;
        g = 0.4f + pulse * 0.6f;
        b = 0.1f + pulse * 0.2f;
    }
    
    public override void PlaceInWorld(int i, int j, Item item)
    {
        ModContent.GetInstance<RingSystem>().Bonfires.Add(new Point(i, j));
        Main.NewText($"Hoguera añadida en X:{i}, Y:{j}", Color.Orange);
    }
    
}