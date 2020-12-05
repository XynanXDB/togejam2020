using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

namespace Game.Core
{
    public class UMainMenu : MonoBehaviour
    {
        [SerializeField] protected GameObject Credits;
        [SerializeField] protected CinemachineVirtualCamera MainMenuVCam;

        void OnEnable()
        {
            MainMenuVCam.Priority = 10;
        }

        void OnDisable()
        {
            MainMenuVCam.Priority = 0;
        }

        void Disable()
        {
            StartCoroutine(WaitandChangePriority());
        }

        IEnumerator WaitandChangePriority()
        {
            yield return new WaitForSeconds(1.0f);
            gameObject.SetActive(false);
        }

        public void OnClickPlay()
        {
            Disable();

            UPlayableDirector.PlayableDirector.PlayCinematic("Beat1_Start");
        }

        public void OnClickCredits()
        {
            Disable();

            UPlayableDirector.PlayableDirector.OnStop = () =>
                {
                    Credits.SetActive(true);
                };
            UPlayableDirector.PlayableDirector.PlayCinematic("NeutralCredits");
        }

        public void OnClickQuit()
        {
            Application.Quit();
        }
    }
}