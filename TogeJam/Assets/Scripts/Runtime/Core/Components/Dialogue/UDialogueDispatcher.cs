using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class UDialogueDispatcher : MonoBehaviour
    {
        [SerializeField] protected UDialogueManager DialogueManager;

         [Header("Debug")]
        [SerializeField] protected UPlayerController Player;

        public void Beat1_Start() => DialogueManager.InitiateDialogue("Starting");
        public void Beat1_ReachClientHome()
        {
            DialogueManager.InitiateDialogue("MC.ReachClientHouse", new List<ITalkable>(){Player});
        }
    }
}