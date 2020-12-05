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
        public Camera MainCam = null;

        [RangeAttribute(0.0f, 5.0f)]
        [SerializeField] protected float TimeScale = 1.0f;
        private GameObject FocusedGO;

        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        void OnValidate()
        {
            Time.timeScale = TimeScale;
        }

        void Start()
        {
            MainCam = Camera.main;
        }

        void Update()
        {
            if (FocusedGO != null && EventSystem.current.currentSelectedGameObject != FocusedGO)
                EventSystem.current.SetSelectedGameObject(FocusedGO);
        }

        public void ForceFocusGameObject(GameObject GO)
        {
            FocusedGO = GO;

            if (FocusedGO != null)
                EventSystem.current.SetSelectedGameObject(FocusedGO);
        }
    }
}
