using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class UTalkableController : UController, ITalkable, IInteractable
    {
        [SerializeField] protected FSpeakerInfo SpeakerInfo;
        [HideInInspector] public GameObject Interactor;
        [SerializeField] protected Animator DogAnimate;
        private bool bTurnDog = false;

///////////////////////////////////////////////////////////////////////////////

        void Update()
        {
            if (bTurnDog)
            {
                Quaternion currentRotation = transform.rotation;
                transform.rotation = Quaternion.RotateTowards(currentRotation, Quaternion.Euler(0.0f, 90.0f, 0.0f), 85.0f * Time.deltaTime);

                if (Quaternion.Dot(currentRotation, Quaternion.Euler(0.0f, 90.0f, 0.0f)) >= 1.0f)
                    bTurnDog = false;
            }
        }

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
            if (Animation == "Walk")
                DogAnimate.Play(Animation);      
        }

        public void TurnDog()
        {
            bTurnDog = true;
        }

        public void SendNativeCommand(string Data)
        {
            if (Data == "TurnDog")
                bTurnDog = true;
        }
    }
}