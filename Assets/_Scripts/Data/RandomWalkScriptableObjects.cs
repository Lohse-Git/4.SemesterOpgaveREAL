using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SimpleRandomWalkParameters_",menuName = "PCG/SimpleRAndomWalkData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    public int iterationer = 10, WalkLængde = 10;
    public bool startTilfældigtHverIteration = true;
}
