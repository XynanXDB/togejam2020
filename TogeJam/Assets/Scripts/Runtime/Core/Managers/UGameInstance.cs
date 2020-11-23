using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;


[assembly: InternalsVisibleTo("SystemInit")]

namespace Game.Core
{
    public class UGameInstance : MonoBehaviour
    {
        public static UGameInstance GameInstance { get; internal set; }
        private GameObject FocusedGO;

        public Camera MainCam = null;

        void Start()
        {
            MainCam = Camera.main;
        }

        void Update()
        {
            if (FocusedGO != null && EventSystem.current.currentSelectedGameObject != FocusedGO)
            {
                EventSystem.current.SetSelectedGameObject(FocusedGO);
                Debug.Log("Refocus on " + FocusedGO);
            }
        }

        public void ForceFocusGameObject(GameObject GO)
        {
            FocusedGO = GO;

            if (FocusedGO != null)
                EventSystem.current.SetSelectedGameObject(FocusedGO);
        }
    }
}
