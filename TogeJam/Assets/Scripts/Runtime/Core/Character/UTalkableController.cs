using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class UTalkableController : UController, ITalkable, IInteractable
    {
        [SerializeField] protected FSpeakerInfo SpeakerInfo;
        [SerializeField] protected GameObject Interactor;

        public InteractionRange GetInteractionRange() => InteractionRange.CloseRange;

        public FSpeakerInfo GetSpeakerInfo()
        {
            if (SpeakerInfo.SpeechTransform == null)
                SpeakerInfo.SpeechTransform = transform;

            return SpeakerInfo;
        }

        public void IExecuteInteract(GameObject GO)
        {
            List<ITalkable> Speakers = new List<ITalkable>();
            Speakers.Add(GO.GetComponent<ITalkable>());
            Speakers.Add(GetComponent<ITalkable>());

            UDialogueManager.DialogueManager.InitiateDialogue("Billy", Speakers, "Billy.Start");
        }

        public void IOnFocus(GameObject GO)
        {
            
        }

        public void IOnUnfocus(GameObject GO)
        {
            
        }
    }
}