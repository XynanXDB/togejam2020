using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System.Runtime.CompilerServices;
using Game.Utility;
using Game.Library.Delegate;

[assembly: InternalsVisibleTo("SystemInit")]

namespace Game.Core
{
    public struct YarnCommandPacket
    {
        public string name;
        public string Action;
        public string Animation;

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
        public static UDialogueManager DialogueManager { get; internal set; }
        
        [SerializeField] protected DialogueRunner DialogueRunner;
        [SerializeField] protected UObjectPooler BubblePooler;
        [SerializeField] protected UDialogueDataBase DialogueDB;
        [SerializeField] protected DialogueUI DialogueUI;
        [SerializeField] protected UDialogueOptionGroup OptionGroup;
        private List<UDialogueBubble> AssignedBubbles = null;
        private OneParamSignature<YarnCommandPacket> OnReceiveSetSpeaker = null;

        [Header("Debug")]
        [SerializeField] protected GameObject Player;


///////////////////////////////////////////////////////////////////////
        
        void Awake()
        {
            DontDestroyOnLoad(this);
            DialogueRunner.AddCommandHandler("SetSpeaker", SetSpeaker);
            AssignedBubbles = new List<UDialogueBubble>();

            BubblePooler.GetAllPooledObjects().ForEach( GO =>
            {
                UDialogueBubble B = GO.GetComponent<UDialogueBubble>();
                OnReceiveSetSpeaker += B.OnSetSpeaker;
            });
        }

        void Start()
        {
            ITalkable I;
            if((I = Player.GetComponent<ITalkable>()) != null)
                JoinConversation(I);

            DialogueRunner.Add(DialogueDB.GetYarnAssetByKey("Billy"));
            DialogueRunner.StartDialogue("Billy.Start");
        }

        // Name -> Think/Talk -> Animation
        void SetSpeaker(string[] Data) => OnReceiveSetSpeaker?.Invoke(new YarnCommandPacket(Data[0], Data[1], Data[2]));

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
            UGameInstance.GameInstance.ForceFocusGameObject(null);
            OptionGroup.gameObject.SetActive(true);
        }
    }
}
