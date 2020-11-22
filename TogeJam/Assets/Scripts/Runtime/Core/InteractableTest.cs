using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class InteractableTest : MonoBehaviour, IInteractable
    {
        public InteractionRange GetInteractionRange() { return InteractionRange.IgnoreRange; }

        public void IExecuteInteract()
        {
            Debug.Log("A");
        }

        public void IOnFocus(GameObject GO)
        {
            
        }

        public void IOnUnfocus(GameObject GO)
        {
            
        }
    }
}
