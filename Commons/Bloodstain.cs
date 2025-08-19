using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.IO;

namespace TerraSouls.Commons;

public sealed class Bloodstain
{
    public Vector2 Position;
    public int Humanity;
    public long Souls;
    public string WorldGuid;

    public Bloodstain(Vector2 position, int lostHumanity, long lostSouls)
    {
        Position = position;
        Humanity = lostHumanity;
        Souls = lostSouls;
        WorldGuid = Main.ActiveWorldFileData.UniqueId.ToString();
    }

    private Bloodstain()
    {
        Position = Vector2.Zero;
        Humanity = -1;
        Souls = -1;
        WorldGuid = "";
    }

    public TagCompound ToTag()
    {
        return new TagCompound
        {
            ["position"] = Position,
            ["humanity"] = Humanity,
            ["souls"] = Souls,
            ["worldGUID"] = WorldGuid
        };
    }

    public static Bloodstain FromTag(TagCompound tag)
    {
        var rawSouls = tag["souls"];
        var souls = rawSouls switch
        {
            long l => l,
            int i => i,
            _ => 0
        };

        return new Bloodstain
        {
            Position = tag.Get<Vector2>("position"),
            Humanity = tag.GetInt("humanity"),
            Souls = souls,
            WorldGuid = tag.GetString("worldGUID")
        };
    }

    public override string ToString()
    {
        return "Souls:\n" +
               $"    position: {Position}\n" +
               $"    humanity: {Humanity}\n" +
               $"    souls: {Souls}\n" +
               $"    worldGUID: {WorldGuid}\n";
    }
}