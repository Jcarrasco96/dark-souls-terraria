using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TerraSouls.Tiles;

// ReSharper disable once ClassNeverInstantiated.Global
public class BonfireTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileWaterDeath[Type] = true;
        Main.tileLavaDeath[Type] = true;

        AnimationFrameHeight = 52;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
        TileObjectData.newTile.CoordinateHeights = [16, 16, 20];
        TileObjectData.newTile.DrawYOffset = 2;
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(200, 150, 50), Language.GetText("Mods.TerraSouls.Tiles.DarkSoulsBonfire.DisplayName"));

        DustType = DustID.Smoke;
    }

    public override bool RightClick(int i, int j)
    {
        // if (Main.netMode != NetmodeID.Server)
        // {
        //     // ModContent.GetInstance<RingSystem>().ShowBonfiresUi();
        //     ModContent.GetInstance<LevelUpUiSystem>().ShowLevelUpInterface();
        // }
        
        if (Main.LocalPlayer.whoAmI != Main.myPlayer)
        {
            return false;
        }
        
        SetSpawnToCurrentPosition(Main.LocalPlayer);

        ModContent.GetInstance<RingSystem>().ShowLevelUpUi();
        
        return true;
    }

    private static void SetSpawnToCurrentPosition(Player player)
    {
        // var tileX = (int)(player.position.X / 16);
        // var tileY = (int)(player.position.Y / 16);
        //
        // player.SpawnX = tileX;
        // player.SpawnY = tileY;
        // player.ChangeSpawn(tileX, tileY);
        
        Main.spawnTileX = (int)(player.position.X - 8 + player.width / 2f) / 16;
        Main.spawnTileY = (int)(player.position.Y + player.height) / 16;
    }

    public override void MouseOver(int i, int j)
    {
        var player = Main.LocalPlayer;
        player.cursorItemIconEnabled = true;
        player.cursorItemIconText = "Rest and increase stats";
        player.cursorItemIconID = -1;//ModContent.ItemType<BonfireItem>();
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

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);

        if (!fail)
        {
            ModContent.GetInstance<RingSystem>().Bonfires.RemoveAll(p => p == new Point(i, j));
        }
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

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (!Main.rand.NextBool(5))
        {
            return;
        }

        var spawnPos = new Vector2(i * 16 + 4, j * 16 - 8);
        var dustIndex = Dust.NewDust(spawnPos, 8, 8, DustID.Smoke, 0f, -0.2f, 200, Color.LightGray);

        Main.dust[dustIndex].noGravity = true;
        Main.dust[dustIndex].velocity.Y = -1.2f;
        Main.dust[dustIndex].velocity.X *= 0.3f;
        Main.dust[dustIndex].scale *= 0.8f;
        Main.dust[dustIndex].fadeIn = 1.8f;
    }
}