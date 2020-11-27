using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Core;

public class UStreetLoopManager : MonoBehaviour
{
    private UStreetBlock PastBlock;
    [SerializeField] protected NavMeshSurface NavSurface;
    [SerializeField] protected UStreetBlock ParkBlock;

    
    public void SetPastBlock(UStreetBlock StreetBlock)
    {
        if (PastBlock == null)
        {
            PastBlock = StreetBlock;
            return;
        }
        else
        {
            PastBlock.TriggerLoop(StreetBlock);
            PastBlock = StreetBlock;
            NavSurface.BuildNavMesh();
        }
    }
}

