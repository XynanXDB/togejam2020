
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using Game.Library.Delegate;
using System;

namespace Game.Core
{
    public class UPlayableDirector : MonoBehaviour
    {
        [SerializeField] protected UPlayableDataBase PlayableDB;
        [SerializeField] protected PlayableDirector Director;
        public VoidSignature OnStop;

        private static UPlayableDirector _PlayableDirector;
        public static UPlayableDirector  PlayableDirector
        {
            get 
            {
                if (_PlayableDirector == null)
                    _PlayableDirector = FindObjectOfType<UPlayableDirector>();
                
                return _PlayableDirector;
            }
        }

        void Awake() => Director.stopped += OnPlayableStop;
        void OnDestroy() => Director.stopped -= OnPlayableStop;

        void Start()
        {
            PlayCinematic("MainMenu");
        }

        void OnPlayableStop(PlayableDirector InDirector)
        {
            OnStop?.Invoke();
            OnStop = null;
        }

        public void PlayCinematic(string Name, VoidSignature OnStopCallback = null)
        {
            PlayableAsset Asset = PlayableDB.GetPlayableAssetByKey(Name);
            if (Asset != null)
            {
                if (OnStopCallback != null)
                    OnStop = OnStopCallback;

                Director.Play(Asset);
            }
        }
    }
}