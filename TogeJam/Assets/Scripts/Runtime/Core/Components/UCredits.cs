using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class UCredits : MonoBehaviour
    {
        [SerializeField] protected Button QuitGame;

        void OnEnable()
        {
            QuitGame.gameObject.SetActive(false);
            StartCoroutine(WaitandShowRTMButton());
        }

        public void OnClickQuitGame()
        {
            UGameInstance.GameInstance.ForceFocusGameObject(null);
            Application.Quit();
        }

        IEnumerator WaitandShowRTMButton()
        {
            yield return new WaitForSeconds(5.0f);
            QuitGame.gameObject.SetActive(true);
            UGameInstance.GameInstance.ForceFocusGameObject(QuitGame.gameObject);
        }
    }
}