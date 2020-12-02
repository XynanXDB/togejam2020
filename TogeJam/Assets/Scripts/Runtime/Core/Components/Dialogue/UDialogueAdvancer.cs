using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.Core
{
    public class UDialogueAdvancer : MonoBehaviour
    {
        [SerializeField] protected Animator Animator;
        [SerializeField] protected Transform ParentTransform;
        static readonly string Start = "Start";
        private DialogueUI DialogueUI;
        private string SpeakerName;
        
//////////////////////////////////////////////////////////////////////////////////

        void OnEnable()
        {
            Animator.Play(Start);
        }

        public void AssignSpeaker(DialogueUI UI, string InSpeakerName)
        {
            DialogueUI = UI;
            SpeakerName = InSpeakerName;

            if (DialogueUI != null)
            {
                DialogueUI.onLineStart.AddListener(OnLineStart);
                DialogueUI.onOptionsStart.AddListener(OnOptionsStart);
                DialogueUI.onOptionsEnd.AddListener(OnOptionsEnd);
            }
            else
            {
                DialogueUI.onLineStart.RemoveListener(OnLineStart);
                DialogueUI.onOptionsStart.RemoveListener(OnOptionsStart);
                DialogueUI.onOptionsEnd.RemoveListener(OnOptionsEnd);
            }

            OnOptionsStart();
        }

        void ForceFocusOnAdvancer() => UGameInstance.GameInstance.ForceFocusGameObject(gameObject);

        void OnOptionsStart() => UGameInstance.GameInstance.ForceFocusGameObject(null);
        void OnOptionsEnd() => UGameInstance.GameInstance.ForceFocusGameObject(gameObject);

        void OnLineStart()
        {
            gameObject.SetActive(false);
        }

        public void OnClick()
        {
            DialogueUI.MarkLineComplete();
        }
    }
}