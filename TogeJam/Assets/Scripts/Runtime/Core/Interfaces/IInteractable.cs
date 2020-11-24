using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public enum InteractionRange
    {
        CloseRange = 0,
        IgnoreRange = 1,
    }

    public interface IInteractable
    {
        void IOnFocus(GameObject GO);
        void IOnUnfocus(GameObject GO);
        void IExecuteInteract(GameObject GO);
        InteractionRange GetInteractionRange();
    }
}
