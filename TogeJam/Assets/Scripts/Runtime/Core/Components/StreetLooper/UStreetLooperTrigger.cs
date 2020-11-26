using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class UStreetLooperTrigger : MonoBehaviour
    {
        [SerializeField] protected BoxCollider TriggerBox;
        [SerializeField] protected UStreetBlock OwnerBlock;
        [SerializeField] protected UStreetLoopManager LoopManager;

        void OnTriggerEnter(Collider Other)
        {
            if (Other.gameObject.CompareTag("Player"))
                LoopManager.SetPastBlock(OwnerBlock);
        }
    }
}