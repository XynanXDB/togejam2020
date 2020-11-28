using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RotaryHeart.Lib.SerializableDictionary;

namespace Game.Core
{
    public class UPlayerCharacter : MonoBehaviour
    {
        [System.Serializable]
        protected class EyeAnimMatchup : SerializableDictionaryBase<string, Material>
        {}

        protected enum EMovementType
        {
            Right,
            Left,
            Stop
        }

        [System.Serializable]
        protected struct FEyeAnimController
        {
            public SkinnedMeshRenderer MeshRenderer;
            public EyeAnimMatchup EyeMatchup;

////////////////////////////////////////////////////////////////////////////////////////////////

            public bool TrySetEyeAnim(string Animation)
            {
                Material FoundMaterial = null;
                bool RetVal = EyeMatchup.TryGetValue(Animation, out FoundMaterial);

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

        public void OnInteract()
        {
            InteractComponent.HandleExecuteInteract();
        }

        public void SetAnimation(string Animation)
        {
            if (!EyeAnimController.TrySetEyeAnim(Animation))
                if (Animation == "Walk")
                {
                    PlayerCharAnimate.Play(Animation);
                    MovementType = EMovementType.Right;
                }
        }
    }
}
