using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Game.Core
{
    public class UDialogueOptionGroup : MonoBehaviour
    {
        [SerializeField] protected RectTransform SelfRect;
        [SerializeField] protected RectTransform TargetRect;
        [SerializeField] protected Animator OptionsGroupAnimator;
        [SerializeField] protected GameObject FirstSelectObject;

        void OnEnable()
        {
            OptionsGroupAnimator.Play("Start");

            UGameInstance.GameInstance.ForceFocusGameObject(null);
            EventSystem.current.SetSelectedGameObject(FirstSelectObject);
        }

        void Update()
        {
            if (TargetRect != null)
                SelfRect.position = TargetRect.position;
        }

        public void TrackTransform(RectTransform Rect)
        {
            TargetRect = Rect;
        }
    }
}
