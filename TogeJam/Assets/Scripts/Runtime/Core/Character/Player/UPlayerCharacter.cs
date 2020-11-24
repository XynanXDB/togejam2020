using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class UPlayerCharacter : MonoBehaviour
    {
        [SerializeField] protected UPlayerController PlayerController;
        [SerializeField] protected URaycastInteractComponent InteractComponent;

    ////////////////////////////////////////////////////////////////////////////////////////////
        void Awake()
        {
            PlayerController.OnInteractDelegate += OnInteract;
        }

        public void OnInteract()
        {
            InteractComponent.HandleExecuteInteract();
        }
    }
}
