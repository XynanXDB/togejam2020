using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core
{
    public class UDialogueChoiceButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler, IPointerExitHandler
    {
        private EventSystem ES;
        [SerializeField] protected Animator ButtonAnimate;

        public static readonly string Normal = "Normal";
        public static readonly string Loop = "Selected";

//////////////////////////////////////////////////////////////////////////////////////
        
        void OnEnable()
        {
            ES = EventSystem.current;

            if (ES.currentSelectedGameObject == gameObject)
                ButtonAnimate.Play(Loop);
            else
                ButtonAnimate.Play(Normal);
        }
        public void OnSelect(BaseEventData eventData) => ButtonAnimate.Play(Loop);

        public void OnDeselect(BaseEventData eventData) => ButtonAnimate.Play(Normal);

        public void OnPointerEnter(PointerEventData eventData)
        {
            ES.SetSelectedGameObject(eventData.selectedObject);
            ButtonAnimate.Play(Loop);
        }

        public void OnPointerExit(PointerEventData eventData) 
        {
            ES.SetSelectedGameObject(null);
            ButtonAnimate.Play(Normal);
        }

    }
}