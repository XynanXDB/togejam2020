using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SystemInit")]

namespace Game.Core
{
    public class UDialogueManager : MonoBehaviour
    {
        public static UDialogueManager DialogueManager { get; internal set; }
        [SerializeField] protected DialogueRunner DialogueRunner;

///////////////////////////////////////////////////////////////////////
        
        void Awake()
        {
            DontDestroyOnLoad(this);
            DialogueRunner.AddCommandHandler("SetSpeaker", SetSpeaker);
        }

        // Name -> Animation
        void SetSpeaker(string[] Data)
        {

        }

        public void StartDialogue()
        {
            DialogueRunner.StartDialogue();
        }
    }
}
