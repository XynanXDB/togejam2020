﻿using System;
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
        //private int bTurnDog = 0;

///////////////////////////////////////////////////////////////////////////////

        // void Update()
        // {
        //     // if (bTurnDog == 1)
        //     // {
        //     //     Quaternion currentRotation = transform.rotation;
        //     //     transform.rotation = Quaternion.RotateTowards(currentRotation, Quaternion.Euler(0.0f, 90.0f, 0.0f), 85.0f * Time.deltaTime);

        //     //     if (Quaternion.Dot(currentRotation, Quaternion.Euler(0.0f, 90.0f, 0.0f)) >= 1.0f)
        //     //         bTurnDog = 0;
        //     // } else if (bTurnDog == 2)
        //     // {
        //     //     Quaternion currentRotation = transform.rotation;
        //     //     transform.rotation = Quaternion.RotateTowards(currentRotation, Quaternion.Euler(0.0f, -90.0f, 0.0f), 95.0f * Time.deltaTime);

        //     //     if (Quaternion.Dot(currentRotation, Quaternion.Euler(0.0f, -90.0f, 0.0f)) >= 1.0f)
        //     //         bTurnDog = 0;
        //     // }
        // }

        public void StartGoodCredits()
        {
            transform.SetPositionAndRotation(new Vector3(-172.44f, -0.03f, -135.48f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
            DogAnimate.Play("PreCredits");
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
        }

        public void IOnFocus(GameObject GO)
        {
            
        }

        public void IOnUnfocus(GameObject GO)
        {
            
        }

        public void SetAnimation(string Animation) //TODO Implement Animation
        {
            if (Animation == "GoodCredits")
                transform.position = new Vector3(-172.055f, -0.03f, -135.414f);

            DogAnimate.Play(Animation);
        }

        public void SendNativeCommand(string Data)
        {
            if (Data == "TurnDog.Right")
                transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            else if (Data == "TurnDog.Left")
                transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        }
    }
}