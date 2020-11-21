using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SystemInit
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        Application.targetFrameRate = 60;

    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnStartup()
    {
        
    }
}
