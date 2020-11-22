using System.Collections.Generic;
using UnityEngine;
using Game.Library.Delegate;

namespace Game.Core
{
    public abstract class UController : MonoBehaviour
    {
        public VoidSignature OnInteractDelegate;

        void OnDestroy()
        {
            OnInteractDelegate = null;
        }
    }
}
