
using UnityEngine;
using UnityEngine.Playables;
using Game.Utility;

namespace Game.Core
{
    public class UPlayableDirector : MonoBehaviour
    {
        [SerializeField] protected UPlayableDataBase PlayableDB;
        [SerializeField] protected PlayableDirector Director;
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

        public void PlayCinematic(string Name)
        {
            PlayableAsset Asset = PlayableDB.GetPlayableAssetByKey(Name);
            if (Asset != null)
                Director.Play(Asset);
        }
    }
}