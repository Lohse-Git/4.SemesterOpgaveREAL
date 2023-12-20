using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap gulvTilemap, vægTilemap;
    [SerializeField]
    private TileBase gulvFlise, vægTop, vægSideHøjre, vægSideVenstre, vægBund, vægFuld,
        vægIndvendigHjørneNedVenstre, vægIndvendigHjørneNedHøjre,
        vægDiagonaltHjørneNedHøjre, vægDiagonaltHjørneNedVenstre, vægDiagonaltHjørneOpHøjre, vægDiagonaltHjørneOpVenstre;

    public void MalGulvFliser(IEnumerable<Vector2Int> gulvPositioner)
    {
        MalFliser(gulvPositioner, gulvTilemap, gulvFlise);
    }

    private void MalFliser(IEnumerable<Vector2Int> positioner, Tilemap tilemap, TileBase flise)
    {
        foreach (var position in positioner)
        {
            MalEnkeltFlise(tilemap, flise, position);
        }
    }

    internal void MalEnkeltGrundlæggendeVæg(Vector2Int position, string binærType)
    {
        int typeSomInt = Convert.ToInt32(binærType, 2);
        TileBase flise = null;
        if (VægTyperHjælper.vægTop.Contains(typeSomInt))
        {
            flise = vægTop;
        }
        else if (VægTyperHjælper.vægSideHøjre.Contains(typeSomInt))
        {
            flise = vægSideHøjre;
        }
        else if (VægTyperHjælper.vægSideVenstre.Contains(typeSomInt))
        {
            flise = vægSideVenstre;
        }
        else if (VægTyperHjælper.vægBund.Contains(typeSomInt))
        {
            flise = vægBund;
        }
        else if (VægTyperHjælper.vægFuld.Contains(typeSomInt))
        {
            flise = vægFuld;
        }

        if (flise != null)
            MalEnkeltFlise(vægTilemap, flise, position);
    }

    private void MalEnkeltFlise(Tilemap tilemap, TileBase flise, Vector2Int position)
    {
        var flisePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(flisePosition, flise);
    }

    public void Ryd()
    {
        gulvTilemap.ClearAllTiles();
        vægTilemap.ClearAllTiles();
    }

    internal void MalEnkeltHjørneVæg(Vector2Int position, string binærType)
    {
        int typeSomInt = Convert.ToInt32(binærType, 2);
        TileBase flise = null;

        if (VægTyperHjælper.vægIndvendigHjørneNedVenstre.Contains(typeSomInt))
        {
            flise = vægIndvendigHjørneNedVenstre;
        }
        else if (VægTyperHjælper.vægIndvendigHjørneNedHøjre.Contains(typeSomInt))
        {
            flise = vægIndvendigHjørneNedHøjre;
        }
        else if (VægTyperHjælper.vægDiagonaltHjørneNedVenstre.Contains(typeSomInt))
        {
            flise = vægDiagonaltHjørneNedVenstre;
        }
        else if (VægTyperHjælper.vægDiagonaltHjørneNedHøjre.Contains(typeSomInt))
        {
            flise = vægDiagonaltHjørneNedHøjre;
        }
        else if (VægTyperHjælper.vægDiagonaltHjørneOpHøjre.Contains(typeSomInt))
        {
            flise = vægDiagonaltHjørneOpHøjre;
        }
        else if (VægTyperHjælper.vægDiagonaltHjørneOpVenstre.Contains(typeSomInt))
        {
            flise = vægDiagonaltHjørneOpVenstre;
        }
        else if (VægTyperHjælper.vægFuldOttoRetninger.Contains(typeSomInt))
        {
            flise = vægFuld;
        }
        else if (VægTyperHjælper.vægBundOttoRetninger.Contains(typeSomInt))
        {
            flise = vægBund;
        }

        if (flise != null)
            MalEnkeltFlise(vægTilemap, flise, position);
    }
}
