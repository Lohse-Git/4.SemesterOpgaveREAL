using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> RandomWalkAlgo(Vector2Int startposition, int WalkLængde)
    {
        HashSet<Vector2Int> sti = new HashSet<Vector2Int>();

        sti.Add(startposition);
        var forrigePosition = startposition;

        for (int i = 0; i < WalkLængde; i++)
        {
            var nyPosition = forrigePosition + Direction2D.HentTilfældigKardinalRetning();
            sti.Add(nyPosition);
            forrigePosition = nyPosition;
        }
        return sti;
    }

    public static List<Vector2Int> RandomWalkGange(Vector2Int startposition, int korridorLængde)
    {
        List<Vector2Int> korridor = new List<Vector2Int>();
        var retning = Direction2D.HentTilfældigKardinalRetning();
        var nuværendePosition = startposition;
        korridor.Add(nuværendePosition);

        for (int i = 0; i < korridorLængde; i++)
        {
            nuværendePosition += retning;
            korridor.Add(nuværendePosition);
        }
        return korridor;
    }

    public static List<BoundsInt> BinærRumOpdeling(BoundsInt rumAtOpdele, int minWidth, int minHeight)
    {
        Queue<BoundsInt> rumKø = new Queue<BoundsInt>();
        List<BoundsInt> rumListe = new List<BoundsInt>();
        rumKø.Enqueue(rumAtOpdele);
        while (rumKø.Count > 0)
        {
            var rum = rumKø.Dequeue();
            if (rum.size.y >= minHeight && rum.size.x >= minWidth)
            {
                if (Random.value < 0.5f)
                {
                    if (rum.size.y >= minHeight * 2)
                    {
                        SplitHorisontalt(minHeight, rumKø, rum);
                    }
                    else if (rum.size.x >= minWidth * 2)
                    {
                        SplitVertikalt(minWidth, rumKø, rum);
                    }
                    else if (rum.size.x >= minWidth && rum.size.y >= minHeight)
                    {
                        rumListe.Add(rum);
                    }
                }
                else
                {
                    if (rum.size.x >= minWidth * 2)
                    {
                        SplitVertikalt(minWidth, rumKø, rum);
                    }
                    else if (rum.size.y >= minHeight * 2)
                    {
                        SplitHorisontalt(minHeight, rumKø, rum);
                    }
                    else if (rum.size.x >= minWidth && rum.size.y >= minHeight)
                    {
                        rumListe.Add(rum);
                    }
                }
            }
        }
        return rumListe;
    }

    private static void SplitVertikalt(int minWidth, Queue<BoundsInt> rumKø, BoundsInt rum)
    {
        var xOpdeling = Random.Range(1, rum.size.x);
        BoundsInt rum1 = new BoundsInt(rum.min, new Vector3Int(xOpdeling, rum.size.y, rum.size.z));
        BoundsInt rum2 = new BoundsInt(new Vector3Int(rum.min.x + xOpdeling, rum.min.y, rum.min.z),
            new Vector3Int(rum.size.x - xOpdeling, rum.size.y, rum.size.z));
        rumKø.Enqueue(rum1);
        rumKø.Enqueue(rum2);
    }

    private static void SplitHorisontalt(int minHeight, Queue<BoundsInt> rumKø, BoundsInt rum)
    {
        var yOpdeling = Random.Range(1, rum.size.y);
        BoundsInt rum1 = new BoundsInt(rum.min, new Vector3Int(rum.size.x, yOpdeling, rum.size.z));
        BoundsInt rum2 = new BoundsInt(new Vector3Int(rum.min.x, rum.min.y + yOpdeling, rum.min.z),
            new Vector3Int(rum.size.x, rum.size.y - yOpdeling, rum.size.z));
        rumKø.Enqueue(rum1);
        rumKø.Enqueue(rum2);
    }
}

public static class Direction2D
{
    public static List<Vector2Int> kardinalRetningerListe = new List<Vector2Int>
    {
        new Vector2Int(0,1), //OP
        new Vector2Int(1,0), //HØJRE
        new Vector2Int(0, -1), // NED
        new Vector2Int(-1, 0) //VENSTRE
    };

    public static List<Vector2Int> diagonaleRetningerListe = new List<Vector2Int>
    {
        new Vector2Int(1,1), //OP-HØJRE
        new Vector2Int(1,-1), //HØJRE-NED
        new Vector2Int(-1, -1), // NED-VENSTRE
        new Vector2Int(-1, 1) //VENSTRE-OP
    };

    public static List<Vector2Int> otteRetningerListe = new List<Vector2Int>
    {
        new Vector2Int(0,1), //OP
        new Vector2Int(1,1), //OP-HØJRE
        new Vector2Int(1,0), //HØJRE
        new Vector2Int(1,-1), //HØJRE-NED
        new Vector2Int(0, -1), // NED
        new Vector2Int(-1, -1), // NED-VENSTRE
        new Vector2Int(-1, 0), //VENSTRE
        new Vector2Int(-1, 1) //VENSTRE-OP
    };

    public static Vector2Int HentTilfældigKardinalRetning()
    {
        return kardinalRetningerListe[UnityEngine.Random.Range(0, kardinalRetningerListe.Count)];
    }
}

