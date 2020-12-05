using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core
{
    public class UMainMenuButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler, IPointerExitHandler
    {
        [SerializeField] protected Animator ButtonAnimate;

        private EventSystem ES;

        private static readonly string Selected  = "IsSelecting";
        void OnEnable()
        {
            ES = EventSystem.current;

            if (ES.currentSelectedGameObject == gameObject)
                ButtonAnimate.SetBool(Selected, true);
            else
                ButtonAnimate.SetBool(Selected, false);
        }

        public void OnSelect(BaseEventData eventData) => ButtonAnimate.SetBool(Selected, true);

        public void OnDeselect(BaseEventData eventData) => ButtonAnimate.SetBool(Selected, false);

        public void OnPointerEnter(PointerEventData eventData)
        {
            ES.SetSelectedGameObject(eventData.selectedObject);
            ButtonAnimate.SetBool(Selected, true);

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ES.SetSelectedGameObject(null);
            ButtonAnimate.SetBool(Selected, false);
        }
    }
}
