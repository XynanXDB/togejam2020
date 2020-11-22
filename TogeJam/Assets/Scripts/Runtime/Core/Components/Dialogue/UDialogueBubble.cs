using Yarn.Unity;
using TMPro;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;

namespace Game.Core
{
    [System.Serializable]
    public class DialogueBubbleDictionary : SerializableDictionaryBase<string, string>
    {}

    public class UDialogueBubble : MonoBehaviour
    {
        private string SpeakerName;
        private Transform SpeakerTransform; 
        private DialogueUI DialogueUI;
        private YarnCommandPacket CacheYarnPacket;
        private bool DontReplaySameAnim;
        [SerializeField] protected RectTransform OwningRect;
        [SerializeField] protected Vector3 Offset;
        [SerializeField] protected TMP_Text DisplayText;
        [SerializeField] protected Animator BGAnimate;
        [SerializeField] protected DialogueBubbleDictionary DialogueBubbleDictionary;

        void Update()
        {
            Vector3 Screenspace = UGameInstance.GameInstance.MainCam.WorldToScreenPoint(SpeakerTransform.position, Camera.MonoOrStereoscopicEye.Mono);
            OwningRect.SetPositionAndRotation(Screenspace, Quaternion.identity);
        }

        public void AssignSpeaker(FSpeakerInfo Info, DialogueUI UI)
        {
            SpeakerName = Info.Name;
            SpeakerTransform = Info.SpeechTransform;
            DialogueUI = UI;

            DialogueUI.onLineUpdate.AddListener(SetDisplayText);
            DialogueUI.onLineStart.AddListener(PlayBubbleAnim);

            Debug.Log(gameObject.name + " assigned to " + SpeakerName);
            gameObject.SetActive(true);
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
            if (CacheYarnPacket.Action != null && CacheYarnPacket.Action == Packet.Action)
                DontReplaySameAnim = true;
            else
                DontReplaySameAnim = false;

            CacheYarnPacket = Packet;
        }
    }
}