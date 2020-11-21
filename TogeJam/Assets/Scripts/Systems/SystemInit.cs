using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Systems
{
    public static class SystemInit
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Init()
        {
            Application.targetFrameRate = 60;

            GameObject obj = GameObject.FindWithTag("AudioManager");
            if (obj == null)
                UMasterAudioManager.MasterAudioManager = CreateManager("AudioManager").GetComponent<UMasterAudioManager>();

            obj = GameObject.FindWithTag("DialogueManager");
            if (obj == null)
                UDialogueManager.DialogueManager = CreateManager("DialogueManager").GetComponent<UDialogueManager>();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnStartup()
        {
        }

        public static GameObject CreateManager(string ObjectName)
        {
            GameObject Prefab = Resources.Load("Prefabs/" + ObjectName) as GameObject;
            GameObject Instance  = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity);
            Instance.name = ObjectName;
            
            return (Prefab == null) ? null : Instance;
        }
    }

}
