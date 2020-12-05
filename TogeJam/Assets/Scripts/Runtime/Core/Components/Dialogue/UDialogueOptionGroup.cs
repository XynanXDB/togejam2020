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
        
        void OnEnable()
        {
            OptionsGroupAnimator.Play("Start");

            UGameInstance.GameInstance.ForceFocusGameObject(null);
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
