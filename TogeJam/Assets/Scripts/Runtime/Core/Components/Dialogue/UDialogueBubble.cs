using Yarn.Unity;
using TMPro;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using UnityEngine.UI;

namespace Game.Core
{
    [System.Serializable]
    public class DialogueBubbleDictionary : SerializableDictionaryBase<string, string>
    {}

    public class UDialogueBubble : MonoBehaviour
    {
        private DialogueUI DialogueUI;
        private string SpeakerName;
        private Transform SpeakerTransform; 
        private YarnCommandPacket CacheYarnPacket;
        private bool DontReplaySameAnim;
        private Coroutine AnimateScaleCoroutine;
        [SerializeField] protected RectTransform OwningRect;
        [SerializeField] protected Vector3 Offset;
        [SerializeField] protected TMP_Text DisplayText;
        [SerializeField] protected Animator BGAnimate;
        [SerializeField] protected float PopupSpeed = 0.05f;
        [SerializeField] protected UDialogueAdvancer Advancer;
        [SerializeField] protected DialogueBubbleDictionary DialogueBubbleDictionary;

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
            
            Advancer.SetDialogueUI(UI);

            Debug.Log(gameObject.name + " assigned to " + SpeakerName);
            gameObject.SetActive(true);
        }

        public void UnassignSpeaker()
        {
            DialogueUI.onLineUpdate.RemoveListener(SetDisplayText);
            DialogueUI.onLineStart.RemoveListener(PlayBubbleAnim);

            SpeakerName = null;
            SpeakerTransform = null;
            DialogueUI = null;

            DisplayText.text = "";
            AnimateScaleCoroutine = StartCoroutine(AnimateScale(false));
        }

        void PlayBubbleAnim()
        {
            if (DontReplaySameAnim) return;

            string Clip = null;

            if (DialogueBubbleDictionary.TryGetValue(CacheYarnPacket.Action, out Clip))
                BGAnimate.Play(Clip);
            else
                Debug.Log("Missing Clip");
        }

        void SetDisplayText(string Text) => DisplayText.text = Text;

        public void OnSetSpeaker(YarnCommandPacket Packet) 
        {
            if (CacheYarnPacket.name != null && CacheYarnPacket.name == Packet.name) return;

            if (CacheYarnPacket.Action != null && CacheYarnPacket.Action == Packet.Action)
                DontReplaySameAnim = true;
            else
                DontReplaySameAnim = false;

            CacheYarnPacket = Packet;
        }
    }
}