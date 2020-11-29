using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class UDialogueDispatcher : MonoBehaviour
    {
        private static UDialogueDispatcher _Dispatcher;
        public static UDialogueDispatcher Dispatcher
        {
            get
            {
                if (_Dispatcher == null)
                    _Dispatcher = FindObjectOfType<UDialogueDispatcher>();

                return _Dispatcher;
            }
        }
        [SerializeField] protected UDialogueManager DialogueManager;

         [Header("Debug")]
        [SerializeField] protected UPlayerController Player;
        [SerializeField] protected UTalkableController Dog;

///////////////////////////////////////////////////////////////////////////////

        public void Beat1_Start()
        {
            DialogueManager.OnCustomDialogueEnd = () => UPlayableDirector.PlayableDirector.PlayCinematic("Beat1_ReachClientHouse");
            DialogueManager.InitiateDialogue("Starting");
        }
        public void Beat1_ReachClientHome()
        {
            DialogueManager.OnCustomDialogueEnd = 
                () => UPlayableDirector.PlayableDirector.PlayCinematic("Beat1_GetDog", Beat1_GotDog);

            DialogueManager.InitiateDialogue("MC.ReachClientHouse", new List<ITalkable>(){Player});
        }
        public void Beat1_GotDog()
        {
           DialogueManager.InitiateDialogue("MC.GotDog", new List<ITalkable>(){Player, Dog});
        }
    }
}