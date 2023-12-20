using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GangGenerator : RandomWalkMapGenerator
{
    [SerializeField]
    private int korridorLængde = 14, korridorAntal = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    private float rumProcentdel = 0.8f;

    protected override void KørProceduralGenerering()
    {
        KorridorFørstGenerering();
    }

    private void KorridorFørstGenerering()
    {
        HashSet<Vector2Int> gulvPositioner = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentielleRumPositioner = new HashSet<Vector2Int>();

        OpretKorridorer(gulvPositioner, potentielleRumPositioner);

        HashSet<Vector2Int> rumPositioner = OpretRum(potentielleRumPositioner);

        List<Vector2Int> dødeEnder = FindAlleDødeEnder(gulvPositioner);

        OpretRumVedDødeEnder(dødeEnder, rumPositioner);

        gulvPositioner.UnionWith(rumPositioner);

        tilemapVisualizer.MalGulvFliser(gulvPositioner);
        VægGenerator.OpretVægge(gulvPositioner, tilemapVisualizer);

    }

    private void OpretRumVedDødeEnder(List<Vector2Int> dødeEnder, HashSet<Vector2Int> rumGulve)
    {
        foreach (var position in dødeEnder)
        {
            if (rumGulve.Contains(position) == false)
            {
                var rum = KørRandomWalk(RandomWalkParametre, position);
                rumGulve.UnionWith(rum);
            }
        }
    }

    private List<Vector2Int> FindAlleDødeEnder(HashSet<Vector2Int> gulvPositioner)
    {
        List<Vector2Int> dødeEnder = new List<Vector2Int>();
        foreach (var position in gulvPositioner)
        {
            int naboerAntal = 0;
            foreach (var retning in Direction2D.kardinalRetningerListe)
            {
                if (gulvPositioner.Contains(position + retning))
                    naboerAntal++;

            }
            if (naboerAntal == 1)
                dødeEnder.Add(position);
        }
        return dødeEnder;
    }

    private HashSet<Vector2Int> OpretRum(HashSet<Vector2Int> potentielleRumPositioner)
    {
        HashSet<Vector2Int> rumPositioner = new HashSet<Vector2Int>();
        int rumAtOpretteAntal = Mathf.RoundToInt(potentielleRumPositioner.Count * rumProcentdel);

        List<Vector2Int> rumAtOprette = potentielleRumPositioner.OrderBy(x => Guid.NewGuid()).Take(rumAtOpretteAntal).ToList();

        foreach (var rumPosition in rumAtOprette)
        {
            var rumGulv = KørRandomWalk(RandomWalkParametre, rumPosition);
            rumPositioner.UnionWith(rumGulv);
        }
        return rumPositioner;
    }

    private void OpretKorridorer(HashSet<Vector2Int> gulvPositioner, HashSet<Vector2Int> potentielleRumPositioner)
    {
        var nuværendePosition = startPosition;
        potentielleRumPositioner.Add(nuværendePosition);

        for (int i = 0; i < korridorAntal; i++)
        {
            var korridor = ProceduralGenerationAlgorithms.RandomWalkGange(nuværendePosition, korridorLængde);
            nuværendePosition = korridor[korridor.Count - 1];
            potentielleRumPositioner.Add(nuværendePosition);
            gulvPositioner.UnionWith(korridor);
        }
    }
}