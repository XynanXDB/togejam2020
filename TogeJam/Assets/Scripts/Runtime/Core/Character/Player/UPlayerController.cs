using UnityEngine;
using UnityEngine.InputSystem;
using Game.Core.Movement;
using Game.Utility;

namespace Game.Core
{
    public class UPlayerController : UController, ITalkable
    {
        [SerializeField] protected PlayerInput PlayerInput;
        [SerializeField] protected NavAgentHandler NavAgentHandler;
        [SerializeField] protected Camera MainCam;
        [SerializeField] protected float AcceptanceRadiusPercentage = 0.9f;
        [SerializeField] protected FSpeakerInfo SpeakerInfo;
        [SerializeField] protected UPlayerCharacter PlayerCharacter;
        protected FInputHandler InputHandler;

 //////////////////////////////////////////////////////////////////////

        void Reset()
        {
            if (TryGetComponent<PlayerInput>(out PlayerInput))
                PlayerInput = gameObject.AddComponent<PlayerInput>();
        }

        void Awake()
        {
            InputHandler.TargetPlayerInput = PlayerInput;
        }

        public virtual void OnInteract(InputAction.CallbackContext Context)
        {            
            RaycastHit Hit;
            if (InputUtils.RaycastBeneathMouseCursor(MainCam, out Hit))
            {
                Vector3 CurrentLocation = gameObject.transform.position;
                Vector3 Destination = Hit.point;

                IInteractable I = Hit.transform.gameObject.GetComponent<IInteractable>();
                if (I  != null)
                {
                    InteractionRange RangeType = I.GetInteractionRange();
                    Destination = Hit.collider.ClosestPoint(CurrentLocation) * AcceptanceRadiusPercentage;
                    
                    if (!MathHelpers.Vector3Equals(CurrentLocation, Destination, 10.0f) && RangeType == InteractionRange.CloseRange)
                        NavAgentHandler.SetDestination(Destination);
                    else if (OnInteractDelegate != null && Context.performed)
                        OnInteractDelegate.Invoke();
                }
                else
                {
                    if (!MathHelpers.Vector3Equals(CurrentLocation, Destination, 10.0f))
                    NavAgentHandler.SetDestination(Destination);
                }
            }
        }

        public FSpeakerInfo GetSpeakerInfo()
        {
            if (SpeakerInfo.SpeechTransform == null)
                SpeakerInfo.SpeechTransform = transform;

            return SpeakerInfo;
        }

        public void SetMovementMode(InputMode Mode) => InputHandler.SetMovementMode(Mode);

        public void SetAnimation(string Animation) //TODO Implement Animation
        {
            PlayerCharacter.SetAnimation(Animation);
        }

        public void SendNativeCommand(string Data)
        {
            
        }
    }
}