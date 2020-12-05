using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class UMainMenu : MonoBehaviour
    {
        [SerializeField] protected GameObject Credits;

        public void OnClickPlay()
        {
            gameObject.SetActive(false);
            UPlayableDirector.PlayableDirector.PlayCinematic("Beat1_Start");
        }

        public void OnClickCredits()
        {
            gameObject.SetActive(false);
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