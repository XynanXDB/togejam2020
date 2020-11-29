using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class UStreetBlock : MonoBehaviour
    {
        public void TriggerLoop(UStreetBlock AfterBlock)
        {
            if (gameObject.activeSelf)
                transform.position = AfterBlock.transform.position + Vector3.right * 48.9f;
        }
    }
}