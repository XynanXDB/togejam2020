using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
