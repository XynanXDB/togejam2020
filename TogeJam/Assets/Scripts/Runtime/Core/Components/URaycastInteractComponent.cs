using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utility;

namespace Game.Core
{
    public class URaycastInteractComponent : MonoBehaviour
    {
        [SerializeField] protected float RaycastRange = 100.0f;
        [SerializeField] protected UPlayerController PlayerController;
        [SerializeField] protected GameObject Interactor;
        [SerializeField] protected Camera MainCam;

        private IInteractable CurrentFocusedInteractable;

        void Awake()
        {
            //TODO Uncomment this
            //PlayerController.OnInteractDelegate += OnClick;
        }

        void OnClick()
        {
            RaycastHit Hit;

             if (InputUtils.RaycastBeneathMouseCursor(MainCam, out Hit))
             {
                CurrentFocusedInteractable = Hit.transform.gameObject.GetComponent<IInteractable>();
                HandleOnFocus(CurrentFocusedInteractable);
             }
            else
            {
                CurrentFocusedInteractable = null;
                HandleOnUnfocus();
            }
        }
        // bool RaycastForInteractable(out RaycastHit Hit)
        // {
        //     Vector3 Origin = Interactor.transform.position;
        //     Origin.y = 1.0f;
        //     Vector3 Direction = Interactor.transform.forward;

        //     Debug.DrawLine(Origin, Origin + Direction * RaycastRange, Color.red, 5.0f);

        //     return Physics.Raycast(Origin, Direction, out Hit, RaycastRange);
        // }

        void HandleOnFocus(IInteractable Interactable)
        {
            if (CurrentFocusedInteractable != Interactable)
            {
                HandleOnUnfocus();
                Interactable.IOnFocus(Interactor);
            }
        }

        void HandleOnUnfocus()
        {
            if (CurrentFocusedInteractable == null) return;

            CurrentFocusedInteractable.IOnUnfocus(Interactor);
            CurrentFocusedInteractable = null;
        }

        public void HandleExecuteInteract()
        {
            if (CurrentFocusedInteractable == null) return;

            CurrentFocusedInteractable.IExecuteInteract();
            CurrentFocusedInteractable = null;
        }
    }
}