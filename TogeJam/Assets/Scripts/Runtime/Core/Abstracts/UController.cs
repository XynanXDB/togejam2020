using System.Collections.Generic;
using UnityEngine;
using Game.Library.Delegate;

namespace Game.Core
{
    public abstract class UController : MonoBehaviour
    {
        public VoidSignature OnInteractDelegate;
        private static readonly Vector3 VecZero = Vector3.zero;
        void OnDestroy()
        {
            OnInteractDelegate = null;
        }

        public void Teleport(Transform T) => transform.SetPositionAndRotation(T.position, T.rotation);
    }
}
