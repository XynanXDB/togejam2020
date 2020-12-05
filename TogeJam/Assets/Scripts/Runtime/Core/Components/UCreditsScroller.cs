using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class UCreditsScroller : MonoBehaviour
    {
        [SerializeField] protected RectTransform Rect;
        [SerializeField] protected Vector3 EndVector;
        [SerializeField] protected float ScrollSpeed;

        void OnEnable()
        {
            Rect.localPosition = new Vector3(0.0f, -Screen.height, 0.0f);
        }

        void Update()
        {
            Rect.localPosition = Vector3.Lerp(Rect.localPosition, EndVector, ScrollSpeed * Time.deltaTime);
        }
    }
}