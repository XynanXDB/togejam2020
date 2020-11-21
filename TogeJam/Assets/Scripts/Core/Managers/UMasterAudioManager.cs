using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SystemInit")]

namespace Game.Core
{
    public class UMasterAudioManager : MonoBehaviour
    {
        public static UMasterAudioManager MasterAudioManager { get; internal set; }
        public AudioSource BGMAudioSource { get; private set; }
        public AudioSource SFXAudioSource { get; private set; }
        [SerializeField] protected AudioMixer MasterMixer;

        public static readonly string MasterVolumeName = "MasterVolume";
        public static readonly string MusicVolumeName = "MusicVolume";
        public static readonly string SFXVolumeName = "SFXVolume";

    //////////////////////////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            DontDestroyOnLoad(this);
            BGMAudioSource = transform.Find("BGMAudioManager").GetComponent<AudioSource>();
            SFXAudioSource = transform.Find("SFXAudioManager").GetComponent<AudioSource>();
        }

        public void SetMusicVolume (float InMusicVolume) => MasterMixer.SetFloat(SFXVolumeName, InMusicVolume);
        public void SetMasterVolume (float InMasterVolume) => MasterMixer.SetFloat(MasterVolumeName, InMasterVolume);
        public void SetSFXVolume (float InSFXVolume) => MasterMixer.SetFloat(SFXVolumeName, InSFXVolume);
        public void PlayBGM(AudioClip Clip, bool bLoop = true)
        {
            BGMAudioSource.clip = Clip;
            BGMAudioSource.loop = bLoop;
            BGMAudioSource.Play();
        }
        public void PlaySFX(AudioClip Clip)
        {
            SFXAudioSource.clip = Clip;
            SFXAudioSource.Play();
        }
    }
}