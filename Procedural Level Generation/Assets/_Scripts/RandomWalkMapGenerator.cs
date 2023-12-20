using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalkMapGenerator : AbstractMapGenerator
{
    [SerializeField]
    protected SimpleRandomWalkSO RandomWalkParametre;

    protected override void KørProceduralGenerering()
    {
        HashSet<Vector2Int> gulvPositioner = KørRandomWalk(RandomWalkParametre, startPosition);
        tilemapVisualizer.Ryd();
        tilemapVisualizer.MalGulvFliser(gulvPositioner);
        VægGenerator.OpretVægge(gulvPositioner, tilemapVisualizer);
    }

    protected HashSet<Vector2Int> KørRandomWalk(SimpleRandomWalkSO parametre, Vector2Int position)
    {
        var nuværendePosition = position;
        HashSet<Vector2Int> gulvPositioner = new HashSet<Vector2Int>();
        for (int i = 0; i < parametre.iterationer; i++)
        {
            var sti = ProceduralGenerationAlgorithms.RandomWalkAlgo(nuværendePosition, parametre.WalkLængde);
            gulvPositioner.UnionWith(sti);
            if (parametre.startTilfældigtHverIteration)
                nuværendePosition = gulvPositioner.ElementAt(Random.Range(0, gulvPositioner.Count));
        }
        return gulvPositioner;
    }
}
