using System.Net.Mime;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System.Runtime.CompilerServices;
using Game.Utility;
using Game.Library.Delegate;
using System;

[assembly: InternalsVisibleTo("SystemInit")]

namespace Game.Core
{
    public struct YarnCommandPacket
    {
        public string name;
        public string Action;
        public string Animation;

///////////////////////////////////////////////////////////////////////////////

        public YarnCommandPacket(string InName, string InAction, string InAnimation)
        {
            name = InName;
            Action = InAction;
            Animation = InAnimation;
        }

        public override bool Equals(object obj)
        {
            return obj is YarnCommandPacket packet &&
                   name == packet.name &&
                   Action == packet.Action &&
                   Animation == packet.Animation;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator==(YarnCommandPacket Self, YarnCommandPacket Other)
        {
            return (Self.name == Other.name && Self.Action == Other.Action && Self.Animation == Other.Animation);
        }

        public static bool operator!=(YarnCommandPacket Self, YarnCommandPacket Other)
        {
            return !(Self == Other);
        }
    }

    public class UDialogueManager : MonoBehaviour
    {
        private static UDialogueManager _DialogueManager;
        public static UDialogueManager DialogueManager
        {
            get
            {
                if (_DialogueManager == null)
                    _DialogueManager = FindObjectOfType<UDialogueManager>();
                else
                    _DialogueManager = GenericHelpers.CreateManager("DialogueManager").GetComponent<UDialogueManager>();
                
                return _DialogueManager;
            }
        }
        
        [SerializeField] protected DialogueRunner DialogueRunner;
        [SerializeField] protected UObjectPooler BubblePooler;
        [SerializeField] protected UDialogueDataBase DialogueDB;
        [SerializeField] protected DialogueUI DialogueUI;
        [SerializeField] protected UDialogueOptionGroup OptionGroup;
        [SerializeField] protected UTransientDialogueBox TransientDialogueBox;
        private List<UDialogueBubble> AssignedBubbles = null;
        private List<ITalkable> JoinedSpeakers;
        public OneParamSignature<YarnCommandPacket> OnReceiveSetSpeaker = null;

        [Header("Debug")]
        [SerializeField] protected UPlayerController Player;
        [SerializeField] protected UController Dog;

///////////////////////////////////////////////////////////////////////
        
        void Awake()
        {
            DontDestroyOnLoad(this);
            DialogueRunner.AddCommandHandler("SetSpeaker", SetSpeaker);
            DialogueRunner.AddCommandHandler("SetAnimation", SetAnimation);

            DialogueUI.onDialogueEnd.AddListener(OnDialogueEnd);
            
            AssignedBubbles = new List<UDialogueBubble>();
            JoinedSpeakers = new List<ITalkable>();
            
            BubblePooler.GetAllPooledObjects().ForEach( GO =>
            {
                UDialogueBubble B = GO.GetComponent<UDialogueBubble>();
                OnReceiveSetSpeaker += B.OnSetSpeaker;
            });
        }

        void SetAnimation(string[] Data)
        {
            string[] Segments = Data[0].Split('.');
            
            ITalkable Target = JoinedSpeakers.Find( I => I.GetSpeakerInfo().Name == Segments[0]);
            if (Target != null)
                Target.SetAnimation(Segments[1]);
        }

        void Start()
        {
            StartDialogue("Starting");
            TransientDialogueBox.Init(DialogueUI);
        }

        void StartDialogue(string YarnAssetName, string StartNodeName = "Start")
        {
            YarnProgram YarnAsset = DialogueDB.GetYarnAssetByKey(YarnAssetName);
            if (YarnAsset != null)
            {
                DialogueRunner.Add(YarnAsset);
                DialogueRunner.StartDialogue(StartNodeName);
                Player.SetMovementMode(InputMode.UI);
            }
            else
            {
                Debug.LogError("YarnAsset not found");
                return;
            }
        }

        public void InitiateDialogue(string YarnAssetName, List<ITalkable> InSpeakers, string StartNodeName = "Start")
        {
            JoinedSpeakers = InSpeakers;

            foreach(ITalkable I in InSpeakers)
                JoinConversation(I);

            StartDialogue(YarnAssetName, StartNodeName);
        }

        // Name -> Think/Talk -> Animation
        void SetSpeaker(string[] Data)
        {
            OnReceiveSetSpeaker?.Invoke(new YarnCommandPacket(Data[0], Data[1], Data[2]));
        }

        void OnDestroy()
        {
            OnReceiveSetSpeaker = null;
        }

        public void JoinConversation(ITalkable Speaker)
        {
            FSpeakerInfo SpeakerInfo = Speaker.GetSpeakerInfo();
            UDialogueBubble Bubble = BubblePooler.GetTopObject().GetComponent<UDialogueBubble>();
            
            Bubble.AssignSpeaker(SpeakerInfo, DialogueUI);
            AssignedBubbles.Add(Bubble);
        }

        public void OnOptionsStart()
        {
            OptionGroup.gameObject.SetActive(true);
        }

        void OnDialogueEnd()
        {
            AssignedBubbles.ForEach(B => {
                B.UnassignSpeaker();
            });

            TransientDialogueBox.Disable();

            AssignedBubbles.Clear();
            JoinedSpeakers.Clear();
            Player.SetMovementMode(InputMode.Game);

            DialogueRunner.Clear();
        }
    }
}
