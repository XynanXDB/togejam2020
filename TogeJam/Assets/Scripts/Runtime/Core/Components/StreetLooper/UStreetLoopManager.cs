using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Core;

public class UStreetLoopManager : MonoBehaviour
{
    private UStreetBlock PastBlock;
    private UStreetBlock NextBlock;
    [SerializeField] protected UStreetBlock ParkBlock;
    [SerializeField] protected UStreetBlock DefaultAfterBlock;
    private bool bLoopStreet = true;
    public bool bParkPlaced = false;

    void Awake()
    {
        ParkBlock.gameObject.SetActive(false);
    }

    public void SetPastBlock(UStreetBlock StreetBlock)
    {        
        if (PastBlock == null)
        {
            if (!bLoopStreet)
            {
                ParkBlock.TriggerLoop(DefaultAfterBlock);
                bParkPlaced = true;
                return;
            }
            PastBlock = StreetBlock;
            return;
        }
        else
        {
            if (!bLoopStreet)
            {
                ParkBlock.TriggerLoop(StreetBlock);
                bParkPlaced = true;
                return;
            }
            {
                PastBlock.TriggerLoop(StreetBlock);
                PastBlock = StreetBlock;
            }
        }
    }

    public void StopLoop()
    {
        bLoopStreet = false;
        ParkBlock.gameObject.SetActive(true);
    }
}

