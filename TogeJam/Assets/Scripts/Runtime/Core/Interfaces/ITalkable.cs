﻿using UnityEngine;

namespace Game.Core
{
    [System.Serializable]
    public struct FSpeakerInfo
    {
        public string Name;
        public Transform SpeechTransform;
    }

    public interface ITalkable 
    {
        FSpeakerInfo GetSpeakerInfo();
    }
}