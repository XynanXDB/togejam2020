using UnityEngine;
using Game.Utility;

namespace Game.Core.Movement
{
    [RequireComponent(typeof(UPlayerCharacter), typeof(UPlayerController))]
    public class UMovementComponent : MonoBehaviour
    {
        [SerializeField] protected NavAgentHandler NavAgentHandler;
        [SerializeField] protected UPlayerController PlayerController;
        [SerializeField] protected Rigidbody TargetRigidbody;
        [SerializeField] protected Camera MainCam;
        [SerializeField] protected float AcceptanceRadiusPercentage = 0.9f;
        
        private Vector3 _KinematicVelocity = Vector3.zero;
        public Vector3 KinematicVelocity { get { return _KinematicVelocity; }}

    }
}

