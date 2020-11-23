using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.Core
{
    public class UDialogueAdvancer : MonoBehaviour
    {
        [SerializeField] protected Button AdvanceButton;
        [SerializeField] protected Animator Animator;
        private DialogueUI DialogueUI;
        static readonly string Start = "Start";
        
//////////////////////////////////////////////////////////////////////////////////

        public void SetDialogueUI(DialogueUI UI)
        {
            DialogueUI = UI;

            if (DialogueUI != null)
            {
                DialogueUI.onLineStart.AddListener(OnLineStart);
                DialogueUI.onLineFinishDisplaying.AddListener(OnLineFinishDisplay);
                DialogueUI.onOptionsStart.AddListener(OnOptionsStart);
                DialogueUI.onOptionsEnd.AddListener(OnOptionsEnd);
            }
            else
            {
                DialogueUI.onLineStart.RemoveListener(OnLineStart);
                DialogueUI.onLineFinishDisplaying.RemoveListener(OnLineFinishDisplay);
                DialogueUI.onOptionsStart.RemoveListener(OnOptionsStart);
                DialogueUI.onOptionsEnd.RemoveListener(OnOptionsEnd);
            }

            OnOptionsStart();
        }

        void OnLineFinishDisplay()
        {
            gameObject.SetActive(true);
            Animator.Play(Start);
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