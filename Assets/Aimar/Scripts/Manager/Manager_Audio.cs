using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimarWork
{
    public class Manager_Audio : MonoBehaviour
    {
        public enum ENUM_AudioGeneralType
        {
            Click,
            Toggle,
            Switch
        }
        [SerializeField] private AudioSource MusicSource;
        [SerializeField] private AudioSource SFXSource;

        public static Manager_Audio instance;

        public static event Action<bool> MuteMusic;
        public static event Action<bool> MuteSFX;

        [Title("Audio List")]
        SerializableDictionary<ENUM_AudioGeneralType, AudioClip> AudioGeneral;
        public bool MusicMute { get; private set; }
        public bool SFXMute { get; private set; }
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public async void PlayMusic(AudioClip audioClip)
        {
            await MusicSource.DOFade(0, 0.3f).AsyncWaitForCompletion();

            
            MusicSource.clip = audioClip;
            MusicSource.Play();  

            await MusicSource.DOFade(1, 0.3f).AsyncWaitForCompletion();
        }
        public async void StopPlayingMusic()
        {
            await MusicSource.DOFade(0, 0.6f).AsyncWaitForCompletion();
        }
        public void PlaySFX(AudioClip audioClip)
        {
            SFXSource.PlayOneShot(audioClip);
        }
        public void PlaySFX(ENUM_AudioGeneralType type)
        {
            SFXSource.PlayOneShot(AudioGeneral[type]);
        }
        public void ToggleSFX(bool value)
        {
            SFXSource.mute = !value;
            MusicMute = SFXSource.mute;
            MuteSFX?.Invoke(MusicMute);
        }
        public void ToggleMusic(bool value)
        {
            MusicSource.mute = !value;
            SFXMute = SFXSource.mute;
            MuteMusic?.Invoke(SFXMute);
        }
    }
}

