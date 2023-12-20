using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VægGenerator 
{
    public static void OpretVægge(HashSet<Vector2Int> gulvPositioner, TilemapVisualizer tilemapVisualizer)
    {
        var grundlæggendeVægPositioner = FindVægRetninger(gulvPositioner, Direction2D.kardinalRetningerListe);
        var hjørneVægPositioner = FindVægRetninger(gulvPositioner, Direction2D.diagonaleRetningerListe);
        OpretGrundlæggendeVæg(tilemapVisualizer, grundlæggendeVægPositioner, gulvPositioner);
        OpretHjørneVægge(tilemapVisualizer, hjørneVægPositioner, gulvPositioner);
    }

    private static void OpretHjørneVægge(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> hjørneVægPositioner, HashSet<Vector2Int> gulvPositioner)
    {
        foreach (var position in hjørneVægPositioner)
        {
            string naboerBinærType = "";
            foreach (var retning in Direction2D.otteRetningerListe)
            {
                var naboPosition = position + retning;
                if (gulvPositioner.Contains(naboPosition))
                {
                    naboerBinærType += "1";
                }
                else
                {
                    naboerBinærType += "0";
                }
            }
            tilemapVisualizer.MalEnkeltHjørneVæg(position, naboerBinærType);
        }
    }

    private static void OpretGrundlæggendeVæg(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> grundlæggendeVægPositioner, HashSet<Vector2Int> gulvPositioner)
    {
        foreach (var position in grundlæggendeVægPositioner)
        {
            string naboerBinærType = "";
            foreach (var retning in Direction2D.kardinalRetningerListe)
            {
                var naboPosition = position + retning;
                if (gulvPositioner.Contains(naboPosition))
                {
                    naboerBinærType += "1";
                }
                else
                {
                    naboerBinærType += "0";
                }
            }
            tilemapVisualizer.MalEnkeltGrundlæggendeVæg(position, naboerBinærType);
        }
    }

    private static HashSet<Vector2Int> FindVægRetninger(HashSet<Vector2Int> gulvPositioner, List<Vector2Int> retningListe)
    {
        HashSet<Vector2Int> vægPositioner = new HashSet<Vector2Int>();
        foreach (var position in gulvPositioner)
        {
            foreach (var retning in retningListe)
            {
                var naboPosition = position + retning;
                if (!gulvPositioner.Contains(naboPosition))
                    vægPositioner.Add(naboPosition);
            }
        }
        return vægPositioner;
    }
}
