using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

namespace Game.Core
{
    [System.Serializable]
    public class DialogueDictionary : SerializableDictionaryBase<string, YarnProgram>
    {}

    [CreateAssetMenu(fileName = "DialogueDB", menuName = "ScriptableObjects/Create Dialogue DB", order = 1)]
    public class UDialogueDataBase : ScriptableObject
    {
        [SerializeField] DialogueDictionary DialogueDictionary = null;

        public YarnProgram GetYarnAssetByKey(string Key)
        {
            YarnProgram YarnAsset = null;
            DialogueDictionary.TryGetValue(Key, out YarnAsset);

            return YarnAsset;
        }
    }
}
