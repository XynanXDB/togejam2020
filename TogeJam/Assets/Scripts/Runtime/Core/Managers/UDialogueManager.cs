using System.Collections;
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

///////////////////////////////////////////////////////////////////////////////

        public YarnCommandPacket(string InName, string InAction)
        {
            name = InName;
            Action = InAction;
        }

        public override bool Equals(object obj)
        {
            return obj is YarnCommandPacket packet &&
                   name == packet.name &&
                   Action == packet.Action;
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
            return (Self.name == Other.name && Self.Action == Other.Action);
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
        [SerializeField] protected UStreetLoopManager StreetLoopManager;
        [SerializeField] protected UDialogueDispatcher DialogueDispatcher;
        private List<UDialogueBubble> AssignedBubbles = null;
        private List<ITalkable> JoinedSpeakers;
        public OneParamSignature<YarnCommandPacket> OnReceiveSetSpeaker = null;
        public VoidSignature OnCustomDialogueEnd = null;
        private int TwoToStartBeat4Park = 0;
        [SerializeField] protected AudioClip Doorbell;//Should make a database for this
        [SerializeField] protected AudioClip SS_Main;
        [SerializeField] protected AudioClip WalkTheDog;
        [SerializeField] protected GameObject Credits = null;

        [Header("Debug")]
        [SerializeField] protected UPlayerController Player;
        private ITalkable Dog;

///////////////////////////////////////////////////////////////////////
        
        void Awake()
        {
            DontDestroyOnLoad(this);
            DialogueRunner.AddCommandHandler("SetSpeaker", SetSpeaker);
            DialogueRunner.AddCommandHandler("SetAnimation", SetAnimation);
            DialogueRunner.AddCommandHandler("OffBubble", OffBubble);
            DialogueRunner.AddCommandHandler("TurnDog", TurnDog);
            DialogueRunner.AddCommandHandler("Play", PlayYarn);
            DialogueRunner.AddCommandHandler("StopStreetLoop", StopStreetLoop);
            DialogueRunner.AddCommandHandler("PlayScene", PlayCinematic);
            DialogueRunner.AddCommandHandler("IncrementStartBeat4Park", IncrementStartBeat4Park);
            DialogueRunner.AddCommandHandler("PlaySound", PlaySound);
            DialogueRunner.AddCommandHandler("PlayMusic", PlayMusic);
            DialogueRunner.AddCommandHandler("ShowCredits", ShowCredits);
            
            DialogueUI.onDialogueEnd.AddListener(OnDialogueEnd);
            
            AssignedBubbles = new List<UDialogueBubble>();
            JoinedSpeakers = new List<ITalkable>();
            Dog = GameObject.FindGameObjectWithTag("Dog").GetComponent<ITalkable>();
            
            BubblePooler.GetAllPooledObjects().ForEach( GO =>
            {
                UDialogueBubble B = GO.GetComponent<UDialogueBubble>();
                OnReceiveSetSpeaker += B.OnSetSpeaker;
            });
        }

        void ShowCredits(string[] Data)
        {
            Credits.SetActive(true);
        }

        void PlayMusic(string[] Data)
        {
            if (Data[0] == "SS_Main")
                UMasterAudioManager.MasterAudioManager.PlayBGM(SS_Main, true);
            else if (Data[0] == "WTD")
                UMasterAudioManager.MasterAudioManager.PlayBGM(WalkTheDog, true);
        }

        void PlaySound(string[] Data)
        {
            if (Data[0] == "Doorbell")
                UMasterAudioManager.MasterAudioManager.PlaySFX(Doorbell);
        }

        void StopStreetLoop(string[] Data)
        {
            StreetLoopManager.StopLoop();
        }
        public void IncrementStartBeat4Park(string[] Data = null)
        {
            TwoToStartBeat4Park ++;

            if (TwoToStartBeat4Park == 2)
                PlayYarn(new string[1]{"Beat4.Park"});
        }

        void PlayCinematic(string[] Data)
        {
            if (Data[0] == "Beat5.ClientHouse")
            {
                UPlayableDirector.PlayableDirector.OnStop = ()=>
                {
                    PlayYarn(new string[1]{"Beat5.ClientHouse"});
                };
                UPlayableDirector.PlayableDirector.PlayCinematic("Beat5");
            }
            else if (Data[0] == "NeutralCredits")
            {
                UPlayableDirector.PlayableDirector.OnStop = () =>
                {
                    Credits.SetActive(true);
                };
                UPlayableDirector.PlayableDirector.PlayCinematic(Data[0]);
                Player.SetAnimation("Stand");
            }
            else if (Data [0] == "GoodCredits")
            {
                UPlayableDirector.PlayableDirector.PlayCinematic(Data[0]);
            }
        }

        void PlayYarn(string[] Data)
        {
            DialogueRunner.Stop();
            OnDialogueEnd();

            StartCoroutine(WaitThenPlayYarn(Data[0]));
        }
        IEnumerator WaitThenPlayYarn(string Data)
        {
            yield return new WaitForSeconds(1.0f);

            // if (Data == "Beat1.Street")
            //     InitiateDialogue(Data, new List<ITalkable>(){Player, Dog}, "Funnel");
            // else if (Data == "Beat2.Street")
            //     InitiateDialogue(Data, new List<ITalkable>(){Player, Dog}, "FunnelB");
            // else
                InitiateDialogue(Data, new List<ITalkable>(){Player, Dog});
        }

        void TurnDog(string[] Data)
        {
            ITalkable I = JoinedSpeakers.Find(S => S.GetSpeakerInfo().Name == "Dog");
            I?.SendNativeCommand("TurnDog." + Data[0]);
        }

        void Start() //TODO Use GameMode to control this
        {
            UPlayableDirector.PlayableDirector.PlayCinematic("Beat1_Start");
        }

        void OffBubble(string[] Data)
        {
            UDialogueBubble Bubble = AssignedBubbles.Find( B => B.GetSpeakerName == Data[0]);
            if (Bubble != null)
                Bubble.DisableBubble();
        }

        public void SetAnimation(string[] Data)
        {
            foreach (string Command in Data)
            {
                string[] Segments = Command.Split('.');
                
                ITalkable Target = JoinedSpeakers.Find( I => I.GetSpeakerInfo().Name == Segments[0]);
                if (Target != null)
                    Target.SetAnimation(Segments[1]);
            }
        }

        void StartDialogue(string YarnAssetName, string StartNodeName = "Start")
        {
            YarnProgram YarnAsset = DialogueDB.GetYarnAssetByKey(YarnAssetName);
            if (YarnAsset != null)
            {
                DialogueRunner.Add(YarnAsset);
                DialogueRunner.StartDialogue(StartNodeName);
                Player.SetMovementMode(InputMode.UI);

                if (YarnAssetName == "Starting")
                    TransientDialogueBox.Init(DialogueUI);
            }
            else
            {
                Debug.LogError("YarnAsset not found");
                return;
            }
        }

        public void InitiateDialogue(string YarnAssetName, List<ITalkable> InSpeakers = null, string StartNodeName = "Start")
        {
            JoinedSpeakers = InSpeakers;
            
            if (InSpeakers != null)
            {
                foreach(ITalkable I in InSpeakers)
                    JoinConversation(I);
            }

            StartDialogue(YarnAssetName, StartNodeName);
        }

        // Name -> Think/Talk
        void SetSpeaker(string[] Data)
        {
            List<string> Commands = new List<string>() { null, null };

            for(int i = 0; i < Data.Length; i++)
                Commands[i] = Data[i];
            
           OnReceiveSetSpeaker?.Invoke(new YarnCommandPacket(Commands[0], Commands[1]));
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
            DialogueRunner.Clear();

            AssignedBubbles.ForEach(B => {
                B.UnassignSpeaker();
            });

            TransientDialogueBox.Disable();
            AssignedBubbles.Clear();

            if (JoinedSpeakers != null)
                JoinedSpeakers.Clear();

            OnCustomDialogueEnd?.Invoke();
            OnCustomDialogueEnd = null;
        }
    }
}
