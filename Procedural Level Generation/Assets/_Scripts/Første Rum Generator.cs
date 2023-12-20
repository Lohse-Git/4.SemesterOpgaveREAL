using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RumFørstDungeonGenerator : GangGenerator
{
    [SerializeField]
    private int minRumBredde = 4, minRumHøjde = 4;
    [SerializeField]
    private int dungeonBredde = 20, dungeonHøjde = 20;
    [SerializeField]
    [Range(0, 10)]
    private int forskydning = 1;
    [SerializeField]
    private bool randomWalkRum = false;

    protected override void KørProceduralGenerering()
    {
        OpretRum();
    }

    private void OpretRum()
    {
        var rumListe = ProceduralGenerationAlgorithms.BinærRumOpdeling(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonBredde, dungeonHøjde, 0)), minRumBredde, minRumHøjde);

        HashSet<Vector2Int> gulv = new HashSet<Vector2Int>();

        if (randomWalkRum)
        {
            gulv = OpretRumTilfældigt(rumListe);
        }
        else
        {
            gulv = OpretEnkleRum(rumListe);
        }

        List<Vector2Int> rumCentrum = new List<Vector2Int>();
        foreach (var rum in rumListe)
        {
            rumCentrum.Add((Vector2Int)Vector3Int.RoundToInt(rum.center));
        }

        HashSet<Vector2Int> korridorer = ForbindRum(rumCentrum);
        gulv.UnionWith(korridorer);

        tilemapVisualizer.MalGulvFliser(gulv);
        VægGenerator.OpretVægge(gulv, tilemapVisualizer);
    }

    private HashSet<Vector2Int> OpretRumTilfældigt(List<BoundsInt> rumListe)
    {
        HashSet<Vector2Int> gulv = new HashSet<Vector2Int>();
        for (int i = 0; i < rumListe.Count; i++)
        {
            var rumBounds = rumListe[i];
            var rumCentrum = new Vector2Int(Mathf.RoundToInt(rumBounds.center.x), Mathf.RoundToInt(rumBounds.center.y));
            var rumGulv = KørRandomWalk(RandomWalkParametre, rumCentrum);
            foreach (var position in rumGulv)
            {
                if (position.x >= (rumBounds.xMin + forskydning) && position.x <= (rumBounds.xMax - forskydning) && position.y >= (rumBounds.yMin + forskydning) && position.y <= (rumBounds.yMax - forskydning))
                {
                    gulv.Add(position);
                }
            }
        }
        return gulv;
    }

    private HashSet<Vector2Int> ForbindRum(List<Vector2Int> rumCentrum)
    {
        HashSet<Vector2Int> korridorer = new HashSet<Vector2Int>();
        var nuværendeRumCentrum = rumCentrum[Random.Range(0, rumCentrum.Count)];
        rumCentrum.Remove(nuværendeRumCentrum);

        while (rumCentrum.Count > 0)
        {
            Vector2Int nærmeste = FindNærmestePunktTil(nuværendeRumCentrum, rumCentrum);
            rumCentrum.Remove(nærmeste);
            HashSet<Vector2Int> nyKorridor = OpretKorridor(nuværendeRumCentrum, nærmeste);
            nuværendeRumCentrum = nærmeste;
            korridorer.UnionWith(nyKorridor);
        }
        return korridorer;
    }

    private HashSet<Vector2Int> OpretKorridor(Vector2Int nuværendeRumCentrum, Vector2Int destination)
    {
        HashSet<Vector2Int> korridor = new HashSet<Vector2Int>();
        var position = nuværendeRumCentrum;
        korridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            korridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            korridor.Add(position);
        }
        return korridor;
    }

    private Vector2Int FindNærmestePunktTil(Vector2Int nuværendeRumCentrum, List<Vector2Int> rumCentrum)
    {
        Vector2Int nærmeste = Vector2Int.zero;
        float afstand = float.MaxValue;
        foreach (var position in rumCentrum)
        {
            float nuværendeAfstand = Vector2.Distance(position, nuværendeRumCentrum);
            if (nuværendeAfstand < afstand)
            {
                afstand = nuværendeAfstand;
                nærmeste = position;
            }
        }
        return nærmeste;
    }

    private HashSet<Vector2Int> OpretEnkleRum(List<BoundsInt> rumListe)
    {
        HashSet<Vector2Int> gulv = new HashSet<Vector2Int>();
        foreach (var rum in rumListe)
        {
            for (int kolonne = forskydning; kolonne < rum.size.x - forskydning; kolonne++)
            {
                for (int række = forskydning; række < rum.size.y - forskydning; række++)
                {
                    Vector2Int position = (Vector2Int)rum.min + new Vector2Int(kolonne, række);
                    gulv.Add(position);
                }
            }
        }
        return gulv;
    }
}