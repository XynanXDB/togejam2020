using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
namespace Game.Core
{
    [System.Serializable]
    public struct PlayableDBStruct
    {
        public string Key;
        public PlayableAsset PlayableAsset;
    }
    
    [CreateAssetMenu(fileName = "PlayableDB", menuName = "ScriptableObjects/Create Playable DB", order = 1)]
    public class UPlayableDataBase : ScriptableObject
    {
        [SerializeField] List<PlayableDBStruct> PlayableDB = null;

        public PlayableAsset GetPlayableAssetByKey(string Key)
        {
            PlayableAsset PlayableAsset = PlayableDB.Find(Item => Item.Key == Key).PlayableAsset;     
            return PlayableAsset;
        }
    }
}