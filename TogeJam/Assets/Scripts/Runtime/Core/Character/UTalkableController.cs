using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class UTalkableController : UController, ITalkable, IInteractable
    {
        [SerializeField] protected FSpeakerInfo SpeakerInfo;
        [HideInInspector] public GameObject Interactor;

        public InteractionRange GetInteractionRange() => InteractionRange.CloseRange;

        public FSpeakerInfo GetSpeakerInfo()
        {
            if (SpeakerInfo.SpeechTransform == null)
                SpeakerInfo.SpeechTransform = transform;

            return SpeakerInfo;
        }

        public void IExecuteInteract(GameObject GO)
        {
            UDialogueManager.DialogueManager.InitiateDialogue("Billy", new List<ITalkable>()
            {
                GO.GetComponent<ITalkable>(),
                GetComponent<ITalkable>()
            }, "Billy.Start");
        }

        public void IOnFocus(GameObject GO)
        {
            
        }

        public void IOnUnfocus(GameObject GO)
        {
            
        }

        public void SetAnimation(string Animation) //TODO Implement Animation
        {
            Debug.Log(Animation);
        }
    }
}