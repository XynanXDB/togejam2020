using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;
using System;

namespace Game.Core
{
    public class UTransientDialogueBox : MonoBehaviour
    {
        [SerializeField] protected TMP_Text DisplayText;
        [SerializeField] protected float FadeInSpeed = 0.05f;
        [SerializeField] protected UDialogueAdvancer Advancer;
        private Coroutine AnimateFadeCoroutine;
        private string SpeakerName = "Transient";
        private YarnCommandPacket CacheYarnPacket;
        private DialogueUI DialogueUI;
        [SerializeField] protected Animator BoxAnimate;

/////////////////////////////////////////////////////////////////////////////////

        void OnEnable()
        {
           BoxAnimate.Play("FadeIn");
           BoxAnimate.Play("Loop", 1);
        }

        void CleanUp()
        {
            gameObject.SetActive(false);
        }

        void SetEnable() => gameObject.SetActive(true);

        void ConditionalActivateDialogueAdvancer()
        {
            Advancer.gameObject.SetActive(true);
        }

        public void Disable()
        {
            if (gameObject.activeSelf)
            {
                BoxAnimate.Play("FadeOut");
                SpeakerName = null;
                DisplayText.text = "";
                DialogueUI.onLineUpdate.RemoveListener(SetDisplayText);
                DialogueUI.onLineStart.RemoveListener(SetEnable);
                DialogueUI.onLineFinishDisplaying.RemoveListener(ConditionalActivateDialogueAdvancer);
                
                DialogueUI = null;
            }
        }

        public void Init(DialogueUI UI)
        {
            DialogueUI = UI;
            DialogueUI.onLineUpdate.AddListener(SetDisplayText);
            DialogueUI.onLineStart.AddListener(SetEnable);

            DialogueUI.onLineFinishDisplaying.AddListener(ConditionalActivateDialogueAdvancer);
            
            Advancer.AssignSpeaker(DialogueUI, SpeakerName);
            gameObject.SetActive(true);
        }

        void SetDisplayText(string Text)
        {
            DisplayText.text = Text;
        }
    }
}