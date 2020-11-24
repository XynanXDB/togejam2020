using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core
{
    public enum InputMode
    {
        Game,
        UI
    }
    
    public struct FInputHandler 
    {
        public PlayerInput TargetPlayerInput;

        public void SetMovementMode(InputMode Mode)
        {
            TargetPlayerInput.DeactivateInput();

            switch(Mode)
            {
                case InputMode.Game:
                    TargetPlayerInput.SwitchCurrentActionMap("Default");
                    break;
                case InputMode.UI:
                    TargetPlayerInput.SwitchCurrentActionMap("UI");
                    break;
            }

            Debug.Log("Input:" + TargetPlayerInput.currentActionMap.name);
            TargetPlayerInput.ActivateInput();
        }
    }
}
