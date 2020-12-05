using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public class UCredits : MonoBehaviour
    {
        [SerializeField] protected Button ReturnToMainMenu;

        void OnEnable()
        {
            ReturnToMainMenu.gameObject.SetActive(false);
            StartCoroutine(WaitandShowRTMButton());
        }

        public void OnClickReturnToMainMenu()
        {
            UGameInstance.GameInstance.ForceFocusGameObject(null);
            SceneManager.LoadScene("Level01", LoadSceneMode.Single);
        }

        IEnumerator WaitandShowRTMButton()
        {
            yield return new WaitForSeconds(5.0f);
            ReturnToMainMenu.gameObject.SetActive(true);
            UGameInstance.GameInstance.ForceFocusGameObject(ReturnToMainMenu.gameObject);
        }
    }
}