using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace CustomRecipes.Bonfire;

public static class BonfireSystem
{
    // Lista de hogueras colocadas en el mundo
    // public static List<Point> Bonfires = [];

    // Abre el menú de selección
    // public static void OpenBonfireMenu()
    // {
    //     // Si no hay hogueras, no hacer nada
    //     if (Bonfires.Count == 0)
    //     {
    //         Main.NewText("¡No hay hogueras disponibles!", Color.Orange);
    //         return;
    //     }
    //
    //     // Menú simple en consola (para versión básica)
    //     Main.NewText("--- Hogueras Disponibles ---", Color.Yellow);
    //     for (var i = 0; i < Bonfires.Count; i++)
    //     {
    //         Main.NewText($"{i + 1}. Hoguera en X:{Bonfires[i].X}, Y:{Bonfires[i].Y}", Color.LightGray);
    //     }
    //     Main.NewText("Escribe /bonfire [número] para teletransportarte.", Color.Green);
    // }

    // Teletransporta al jugador a la hoguera seleccionada
    // public static void TeleportToBonfire(Player player, int index)
    // {
    //     if (index < 0 || index >= Bonfires.Count) return;
    //
    //     var bonfirePos = Bonfires[index];
    //     
    //     player.Teleport(new Vector2(bonfirePos.X * 16, bonfirePos.Y * 16 - 32));
    //     Main.NewText($"Teletransportado a Hoguera {index + 1}!", Color.Green);
    // }
}