using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class UCinemachineCamLerper : MonoBehaviour
    {
        [SerializeField] protected Vector3 StartLocation;
        [SerializeField] protected Vector3 EndLocation;
        [SerializeField] protected float LerpSpeed = 0.01f;
        private Coroutine LerpCoroutine;

        void OnDisable()
        {
            if (LerpCoroutine != null) 
                StopLerp();
        }

        IEnumerator LerpTo()
        {
            Vector3 CurrentLocation = gameObject.transform.position;
            Vector3 Direction = (EndLocation - StartLocation).normalized;

            float Distance = Vector3.Distance(CurrentLocation, EndLocation);

            while(Distance >= 1.0f)
            {
                Distance = Vector3.Distance(CurrentLocation, EndLocation);
                CurrentLocation += Direction * LerpSpeed;
                gameObject.transform.position = CurrentLocation;
                yield return null;
            }
            LerpCoroutine = null;
        }

        public void StartLerp()
        {
            gameObject.transform.position = StartLocation;
            LerpCoroutine = StartCoroutine(LerpTo());
        }

        public void StopLerp()
        {
            if (LerpCoroutine != null) 
                StopCoroutine(LerpCoroutine);
        }
    }
}