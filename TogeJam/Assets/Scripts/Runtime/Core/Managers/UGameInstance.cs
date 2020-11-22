using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SystemInit")]

namespace Game.Core
{
    public class UGameInstance : MonoBehaviour
    {
        public static UGameInstance GameInstance { get; internal set; }

        public Camera MainCam = null;

        void Start()
        {
            MainCam = Camera.main;
        }
    }
}
