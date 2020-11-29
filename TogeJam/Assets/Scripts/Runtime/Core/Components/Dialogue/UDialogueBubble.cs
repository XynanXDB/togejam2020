using Yarn.Unity;
using TMPro;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

namespace Game.Core
{
    public class UDialogueBubble : MonoBehaviour
    {
        private DialogueUI DialogueUI;
        private string SpeakerName;
        private Transform SpeakerTransform; 
        private YarnCommandPacket CacheYarnPacket;
        private Coroutine AnimateScaleCoroutine;
        [SerializeField] protected RectTransform OwningRect;
        [SerializeField] protected Vector3 Offset;
        [SerializeField] protected TMP_Text DisplayText;
        [SerializeField] protected Animator BGAnimate;
        [SerializeField] protected float PopupSpeed = 0.05f;
        [SerializeField] protected UDialogueAdvancer Advancer;
        public string GetSpeakerName { get { return SpeakerName; }}

        void Update()
        {
            if (SpeakerTransform != null)
            {
                Vector3 Screenspace = UGameInstance.GameInstance.MainCam.WorldToScreenPoint(SpeakerTransform.position, Camera.MonoOrStereoscopicEye.Mono);
                OwningRect.SetPositionAndRotation(Screenspace, Quaternion.identity);
            }
        }

        void OnEnable()
        {
            AnimateScaleCoroutine = StartCoroutine(AnimateScale(true));
        }

        void Awake()
        {
            gameObject.transform.localScale = Vector3.zero;
        }

        void OnDisable()
        {
            if (AnimateScaleCoroutine != null)
                StopCoroutine(AnimateScaleCoroutine);

            gameObject.transform.localScale = Vector3.zero;
        }

        IEnumerator AnimateScale(bool Activate)
        {
            Vector3 tempScale = gameObject.transform.localScale;

            if (Activate)
            {
                while(tempScale != Vector3.one)
                {
                    tempScale += Vector3.one * PopupSpeed;
                    gameObject.transform.localScale = tempScale;
                    yield return null;
                }
                AnimateScaleCoroutine = null;
            }
            else
            {
                while(tempScale != Vector3.zero)
                {
                    tempScale -= Vector3.one * PopupSpeed;
                    gameObject.transform.localScale = tempScale;
                    yield return null;
                }

                gameObject.SetActive(false);
            }
        }

        public void SetDialogueUI(DialogueUI UI)
        {
            DialogueUI = UI;
        }

        public void AssignSpeaker(FSpeakerInfo Info, DialogueUI UI)
        {
            SpeakerName = Info.Name;
            SpeakerTransform = Info.SpeechTransform;
            DialogueUI = UI;

            DialogueUI.onLineUpdate.AddListener(SetDisplayText);
            DialogueUI.onLineStart.AddListener(PlayBubbleAnim);
            DialogueUI.onLineFinishDisplaying.AddListener(ConditionalActivateDialogueAdvancer);
            
            Advancer.AssignSpeaker(UI, SpeakerName);

            Debug.Log(gameObject.name + " assigned to " + SpeakerName);
        }

        void ConditionalActivateDialogueAdvancer()
        {
            if (SpeakerName != CacheYarnPacket.name) return;
                Advancer.gameObject.SetActive(true);
        }

        public void UnassignSpeaker()
        {
            DialogueUI.onLineUpdate.RemoveListener(SetDisplayText);
            DialogueUI.onLineStart.RemoveListener(PlayBubbleAnim);
            DialogueUI.onLineFinishDisplaying.RemoveListener(ConditionalActivateDialogueAdvancer);

            SpeakerName = null;
            SpeakerTransform = null;
            DialogueUI = null;

            DisableBubble();
        }

        public void DisableBubble()
        {
            if (gameObject.activeSelf)
            {
                AnimateScaleCoroutine = StartCoroutine(AnimateScale(false));
                DisplayText.text = "";
            }
        }

        void PlayBubbleAnim()
        {
            if (SpeakerName != CacheYarnPacket.name) return;
            
            gameObject.SetActive(true);

            string Clip = null;

            // if (DialogueBubbleDictionary.TryGetValue(CacheYarnPacket.Action, out Clip))
            //     BGAnimate.Play(Clip);
            // else
            //     Debug.Log(CacheYarnPacket.Action + " Missing Clip");

        }

        void SetDisplayText(string Text)
        {
            if (CacheYarnPacket.name == SpeakerName)
                DisplayText.text = Text;
        }

        public void OnSetSpeaker(YarnCommandPacket Packet) 
        {
            if (CacheYarnPacket.name != null)
            {
                if (CacheYarnPacket.name != Packet.name)
                    DisableBubble();
                else
                {
                    if (!gameObject.activeSelf)
                        PlayBubbleAnim();
                }
            }
            CacheYarnPacket = Packet;
        }
    }
}