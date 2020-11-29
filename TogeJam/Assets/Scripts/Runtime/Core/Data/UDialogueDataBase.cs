using UnityEngine;
using System.Collections.Generic;

namespace Game.Core
{
    [System.Serializable]
    public struct DialogueDBStruct
    {
        public string Key;
        public YarnProgram YarnAsset;
    }

    [CreateAssetMenu(fileName = "DialogueDB", menuName = "ScriptableObjects/Create Dialogue DB", order = 1)]
    public class UDialogueDataBase : ScriptableObject
    {
        [SerializeField] List<DialogueDBStruct> DialogueDB;

        public YarnProgram GetYarnAssetByKey(string Key)
        {
            YarnProgram YarnAsset = DialogueDB.Find( Item => Item.Key == Key).YarnAsset;
            return YarnAsset;
        }
    }
}
