using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class UMovementStopperTrigger : MonoBehaviour
    {
        void OnTriggerEnter(Collider Other)
        {
            if (Other.gameObject.CompareTag("Player"))
            {
                Other.gameObject.GetComponent<UPlayerCharacter>().ParkStop();
                UDialogueManager.DialogueManager.IncrementStartBeat4Park();
            }
        }
    }
}