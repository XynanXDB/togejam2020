using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class UPlayerCharacter : MonoBehaviour
    {
        protected enum EMovementType
        {
            Right,
            Left,
            Stop
        }

        [System.Serializable]
        protected struct EyeAnimMatchup
        {
            public string Key;
            public Material MaterialAsset;
        }

        [System.Serializable]
        protected struct FEyeAnimController
        {
            public SkinnedMeshRenderer MeshRenderer;
           public List<EyeAnimMatchup> EyeMatchup;

////////////////////////////////////////////////////////////////////////////////////////////////

            public bool TrySetEyeAnim(string Animation)
            {
                Material FoundMaterial = EyeMatchup.Find( Item => Item.Key == Animation).MaterialAsset;
                bool RetVal  = (FoundMaterial != null);

                if (RetVal)
                    MeshRenderer.material = FoundMaterial;

                return RetVal;
            }
        }

        [SerializeField] protected UPlayerController PlayerController;
        [SerializeField] protected URaycastInteractComponent InteractComponent;
        [SerializeField] protected Animator PlayerCharAnimate;
        [SerializeField] protected FEyeAnimController EyeAnimController;
        [SerializeField] protected UTalkableController Dog;
        [SerializeField] float MovementSpeed = 5.0f;
        private EMovementType MovementType = EMovementType.Stop;

    ////////////////////////////////////////////////////////////////////////////////////////////
        void Awake()
        {
            PlayerController.OnInteractDelegate += OnInteract;
        }

        void Update()
        {
            switch(MovementType)
            {
                case EMovementType.Right:
                {
                    Vector3 MovementVector = Vector3.right * MovementSpeed * Time.deltaTime;
                    transform.Translate(MovementVector, Space.World);
                    Dog.transform.Translate(MovementVector, Space.World);
                    break;
                }

                case EMovementType.Left:
                {
                    break;
                }
            }
        }

        public void StartGoodEnding()
        {
            MovementType = EMovementType.Stop;
            transform.SetPositionAndRotation(new Vector3(-174.9f, 0.982f, -135.4f), Quaternion.Euler(0.0f, 180.0f, 0.0f));
            SetAnimation("PreCredits");
        }

        public void OnInteract()
        {
            InteractComponent.HandleExecuteInteract();
        }

        public void SetAnimation(string Animation)
        {
            if (!EyeAnimController.TrySetEyeAnim(Animation))
            {
                PlayerCharAnimate.Play(Animation);

                if (Animation == "Walk")
                    MovementType = EMovementType.Right;
                else if (Animation == "Stand")
                    MovementType = EMovementType.Stop;
            }
        }

        public void ParkStop()
        {
            SetAnimation("Stand");
            Dog.SetAnimation("Stand");
        }
    }
}
