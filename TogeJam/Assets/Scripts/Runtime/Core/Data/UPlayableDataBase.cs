using UnityEngine;
using UnityEngine.Playables;
using RotaryHeart.Lib.SerializableDictionary;

namespace Game.Core
{
    [System.Serializable]
    public class PlayableDictionary : SerializableDictionaryBase<string, PlayableAsset>
    {}

    [CreateAssetMenu(fileName = "PlayableDB", menuName = "ScriptableObjects/Create Playable DB", order = 1)]
    public class UPlayableDataBase : ScriptableObject
    {
        [SerializeField] PlayableDictionary PlayableDictionary = null;

        public PlayableAsset GetPlayableAssetByKey(string Key)
        {
            PlayableAsset PlayableAsset = null;
            PlayableDictionary.TryGetValue(Key, out PlayableAsset);
            
            return PlayableAsset;
        }
    }
}