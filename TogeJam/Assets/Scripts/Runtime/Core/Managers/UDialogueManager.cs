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
        public OneParamSignature<YarnCommandPacket> OnReceiveSetSpeaker = null;

        [Header("Debug")]
        [SerializeField] protected UPlayerController Player;
        [SerializeField] protected UController Dog;

///////////////////////////////////////////////////////////////////////
        
        void Awake()
        {
            DontDestroyOnLoad(this);
            DialogueRunner.AddCommandHandler("SetSpeaker", SetSpeaker);

            DialogueUI.onDialogueEnd.AddListener(OnDialogueEnd);
            
            AssignedBubbles = new List<UDialogueBubble>();
            
            BubblePooler.GetAllPooledObjects().ForEach( GO =>
            {
                UDialogueBubble B = GO.GetComponent<UDialogueBubble>();
                OnReceiveSetSpeaker += B.OnSetSpeaker;
            });
        }

        void Start()
        {
            YarnProgram YarnAsset = DialogueDB.GetYarnAssetByKey("Starting");
            if (YarnAsset != null)
                DialogueRunner.Add(YarnAsset);
            else
            {
                Debug.LogError("YarnAsset not found");
                return;
            }

            DialogueRunner.StartDialogue();
            Player.SetMovementMode(InputMode.UI);
            TransientDialogueBox.Init(DialogueUI);
        }

        public void InitiateDialogue(string YarnAssetName, List<ITalkable> InSpeakers, string StartNodeName = "Start")
        {
            foreach(ITalkable I in InSpeakers)
                JoinConversation(I);

            YarnProgram YarnAsset = DialogueDB.GetYarnAssetByKey(YarnAssetName);
            if (YarnAsset != null)
            {
                DialogueRunner.Add(YarnAsset);
                DialogueRunner.StartDialogue(StartNodeName);
                Player.SetMovementMode(InputMode.UI);
            }
            else
            {
                Debug.LogError("YarnAsset not found.");
                return;
            }
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
            Player.SetMovementMode(InputMode.Game);

            DialogueRunner.Clear();
        }
    }
}
