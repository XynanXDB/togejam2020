using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

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

            DialogueUI.onLineStart.AddListener(OnLineStart);
            DialogueUI.onLineFinishDisplaying.AddListener(OnLineFinishDisplay);
        }

        void OnLineFinishDisplay()
        {
            gameObject.SetActive(true);
            Animator.Play(Start);
        }

        void ForceFocusOnAdvancer() => UGameInstance.GameInstance.ForceFocusGameObject(gameObject);

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