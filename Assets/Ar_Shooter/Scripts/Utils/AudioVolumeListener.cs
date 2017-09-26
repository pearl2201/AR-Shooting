using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Dinosaur.Scripts;
namespace Stater
{
    public class AudioVolumeListener : MonoBehaviour
    {

        public AUDIOTYPE typeListen = AUDIOTYPE.SOUND;

        private AudioSource audioSource;

        [SerializeField]
        private float scale = 1;
        private void Start()
        {
			
            audioSource = GetComponent<AudioSource>();
            if (audioSource!=null)
            {
                if (typeListen == AUDIOTYPE.MUSIC)
                {
                    audioSource.volume = MusicManager.Instance.volume*scale;
                }
                else if (typeListen == AUDIOTYPE.SOUND)
                {
                    audioSource.volume = SoundManager.Instance.volume * scale;
                }
            }

        }

        public void UpdateMusicVolume()
        {
            if (audioSource != null)
                audioSource.volume = MusicManager.Instance.volume * scale;
        }

        public void UpdateSoundVolume()
        {
            if (audioSource != null)
                audioSource.volume = SoundManager.Instance.volume * scale;
        }

        private void OnEnable()
        {
            if (audioSource != null)
            {
                if (typeListen == AUDIOTYPE.MUSIC)
                {
                    this.RegisterListener(EventID.OnMusicChange, (sender, param) => UpdateMusicVolume());
                    this.RegisterListener(EventID.OnFadeMusic, (sender, param) => UpdateMusicVolume());
                }
                else if (typeListen == AUDIOTYPE.SOUND)
                {
                    this.RegisterListener(EventID.OnSoundChange, (sender, param) => UpdateSoundVolume());

                }
            }
        }

        private void OnDisable()
        {
            /*
            if (typeListen == AUDIOTYPE.MUSIC)
            {
                EventDispatcher.Instance.RemoveListener(EventID.OnMusicChange, (sender, param) => UpdateMusicVolume());
                EventDispatcher.Instance.RemoveListener(EventID.OnFadeMusic, (sender, param) => UpdateMusicVolume());
            }
            else if (typeListen == AUDIOTYPE.SOUND)
            {
                EventDispatcher.Instance.RemoveListener(EventID.OnSoundChange, (sender, param) => UpdateSoundVolume());

            }
            */
        }

        public void RemoveListener()
        {

            

        }
    }

    public enum AUDIOTYPE
    {
        SOUND,
        MUSIC
    }
}
