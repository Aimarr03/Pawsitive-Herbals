using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimarWork
{
    public class Manager_Audio : MonoBehaviour
    {
        [SerializeField] private AudioSource MusicSource;
        [SerializeField] private AudioSource SFXSource;

        public static Manager_Audio instance;

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
        public void PlaySFX(AudioClip audioClip)
        {
            SFXSource.PlayOneShot(audioClip);
        }
        public void ToggleSFX(bool value) => SFXSource.mute = value;
        public void ToggleMusic(bool value) => MusicSource.mute = value;
    }
}

